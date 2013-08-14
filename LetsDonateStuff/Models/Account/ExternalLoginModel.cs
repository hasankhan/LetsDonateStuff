using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetsDonateStuff.Models.Account
{
    public class ExternalLoginModel
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
}