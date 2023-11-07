using System.Reflection;
using Infrastructure.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.OpenApi.Models;
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
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Infrastructure.xml"));
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "UMS.Authentication.Application.xml"));
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "UMS.Authentication.Domain.xml"));
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
            "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

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