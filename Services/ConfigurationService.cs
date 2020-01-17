using Microsoft.Extensions.Configuration;

namespace Services
{
    public class ConfigurationService
    {
        private static IConfiguration _configuration;

        public static IConfiguration Configuration
        {
            get { return _configuration;  }
            set { _configuration = value; }
        }
    }
}
