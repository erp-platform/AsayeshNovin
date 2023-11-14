using Core.Application.Services;
using Core.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UMS.Authentication.Application.Interfaces;
using UMS.Authentication.Application.Utility;
using UMS.Authentication.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Scrypt;
using UMS.Authentication.Application.Authorization;
using UMS.Authentication.Application.Dtos;
using UMS.Authentication.Application.Dtos.AuthDtos;
using UMS.Authentication.Application.Dtos.PasswordResetDtos;
using UMS.Authentication.Application.Dtos.UserChannelDtos;
using UMS.Authentication.Application.Dtos.UserDtos;
using UMS.Authentication.Application.Dtos.VerificationDtos;

namespace UMS.Authentication.Application.Services;

public class AuthService : IAuthService
{
    private readonly IConfigurationRoot _configuration =
        new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

    private readonly HttpClient _httpClient;

    private readonly IBaseService<UserChannel, UserChannelCreateDto, UserChannelUpdateDto> _userChannelService;
    private readonly IBaseService<Verification, VerificationCreateDto, VerificationUpdateDto> _verificationService;
    private readonly IBaseService<User, UserCreateDto, UserUpdateDto> _userService;
    private readonly IBaseService<PasswordReset, PasswordResetCreateDto, PasswordResetUpdateDto> _passwordResetService;

    private readonly IBaseRepository<Channel> _channelRepository;

    private readonly ScryptEncoder _encoder = new();

    private readonly IJwtUtils _jwtUtils;

    public AuthService(IBaseService<UserChannel, UserChannelCreateDto, UserChannelUpdateDto> userChannelService,
        IBaseService<Verification, VerificationCreateDto, VerificationUpdateDto> verificationService,
        IBaseRepository<Channel> channelRepository,
        IBaseService<User, UserCreateDto, UserUpdateDto> userService,
        IBaseService<PasswordReset, PasswordResetCreateDto, PasswordResetUpdateDto> passwordResetService,
        HttpClient httpClient,
        IJwtUtils jwtUtils)
    {
        _userChannelService = userChannelService;
        _verificationService = verificationService;
        _httpClient = httpClient;
        _jwtUtils = jwtUtils;
        _passwordResetService = passwordResetService;
        _userService = userService;
        _channelRepository = channelRepository;
    }

    public async Task<ResponseDto<UserChannelResponseDto>> SignUp(SignUpDto signUpDto)
    {
        var previousUserChannel = _userChannelService.GetAllAsync().Result.FirstOrDefault(userChannel =>
            userChannel?.Channel.AId == signUpDto.ChannelId &&
            userChannel.Value == signUpDto.Value, null);
        var userChannel = await (previousUserChannel == null
            ? HandleNewRegister(signUpDto)
            : HandlePreviousRegister(previousUserChannel));
        await SendVerificationCodeToChannel(userChannel);
        return new ResponseDto<UserChannelResponseDto>
        {
            Data = new UserChannelResponseDto
            {
                Id = userChannel.Id,
                Value = userChannel.Value,
                IsDefault = userChannel.IsDefault,
                UserId = userChannel.User?.Id,
                Channel = userChannel.Channel.AId,
                VerificationId = userChannel.Verification?.Id,
                PasswordResets = null,
                Logins = null
            }
        };
    }

    public async Task<ResponseDto<UserChannelResponseDto>> Verify(VerifyDto verifyDto)
    {
        var userChannel = await _userChannelService.FindByIdAsync(verifyDto.UserChannelId);
        var verification = userChannel?.Verification;
        if (verification == null)
        {
            throw Helpers.CreateAuthException(
                error: AuthServiceError.UserChannelWithoutVerification,
                httpCode: StatusCodes.Status404NotFound,
                data: verifyDto
            );
        }

        if (userChannel == null)
        {
            throw Helpers.CreateAuthException(
                error: AuthServiceError.UserChannelNotFound,
                httpCode: StatusCodes.Status404NotFound,
                data: verifyDto
            );
        }

        if (verification.Code != verifyDto.Code)
        {
            throw Helpers.CreateAuthException(
                error: AuthServiceError.VerificationCodeIncorrect,
                data: verifyDto
            );
        }

        CheckIsVerified(verification);

        await VerifyUserChannel(userChannel);
        return new ResponseDto<UserChannelResponseDto>
        {
            Data = new UserChannelResponseDto
            {
                Id = userChannel.Id,
                Value = userChannel.Value,
                IsDefault = userChannel.IsDefault,
                UserId = userChannel.User?.Id,
                Channel = userChannel.Channel.AId,
                VerificationId = userChannel.Verification?.Id,
                PasswordResets = null,
                Logins = null
            }
        };
    }

