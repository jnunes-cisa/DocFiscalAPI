using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace WebApi.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class AzureAdAuthenticationBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        public static AuthenticationBuilder AddAzureAdBearer(this AuthenticationBuilder builder)
        {

            return builder.AddAzureAdBearer(_ => { });

        }
        /// <summary>
        /// 
        /// </summary>
        public static AuthenticationBuilder AddAzureAdBearer(this AuthenticationBuilder builder, Action<AzureAdOptions> configureOptions)
        {

            builder.Services.Configure(configureOptions);
            builder.Services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureAzureOptions>();

            builder.AddJwtBearer();

            return builder;

        }
        /// <summary>
        /// 
        /// </summary>
        private class ConfigureAzureOptions : IConfigureNamedOptions<JwtBearerOptions>
        {

            private readonly AzureAdOptions _azureAdOptions;

            public ConfigureAzureOptions(IOptions<AzureAdOptions> azureOptions)
            {

                _azureAdOptions = azureOptions.Value;

            }

            public void Configure(string name, JwtBearerOptions options)
            {

                options.Audience = _azureAdOptions.Audience;
                options.Authority = $"{_azureAdOptions.Instance}{_azureAdOptions.TenantId}";

            }

            public void Configure(JwtBearerOptions options)
            {

                Configure(Options.DefaultName, options);

            }

        }

    }

}