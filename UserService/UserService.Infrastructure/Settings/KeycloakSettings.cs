using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Infrastructure.Settings
{
    public class KeycloakSettings
    {
        public string Realm { get; set; }

        public string AuthServerUrl { get; set; }

        public string SslRequired { get; set; }

        public string Resource { get; set; }

        public bool PublicClient { get; set; }

        public int ConfidentialPort { get; set; }

        public bool VerifyTokenAudience { get; set; }

        public string ClientSecret { get; set; }

        public string GrantType { get; set; }
    }
}
