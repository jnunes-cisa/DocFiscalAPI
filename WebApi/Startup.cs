using Common.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Repositories;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WebApi.Context;
using WebApi.Extensions;
using WebApi.Filters;
using static Common.Enums.Enums;

namespace WebApi
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {

        private bool UseAzureAD = false;
        private bool UseSwagger = false;
        private bool UseInMemoryDataBase = false;
        private bool UseGenerateData = false;
        private bool UseAuthorize = false;
        private EnumAmbiente _ambiente = EnumAmbiente.Desenvolvimento;
        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }
        /// <summary>
        /// 
        /// </summary>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            UseAzureAD = Configuration.GetValue<bool>("UseAzureAD");
            UseSwagger = Configuration.GetValue<bool>("UseSwagger");
            UseInMemoryDataBase = Configuration.GetValue<bool>("UseInMemoryDataBase");
            UseGenerateData = Configuration.GetValue<bool>("UseGenerateData");
            UseAuthorize = Configuration.GetValue<bool>("UseAuthorize");
            _ambiente = Configuration.GetValue<EnumAmbiente>("Ambiente");
        }

        /// <summary>
        /// 
        /// </summary>        
        public void ConfigureServices(IServiceCollection services)
        {

            if (UseAzureAD)
            {
                services
                    .AddAuthentication(sharedOptions => sharedOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme)
                    .AddAzureAdBearer(options => Configuration.Bind("AzureAD", options));
            }

            services
                .AddCors()
                .AddMvc(options =>
                {

                    if (UseAuthorize)
                    {
                        options.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
                    }

                    options.Filters.Add(typeof(ApiExceptionFilter));                    
                });
            services.AddControllers();   

            if (UseSwagger)
            {
                services.AddSwaggerGen(c =>
                {
                    var lastPublish = System.IO.File.GetLastWriteTime(GetType().Assembly.Location);

                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = $"DocFiscal - API - {_ambiente.Description()}",
                        Version = lastPublish.ToString("dd/MM/yyyy HH:mm:ss"),
                    });

                    c.IncludeXmlComments($"{System.AppDomain.CurrentDomain.BaseDirectory}WebApi.xml");
                    c.IncludeXmlComments($"{System.AppDomain.CurrentDomain.BaseDirectory}Entities.xml");                    
                   
                    if (UseAzureAD)
                    {
                        //Define the OAuth2.0 scheme that's in use (i.e. Implicit Flow)
                        c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                        {
                            Type = SecuritySchemeType.OAuth2,
                            Flows = new OpenApiOAuthFlows
                            {
                                Implicit = new OpenApiOAuthFlow
                                {
                                    AuthorizationUrl = new Uri($"https://login.microsoftonline.com/{"be013de1-c776-4eb9-a825-61d5d8b9aca2"}/oauth2/authorize", UriKind.Absolute),
                                    Scopes = new Dictionary<string, string>
                                    {
                                        { "user_impersonation", "Access PedidoWebApi" }
                                    }
                                }
                            }
                        });
                    }
                });

            }

            if (UseInMemoryDataBase)
                // services.AddDbContext<BaseContext>(options => options.UseInMemoryDatabase("DataBaseInMemory"));
                services.AddDbContext<BaseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ConnectionString")), ServiceLifetime.Scoped);
            else
                services.AddDbContext<BaseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ConnectionString")), ServiceLifetime.Scoped);

            //services.AddTransient<InitialDataServices>();

            services.AddScoped<IJsonApiContext, JsonApiContext>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton(Configuration);

        }
        
        /// <summary>
        /// 
        /// </summary>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
            if (UseSwagger)
            {

                app
                    .UseSwagger()
                    .UseSwaggerUI(c =>
                    {
                        c.DocumentTitle = $"DocFiscal API {_ambiente.Description()}";
                        c.SwaggerEndpoint("/swagger/v1/swagger.json", $"DocFiscal - Web API - {_ambiente.Description()} - V1");
                        c.InjectStylesheet($"/swagger-ui/{_ambiente}.css".ToLower());
                        if (UseAzureAD)
                        {
                            var swOption = new AzureAdOptionsSwagger();

                            Configuration.Bind("SwaggerAD", swOption);

                            var stringDict = new Dictionary<string, string>();

                            stringDict.Add("resource", swOption.Audience);

                            c.OAuthClientId(swOption.ClientId);
                            c.OAuthClientSecret(swOption.ClientSecret);
                            c.OAuthRealm(swOption.ReplyURL);
                            c.OAuthAppName(swOption.AppName);
                            c.OAuthScopeSeparator(" ");
                            c.OAuthAdditionalQueryStringParams(stringDict);
                        }
                    });

            }

            //if (UseGenerateData)
            //    initialDataServices.GenerateData();

        }

    }  

}