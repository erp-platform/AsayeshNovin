using Core.Application.Services;
using Core.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using UMS.Authentication.Application.Interfaces;
using UMS.Authentication.Application.Utility;
using UMS.Authentication.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Scrypt;
using UMS.Authentication.Application.Authorization;
using UMS.Authentication.Application.Dtos.Auth;
using UMS.Authentication.Application.Dtos.PasswordResetDtos;
using UMS.Authentication.Application.Dtos.UserChannelDtos;
using UMS.Authentication.Application.Dtos.UserDtos;
using UMS.Authentication.Application.Dtos.VerificationDtos;
using static System.String;

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

    public async Task<UserChannelResponseDto> SignUp(SignUpDto signUpDto)
    {
        var oldUserChannel = _userChannelService.GetAllAsync().Result.FirstOrDefault(userChannel =>
            userChannel?.Channel.AId == signUpDto.ChannelId &&
            userChannel.Value == signUpDto.Value, null);
        var userChannel = await (oldUserChannel == null
            ? HandleNewRegister(signUpDto)
            : HandleOldRegister(oldUserChannel));
        await SendVerificationCodeToChannel(userChannel);
        return new UserChannelResponseDto
        {
            Value = userChannel.Value,
            IsDefault = userChannel.IsDefault,
            UserId = userChannel.User?.Id,
            Channel = userChannel.Channel.AId,
            VerificationId = userChannel.Verification?.Id,
            PasswordResets = null,
            Logins = null
        };
    }

    public async Task<UserChannelResponseDto> Verify(VerifyDto verifyDto)
    {
        var userChannel = await _userChannelService.FindByIdAsync(verifyDto.UserChannelId);
        var verification = userChannel?.Verification;
        if (verification == null)
            throw new Exception(
                $"There's no Verification associated with UserChannel with id: \"{verifyDto.UserChannelId}\"");

        if (userChannel != null && verification.Code == verifyDto.Code)
        {
            if (verification.IsVerified)
                throw new Exception($"Verification with id: \"{verification.Id}\" is already verified.");

            await VerifyUserChannel(userChannel);
            return new UserChannelResponseDto
            {
                Value = userChannel.Value,
                IsDefault = userChannel.IsDefault,
                UserId = userChannel.User?.Id,
                Channel = userChannel.Channel.AId,
                VerificationId = userChannel.Verification?.Id,
                PasswordResets = null,
                Logins = null
            };
        }

        throw new ArgumentException(
            $"Code: {verifyDto.Code} is an incorrect code for UserChannel with id: \"{verifyDto.UserChannelId}\"");
    }

    public async Task<UserResponseDto> SetCredential(CredentialDto credentialDto)
    {
        var userChannel = await _userChannelService.FindByIdAsync(credentialDto.UserChannelId);
        if (userChannel == null)
            throw new ArgumentException($"There's no UserChannel with id: \"{credentialDto.UserChannelId}\"");
        if (userChannel.User != null)
            throw new Exception(
                $"Credential for the User with UserChannel with id: \"{credentialDto.UserChannelId}\" is already set.");
        var verification = userChannel.Verification;
        if (verification == null)
            throw new ArgumentException($"UserChannel with id: \"{userChannel.Id}\" has no Verification.");
        if (!verification.IsVerified)
            throw new ArgumentException(
                $"UserChannel with id: \"{userChannel.Id}\" and Verification with id: \"{verification.Id}\" is not verified.");
        var user = await _userService.CreateAsync(new UserCreateDto
        {
            Username = credentialDto.Username,
            Password = credentialDto.Password,
            VerificationId = verification.Id
        });
        await _userChannelService.UpdateAsync(userChannel.Id, new UserChannelUpdateDto
        {
            User = user
        });
        if (user != null)
        {
            return new UserResponseDto
            {
                Username = user.Username,
                VerificationId = user.VerificationId,
                Channels = null
            };
        }

        throw new Exception(
            $"Failed to create User for UserChannel with id: \"{userChannel.Id}\" and Verification with id: \"{verification.Id}\"");
    }

    public async Task<AuthLoginResponseDto> Login(AuthLoginDto authLoginDto)
    {
        var user = (await _userService.GetAllAsync()).FirstOrDefault(u =>
            u?.Username == authLoginDto.Username &&
            _encoder.Compare(authLoginDto.Password, u.Password), null);
        if (user == null)
            throw new Exception("Username or password is incorrect.");
        return new AuthLoginResponseDto
        {
            User = new UserResponseDto
            {
                Username = user.Username,
                VerificationId = user.VerificationId,
                Channels = user.Channels.Select(a => new UserChannelResponseDto
                {
                    Channel = a.Channel.AId,
                    IsDefault = a.IsDefault,
                    Value = a.Value,
                    VerificationId = a.Verification?.Id
                })
            },
            Token = _jwtUtils.GenerateJwtToken(user)
        };
    }

    public async Task<PasswordResetRequestDto> PasswordResetRequest(PasswordResetRequestDto passwordResetRequestDto)
    {
        var result = Task.FromResult(passwordResetRequestDto);

        var userChannel = (await _userChannelService.GetDbSet()
            .Where(uc => uc.Channel.AId == passwordResetRequestDto.ChannelId)
            .Where(uc => uc.Value == passwordResetRequestDto.Value)
            .ToListAsync()).FirstOrDefault();
        if (userChannel == null)
            return await result;
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
        return await result;
    }

    public async Task<UserResponseDto> PasswordResetAction(string token, PasswordResetAction passwordResetAction)
    {
        var passwordReset = (await _passwordResetService.GetAllAsync())
            .FirstOrDefault(pr => pr?.Token == token && !pr.IsUsed, null);
        var isPasswordResetExpired =
            (DateTime.UtcNow - passwordReset?.CreatedAt)?.TotalSeconds > Constants.PasswordResetTimer;
        if (passwordReset == null || isPasswordResetExpired)
            throw new ArgumentException($"Password reset token is incorrect or already used: \"{token}\"");

        var userId = passwordReset.UserChannel?.User?.Id;
        if (userId == null)
            throw new Exception("Somehow User is null");
        //todo handling nulls
        var user = await _userService.UpdateAsync((Guid)userId, new UserUpdateDto
        {
            Password = passwordResetAction.Password
        });
        await _passwordResetService.UpdateAsync(passwordReset.Id, new PasswordResetUpdateDto
        {
            IsUsed = true
        });
        if (user == null)
            throw new Exception("Failed to changed user password");
        return new UserResponseDto
        {
            Username = user.Username,
            VerificationId = user.VerificationId,
            Channels = null
        };
    }


    private async Task<Verification?> VerifyUserChannel(UserChannel userChannel)
    {
        var verification = userChannel.Verification;
        if (verification != null)
        {
            if (IsVerificationValid(verification))
            {
                return await _verificationService.UpdateAsync(
                    verification.Id,
                    new VerificationUpdateDto
                    {
                        IsVerified = true
                    }
                );
            }

            throw new ArgumentException($"Verification with id: \"{verification.Id}\" is expired!");
        }

        throw new Exception($"UserChannel with id: \"{userChannel.Id}\" has no Verification.");
    }

    private async Task<UserChannel> HandleNewRegister(SignUpDto signUpDto)
    {
        var channel = (await _channelRepository.GetDbSet()
            .Where(ch => ch.AId == signUpDto.ChannelId)
            .ToListAsync()).FirstOrDefault();

        if (channel != null)
        {
            var userChannel = await _userChannelService.CreateAsync(new UserChannelCreateDto
            {
                Channel = channel,
                Value = signUpDto.Value
            });
            if (userChannel == null) throw new Exception("Failed to create UserChannel DB record");

            return await UpdateUserChannelRecord(userChannel, await CreateVerificationRecord());
        }

        var names = Enum.GetNames(typeof(Domain.Enums.Channel));
        throw new ArgumentException($"Channel must be one of these values: {Join(", ", names)}");
    }

    private async Task<UserChannel> HandleOldRegister(UserChannel oldUserChannel)
    {
        var verification = _verificationService.GetAllAsync().Result.LastOrDefault(v =>
                v?.Id == oldUserChannel.Verification?.Id, null
        );
        if (verification == null)
        {
            throw new Exception($"Failed to find Verification record for UserChannel with id: \"{oldUserChannel.Id}\"");
        }

        CheckVerificationInterval(verification);
        return await UpdateUserChannelRecord(
            oldUserChannel,
            await UpdateVerificationRecord(verification, Helpers.GenerateVerificationCode())
        );
    }

    private async Task<UserChannel> UpdateUserChannelRecord(UserChannel userChannel, Verification verification)
    {
        var updatedUserChannel = await _userChannelService.UpdateAsync(userChannel.Id, new UserChannelUpdateDto
        {
            Verification = verification
        });
        if (updatedUserChannel == null)
        {
            throw new Exception("Failed to update UserChannel DB record.");
        }

        return updatedUserChannel;
    }

    private async Task<Verification> CreateVerificationRecord()
    {
        return await _verificationService.CreateAsync(new VerificationCreateDto
        {
            Code = Helpers.GenerateVerificationCode()
        }) ?? throw new Exception("Failed to create Verification database record.");
    }

    private async Task<Verification> UpdateVerificationRecord(Verification verification, string code)
    {
        return await _verificationService.UpdateAsync(verification.Id, new VerificationUpdateDto
        {
            Code = code
        }) ?? throw new Exception("Failed to update Verification database record.");
    }

    private static void CheckVerificationInterval(Verification verification)
    {
        if (IsVerificationValid(verification))
            throw new Exception($"Verification with id: {verification.Id} is not expired yet!");
    }

    private static bool IsVerificationValid(Verification verification)
    {
        return (DateTime.UtcNow - verification.UpdatedAt).TotalSeconds <= Constants.SmsTimer;
    }

    private async Task SendVerificationCodeToChannel(UserChannel userChannel)
    {
        var name = userChannel.Channel.Name;
        if (name == Domain.Enums.Channel.Email.ToString())
        {
            throw new NotImplementedException();
        }

        if (name == Domain.Enums.Channel.Sms.ToString())
        {
            await HandleVerificationSmsChannel(userChannel);
        }

        if (name == Domain.Enums.Channel.Call.ToString())
        {
            throw new NotImplementedException();
        }
    }

    //todo refactor
    private async Task SendResetPasswordTokenToChannel(UserChannel userChannel, string token)
    {
        var name = userChannel.Channel.Name;
        if (name == Domain.Enums.Channel.Email.ToString())
        {
            throw new NotImplementedException();
        }

        if (name == Domain.Enums.Channel.Sms.ToString())
        {
            await SendSms("Reset-Password", token, userChannel.Value);
        }

        if (name == Domain.Enums.Channel.Call.ToString())
        {
            throw new NotImplementedException();
        }
    }

    private async Task HandleVerificationSmsChannel(UserChannel userChannel)
    {
        if (userChannel.Verification == null)
            throw new Exception($"Verification for UserChannel with id: \"{userChannel.Id}\" is null.");
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
        else throw new Exception("SMS SDK information is empty in appsettings.json!");
    }

    private static string CreateSmsUri(string api, string template, string receptor, string text)
    {
        return
            $"https://api.kavenegar.com/v1/{api}/verify/lookup.json?template={template}&receptor={receptor}&token={text}";
    }
}