using Infrastructure.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using UMS.Authentication.Application.Authorization;
using UMS.Authentication.Application.Dtos;
using UMS.Authentication.Application.Interfaces;
using UMS.Authentication.Application.Services;
using UMS.Authentication.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>();

//User
builder.Services.AddScoped<IBaseRepository<User>, BaseRepository<User>>();
builder.Services.AddScoped<IBaseService<User, UserCreateDto, UserUpdateDto>, UserService>();

//UserChannel
builder.Services.AddScoped<IBaseRepository<UserChannel>, BaseRepository<UserChannel>>();
builder.Services
    .AddScoped<IBaseService<UserChannel, UserChannelCreateDto, UserChannelUpdateDto>, UserChannelService>();

//Verification
builder.Services.AddScoped<IBaseRepository<Verification>, BaseRepository<Verification>>();
builder.Services
    .AddScoped<IBaseService<Verification, VerificationCreateDto, VerificationUpdateDto>, VerificationService>();

//PasswordReset
builder.Services.AddScoped<IBaseRepository<PasswordReset>, BaseRepository<PasswordReset>>();
builder.Services
    .AddScoped<IBaseService<PasswordReset, PasswordResetCreateDto, PasswordResetUpdateDto>, PasswordResetService>();

//Authentication
builder.Services.AddScoped<IBaseRepository<Channel>, BaseRepository<Channel>>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddHttpClient<IAuthService, AuthService>();

//JWT
builder.Services.AddScoped<IJwtUtils, JwtUtils>();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("JWT"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var app = builder.Build();

// global cors policy
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

// custom jwt auth middleware
app.UseMiddleware<JwtMiddleware>();

// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI();
// }

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();