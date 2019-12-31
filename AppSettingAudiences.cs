using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core3.xWebApi
{
    public class AppSettingAudiences
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
