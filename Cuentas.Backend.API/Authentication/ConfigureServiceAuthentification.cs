using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
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
                o.RequireHttpsMetadata = false;
                o.SaveToken = true;
                #region == JWT Token Validation ===

                //Para ignorar la validación del firmante, que es microsoft
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = false,
                    
                    //comment this and add this line to fool the validation logic
                    SignatureValidator = delegate (string token, TokenValidationParameters parameters)
                    {
                        
                        var jwt = new JwtSecurityToken(token);
                        return jwt;

                    },
                    RequireExpirationTime = true,
                    ValidateLifetime = false,//TODO: Habilitar la validez del token enviado desde el frontend
                    ClockSkew = TimeSpan.Zero,
                };

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
