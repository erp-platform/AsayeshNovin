using UMS.Authentication.Application.Dtos;
using UMS.Authentication.Application.Interfaces;
using UMS.Authentication.Application.Services;
using UMS.Authentication.Domain.Entities;
using UMS.Authentication.Infrastructure.Interfaces;
using UMS.Authentication.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//User
builder.Services.AddScoped<ICrudRepository<User>, CrudRepository<User>>();
builder.Services.AddScoped<ICrudService<User, UserCreateDto, UserUpdateDto>, UserService>();

//User Channel
builder.Services.AddScoped<ICrudRepository<UserChannel>, CrudRepository<UserChannel>>();
builder.Services
    .AddScoped<ICrudService<UserChannel, UserChannelCreateDto, UserChannelUpdateDto>, UserChannelService>();

//Verification
builder.Services.AddScoped<ICrudRepository<Verification>, CrudRepository<Verification>>();
builder.Services
    .AddScoped<ICrudService<Verification, VerificationCreateDto, VerificationUpdateDto>, VerificationService>();

//Authentication
builder.Services.AddScoped<IRepository<Channel>, Repository<Channel>>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddHttpClient<IAuthService, AuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