    public async Task<ResponseDto<UserResponseDto>> SetCredential(CredentialDto credentialDto)
    {
        var userChannel = await _userChannelService.FindByIdAsync(credentialDto.UserChannelId);
        if (userChannel == null)
        {
            throw Helpers.CreateAuthException(
                error: AuthServiceError.UserChannelNotFound,
                httpCode: StatusCodes.Status404NotFound,
                data: credentialDto
            );
        }

        if (userChannel.User != null)
        {
            throw Helpers.CreateAuthException(
                error: AuthServiceError.CredentialAlreadySet,
                data: credentialDto
            );
        }

        var verification = userChannel.Verification;
        if (verification == null)
        {
            throw Helpers.CreateAuthException(
                error: AuthServiceError.UserChannelWithoutVerification,
                httpCode: StatusCodes.Status404NotFound,
                data: credentialDto
            );
        }

        if (!verification.IsVerified)
        {
            throw Helpers.CreateAuthException(
                error: AuthServiceError.UserChannelNotVerified,
                httpCode: StatusCodes.Status401Unauthorized,
                data: credentialDto
            );
        }

        var user = await _userService.CreateAsync(new UserCreateDto
        {
            Username = credentialDto.Username,
            Password = credentialDto.Password,
            VerificationId = verification.Id
        });

        if (user == null)
        {
            throw Helpers.CreateAuthException(
                error: AuthServiceError.DatabaseError,
                httpCode: StatusCodes.Status500InternalServerError,
                data: credentialDto
            );
        }

        await _userChannelService.UpdateAsync(userChannel.Id, new UserChannelUpdateDto
        {
            User = user
        });

        return new ResponseDto<UserResponseDto>
        {
            Data = new UserResponseDto
            {
                Username = user.Username,
                VerificationId = user.VerificationId,
                Channels = user.Channels.Select(u => new UserChannelResponseDto
                {
                    Id = u.Id,
                    Value = u.Value,
                    IsDefault = u.IsDefault,
                    Channel = u.Channel.AId
                })
            }
        };
    }

    public async Task<ResponseDto<AuthLoginResponseDto>> Login(AuthLoginDto authLoginDto)
    {
        var user = (await _userService.GetAllAsync()).FirstOrDefault(u =>
            u?.Username == authLoginDto.Username &&
            _encoder.Compare(authLoginDto.Password, u.Password), null);
        if (user == null)
        {
            throw Helpers.CreateAuthException(
                error: AuthServiceError.UsernamePasswordError,
                data: authLoginDto,
                httpCode: StatusCodes.Status401Unauthorized
            );
        }

        return new ResponseDto<AuthLoginResponseDto>
        {
            Data = new AuthLoginResponseDto
            {
                User = new UserResponseDto
                {
                    Username = user.Username,
                    VerificationId = user.VerificationId,
                    Channels = user.Channels.Select(a => new UserChannelResponseDto
                    {
                        Id = a.Id,
                        Channel = a.Channel.AId,
                        IsDefault = a.IsDefault,
                        Value = a.Value,
                        VerificationId = a.Verification?.Id
                    })
                },
                Token = _jwtUtils.GenerateJwtToken(user)
            }
        };
    }

    public async Task<ResponseDto<PasswordResetRequestDto>> PasswordResetRequest(
        PasswordResetRequestDto passwordResetRequestDto)
    {
        var result = Task.FromResult(passwordResetRequestDto);

        var userChannel = (await _userChannelService.GetDbSet()
            .Where(uc => uc.Channel.AId == passwordResetRequestDto.ChannelId)
            .Where(uc => uc.Value == passwordResetRequestDto.Value)
            .ToListAsync()).FirstOrDefault();
        if (userChannel == null)
        {
            return new ResponseDto<PasswordResetRequestDto>
            {
                Data = await result
            };
        }

        var token = Helpers.GeneratePasswordResetToken();
        await _passwordResetService.CreateAsync(new PasswordResetCreateDto
        {
            Token = token,
            IsUsed = false,
            UserChannel = userChannel
        });
        //todo refactor
        await SendResetPasswordTokenToChannel(userChannel,
            $"http://asnp.ir/Authentication/PasswordReset/{token}");
        return new ResponseDto<PasswordResetRequestDto>
        {
            Data = await result
        };
    }

