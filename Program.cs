
using myTasks.Services;
using myTasks.Middlewares;
using myTasks.Utilities;
using myTasks.Interfaces;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// builder.Services.AddMyTask();
// builder.Services.AddUser();
// builder.Services.AddToken();
builder.Services.AddServices();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    //var tokenService = builder.Services.BuildServiceProvider().GetRequiredService<ITokenService>;
    ITokenService tokenService=new TokenService();
    cfg.TokenValidationParameters =tokenService.GetTokenValidationParameters();
});
builder.Services.AddAuthorization(cfg =>
            {
                cfg.AddPolicy("ADMIN", policy => policy.RequireClaim("type", "ADMIN"));
                cfg.AddPolicy("USER", policy => policy.RequireClaim("type", "USER"));
            });

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FBI", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                { new OpenApiSecurityScheme
                        {
                         Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer"}
                        },
                    new string[] {}
                }
                });
            });



var app = builder.Build();

app.Map("/favicon.ico", (a) =>
    a.Run(async c => await Task.CompletedTask));

app.UseLogMiddleware();

app.Use(async (context, next) => 
{   
    await next.Invoke();
});



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
// app.useAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
