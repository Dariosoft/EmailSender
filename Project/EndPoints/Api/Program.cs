using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using System.Threading.Tasks;

namespace Dariosoft.EmailSender.EndPoint.Api
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services
                .AddEndPointLayer(builder.Configuration)
                .AddHttpContextAccessor()
                .AddControllers()
                .Services
                .AddAuthentication(defaultScheme: "Dariosoft")
                .AddScheme<EndPoint.Auth.AuthOptions, EndPoint.Auth.AuthenticationHandler>("Dariosoft", options => { })
                .Services
                .AddSingleton<IAuthorizationPolicyProvider, EndPoint.Auth.AuthorizationPolicyProvider>()
                .AddSingleton<IAuthorizationHandler, EndPoint.Auth.AuthorizationHandler>()
                .AddAuthorization();

            if (builder.Environment.IsDevelopment())
            {
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen(options =>
                {
                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "Dariosoft Authorization header using the Bearer scheme",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey
                    });

                    var requirement = new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            []
                        }
                    };

                    options.AddSecurityRequirement(requirement);
                });
            }

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseAuthorization();

            app.MapControllers();

            app.Lifetime.RegisterLifetimeDelegates(app.Services);

            app.Run();
        }
    }
}