    public async Task<ResponseDto<UserResponseDto>> PasswordResetAction(string token,
        PasswordResetAction passwordResetAction)
    {
        var passwordReset = (await _passwordResetService.GetAllAsync())
            .FirstOrDefault(pr => pr?.Token == token && !pr.IsUsed, null);
        var isPasswordResetExpired =
            (DateTime.UtcNow - passwordReset?.CreatedAt)?.TotalSeconds > Constants.PasswordResetTimer;
        if (passwordReset == null || isPasswordResetExpired)
        {
            throw Helpers.CreateAuthException(
                error: AuthServiceError.PasswordResetTokenError,
                httpCode: StatusCodes.Status404NotFound,
                data: new { token }
            );
        }

        var userId = passwordReset.UserChannel?.User?.Id;
        if (userId == null)
        {
            throw Helpers.CreateAuthException(
                error: AuthServiceError.DatabaseError,
                data: new { token },
                httpCode: StatusCodes.Status500InternalServerError
            );
        }

        var user = await _userService.UpdateAsync((Guid)userId, new UserUpdateDto
        {
            Password = passwordResetAction.Password
        });
        await _passwordResetService.UpdateAsync(passwordReset.Id, new PasswordResetUpdateDto
        {
            IsUsed = true
        });
        if (user == null)
        {
            throw Helpers.CreateAuthException(
                error: AuthServiceError.DatabaseError,
                data: new { token },
                httpCode: StatusCodes.Status500InternalServerError
            );
        }

        return new ResponseDto<UserResponseDto>
        {
            Data = new UserResponseDto
            {
                Username = user.Username,
                VerificationId = user.VerificationId,
                Channels = null
            }
        };
    }

    public async Task<ResponseDto<AuthLoginResponseDto>> LoginWithChannel(LoginChannelDto loginChannelDto)
    {
        var userChannel = await _userChannelService.GetDbSet()
            .FirstOrDefaultAsync(uc =>
                uc.Value == loginChannelDto.Value &&
                uc.Channel.AId == loginChannelDto.ChannelId
            );
        var user = userChannel?.User;
        if (user != null)
        {
            if (_encoder.Compare(loginChannelDto.Password, userChannel?.User?.Password))
            {
                return new ResponseDto<AuthLoginResponseDto>
                {
                    Data = new AuthLoginResponseDto
                    {
                        User = new UserResponseDto
                        {
                            Username = user.Username,
                            VerificationId = user.VerificationId,
                            Channels = user.Channels.Select(a => new UserChannelResponseDto
                            {
                                Id = a.Id,
                                Channel = a.Channel.AId,
                                IsDefault = a.IsDefault,
                                Value = a.Value,
                                VerificationId = a.Verification?.Id
                            })
                        },
                        Token = _jwtUtils.GenerateJwtToken(user)
                    }
                };
            }
        }
        else
        {
            throw Helpers.CreateAuthException(
                error: AuthServiceError.DatabaseError,
                httpCode: StatusCodes.Status404NotFound
            );
        }

        throw Helpers.CreateAuthException(
            error: AuthServiceError.UsernamePasswordError,
            httpCode: StatusCodes.Status401Unauthorized
        );
    }

    private async Task<Verification?> VerifyUserChannel(UserChannel userChannel)
    {
        var verification = userChannel.Verification;
        if (verification == null)
        {
            throw Helpers.CreateAuthException(
                error: AuthServiceError.UserChannelWithoutVerification,
                httpCode: StatusCodes.Status404NotFound,
                data: null
            );
        }

        if (IsVerificationValid(verification))
        {
            return await _verificationService.UpdateAsync(
                verification.Id,
                new VerificationUpdateDto { IsVerified = true }
            );
        }

        throw Helpers.CreateAuthException(
            AuthServiceError.VerificationCodeExpired,
            data: null
        );
    }

    private async Task<UserChannel> HandleNewRegister(SignUpDto signUpDto)
    {
        var channel = await GetChannelById(signUpDto.ChannelId);

        var userChannel = await _userChannelService.CreateAsync(new UserChannelCreateDto
        {
            Channel = channel,
            Value = signUpDto.Value
        });
        if (userChannel == null)
        {
            throw Helpers.CreateAuthException(
                AuthServiceError.DatabaseError,
                httpCode: StatusCodes.Status500InternalServerError
            );
        }

        return await UpdateUserChannelRecord(userChannel, await CreateVerificationRecord());
    }

    private async Task<UserChannel> HandlePreviousRegister(UserChannel previousUserChannel)
    {
        var verification = _verificationService.GetAllAsync().Result.LastOrDefault(v =>
                v?.Id == previousUserChannel.Verification?.Id, null
        );
        if (verification == null)
        {
            throw Helpers.CreateAuthException(
                error: AuthServiceError.UserChannelWithoutVerification,
                httpCode: StatusCodes.Status404NotFound,
                data: null
            );
        }

        CheckIsVerified(verification);
        CheckVerificationInterval(verification, previousUserChannel);
        return await UpdateUserChannelRecord(
            previousUserChannel,
            await UpdateVerificationRecord(verification, Helpers.GenerateVerificationCode())
        );
    }

    private static void CheckIsVerified(Verification verification)
    {
        if (verification.IsVerified)
        {
            throw Helpers.CreateAuthException(
                AuthServiceError.VerificationAlreadyVerified,
                data: null
            );
        }
    }

