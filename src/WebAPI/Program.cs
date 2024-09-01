using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;

namespace WebAPI
{
    public class Program
    {
        static bool ValidateCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        public static void Main(string[] args)
        {
            IdentityModelEventSource.LogCompleteSecurityArtifact = true;
            IdentityModelEventSource.ShowPII = true;



            var builder = WebApplication.CreateBuilder(args);
 
            builder.Services.AddControllers();

            builder.Services.AddOptions<KeycloakConfig>()
                .BindConfiguration("Keycloak")
                .ValidateDataAnnotations()
                .ValidateOnStart();

  
            var keycloakOptions = builder.Services.BuildServiceProvider()
                .GetRequiredService<IOptions<KeycloakConfig>>();
         

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                var openIdConnectUrl = $"{keycloakOptions.Value.RealmUrl}/.well-known/openid-configuration";
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Auth",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.OpenIdConnect,
                    OpenIdConnectUrl = new Uri(openIdConnectUrl),
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    },

                };
                options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);

                // this allows to pass a token during swagger request
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, Array.Empty<string>()}
                });

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "WEB API",
                    Version = "v1",
                    Description = ""
                });
            });


            builder.Services.AddCors(options => options.AddDefaultPolicy(b =>
            {
                b.WithOrigins("http://keycloak:9096/")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                b.WithOrigins("http://localhost:9096/")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            }));

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opts =>
                {

                    var validationParameters = new TokenValidationParameters
                    {
                        ClockSkew = TimeSpan.FromMinutes(5),
                        ValidateAudience =  true,
                        ValidateIssuer = true,
                        NameClaimType = "preferred_username",
                        RoleClaimType = "role",
                    };

                    opts.Authority = keycloakOptions.Value.RealmUrl;
                    opts.Audience = keycloakOptions.Value.Audience;
                    opts.TokenValidationParameters = validationParameters;
                    opts.RequireHttpsMetadata = false;
                    opts.SaveToken = true;
                });



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
