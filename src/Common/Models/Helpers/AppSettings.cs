using System;

namespace Models.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public TimeSpan TokenLifetime { get; set; } 
    }
}