    private async Task<UserChannel> UpdateUserChannelRecord(UserChannel userChannel, Verification verification)
    {
        var updatedUserChannel = await _userChannelService.UpdateAsync(userChannel.Id, new UserChannelUpdateDto
        {
            Verification = verification
        });
        if (updatedUserChannel == null)
        {
            throw Helpers.CreateAuthException(
                AuthServiceError.DatabaseError,
                httpCode: StatusCodes.Status500InternalServerError
            );
        }

        return updatedUserChannel;
    }

    private async Task<Verification> CreateVerificationRecord()
    {
        return await _verificationService.CreateAsync(new VerificationCreateDto
        {
            Code = Helpers.GenerateVerificationCode()
        }) ?? throw Helpers.CreateAuthException(
            AuthServiceError.DatabaseError,
            httpCode: StatusCodes.Status500InternalServerError
        );
    }

    private async Task<Verification> UpdateVerificationRecord(Verification verification, string code)
    {
        return await _verificationService.UpdateAsync(verification.Id, new VerificationUpdateDto
        {
            Code = code
        }) ?? throw Helpers.CreateAuthException(
            AuthServiceError.DatabaseError,
            httpCode: StatusCodes.Status500InternalServerError
        );
    }

    private static void CheckVerificationInterval(Verification verification, UserChannel previousUserChannel)
    {
        if (IsVerificationValid(verification))
        {
            throw Helpers.CreateAuthException(
                AuthServiceError.VerificationIsNotExpired,
                new { UserChannelId = previousUserChannel.Id }
            );
        }
    }

    private static bool IsVerificationValid(Verification verification)
    {
        return (DateTime.UtcNow - verification.UpdatedAt).TotalSeconds <= Constants.SmsTimer;
    }

    //todo refactor
    private async Task SendVerificationCodeToChannel(UserChannel userChannel)
    {
        var name = userChannel.Channel.Name;
        if (name == Domain.Enums.Channel.Email.ToString())
        {
            throw Helpers.CreateAuthException(
                AuthServiceError.NotImplementedError,
                httpCode: StatusCodes.Status501NotImplemented
            );
        }

        if (name == Domain.Enums.Channel.Sms.ToString())
        {
            await HandleVerificationSmsChannel(userChannel);
        }

        if (name == Domain.Enums.Channel.Call.ToString())
        {
            throw Helpers.CreateAuthException(
                AuthServiceError.NotImplementedError,
                httpCode: StatusCodes.Status501NotImplemented
            );
        }
    }

    //todo refactor
    private async Task SendResetPasswordTokenToChannel(UserChannel userChannel, string token)
    {
        var name = userChannel.Channel.Name;
        if (name == Domain.Enums.Channel.Email.ToString())
        {
            throw Helpers.CreateAuthException(
                AuthServiceError.NotImplementedError,
                httpCode: StatusCodes.Status501NotImplemented
            );
        }

        if (name == Domain.Enums.Channel.Sms.ToString())
        {
            await SendSms("Reset-Password", token, userChannel.Value);
        }

        if (name == Domain.Enums.Channel.Call.ToString())
        {
            throw Helpers.CreateAuthException(
                AuthServiceError.NotImplementedError,
                httpCode: StatusCodes.Status501NotImplemented
            );
        }
    }

    private async Task HandleVerificationSmsChannel(UserChannel userChannel)
    {
        if (userChannel.Verification == null)
            throw Helpers.CreateAuthException(AuthServiceError.UnknownError);
        await SendSms("Verify", userChannel.Verification.Code, userChannel.Value);
    }

    private async Task SendSms(string templateKey, string text, string receptor)
    {
        var template = _configuration.GetSection($"SMS-SDK:Template:{templateKey}").Value;
        var api = _configuration.GetSection("SMS-SDK")["Api"];
        if (template != null && api != null)
        {
            var uri = CreateSmsUri(api, template, receptor, text);
            var response = await _httpClient.GetAsync(uri);
            Console.WriteLine($"SMS Request HTTP Code: {response.StatusCode}");
        }
        else throw Helpers.CreateAuthException(AuthServiceError.GeneralError);
    }

    private static string CreateSmsUri(string api, string template, string receptor, string text)
    {
        return
            $"https://api.kavenegar.com/v1/{api}/verify/lookup.json?template={template}&receptor={receptor}&token={text}";
    }

    private async Task<Channel> GetChannelById(int id)
    {
        var channel = (await _channelRepository.GetDbSet()
            .Where(ch => ch.AId == id)
            .ToListAsync()).FirstOrDefault();

        if (channel != null) return channel;
        var names = Enum.GetNames(typeof(Domain.Enums.Channel));
        throw Helpers.CreateAuthException(
            AuthServiceError.ChannelTypeError,
            data: new { names }
        );
    }
}