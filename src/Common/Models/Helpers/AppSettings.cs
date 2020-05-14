using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Models.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
    }
}
