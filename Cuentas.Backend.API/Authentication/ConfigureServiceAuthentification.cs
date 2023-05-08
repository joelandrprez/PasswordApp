﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;

namespace Cuentas.Backend.API.Authentication
{
    public static class ConfigureServiceAuthentification
    {
        public static void ConfigureJWT(this IServiceCollection services, bool IsDevelopment)
        {
            var AuthenticationBuilder = services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            });

            AuthenticationBuilder.AddJwtBearer(o =>
            {


                var configBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                var configuration = configBuilder.Build();
                var _key = configuration["Jwt:Key"];
                var _issuer = configuration["Jwt:Issuer"];
                var _audience = configuration["Jwt:Audience"];

                o.RequireHttpsMetadata = false;
                o.SaveToken = true;
                #region == JWT Token Validation ===
                try
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = false,

                        //comment this and add this line to fool the validation logic
                        SignatureValidator = delegate (string token, TokenValidationParameters parameters)
                        {

                            var jwt = new JwtSecurityToken(token);
                            return jwt;

                        },
                        RequireExpirationTime = true,
                        ValidateLifetime = true,//TODO: Habilitar la validez del token enviado desde el frontend
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = _issuer,
                        ValidAudience = _audience,
                        //IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_key))
                    };
                }
                catch (Exception ex)
                {

                    throw;
                }
                //Para ignorar la validación del firmante, que es microsoft


                #endregion

                #region === Event Authentification Handlers ===

                o.Events = new JwtBearerEvents()
                {
                    OnTokenValidated = c =>
                    {
                        Console.WriteLine("User successfully authenticated");
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();

                        c.Response.StatusCode = 500;
                        c.Response.ContentType = "text/plain";

                        if (IsDevelopment)
                        {
                            return c.Response.WriteAsync(c.Exception.ToString());
                        }
                        return c.Response.WriteAsync("An error occured processing your authentication.");
                    },
                };

                #endregion

            });
        }
    }
}
