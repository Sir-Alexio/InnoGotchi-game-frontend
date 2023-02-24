using InnoGotchi_backend.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using NuGet.Configuration;
using InnoGotchi_frontend.Controllers;
using FluentValidation;
using InnoGotchi_backend.Models;
using InnoGotchi_frontend.Models;
using InnoGotchi_frontend.Services;
using FluentValidation.AspNetCore;
using InnoGotchi_frontend.Models.Validators;
using InnoGotchi_frontend.Models.Services;
using InnoGotchi_backend.Models.Dto;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAuthorization();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllers().AddFluentValidation();

builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddScoped<IValidator<UserDto>, UserValidator>();
builder.Services.AddScoped<IValidator<ChangePasswordModel>, ChangePasswordValidator>();


builder.Services.AddScoped<ITokenManager, TokenManager>();
builder.Services.AddScoped<IValidationService, UserDtoValidationService>();
builder.Services.AddScoped<IPasswordValidationService, PasswordValidationService>();

builder.Services.AddControllers();
builder.Services.AddHttpClient("Client", c => c.BaseAddress = new System.Uri("https://localhost:7198/"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

