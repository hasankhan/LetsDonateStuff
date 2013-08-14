using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace LetsDonateStuff.Configuration.Freecycle
{
    public class MailAddressElement: ConfigurationElement
    {
        [ConfigurationProperty("name")]
        public string Name
        {
            get { return (string)this["name"]; }
        }

        [ConfigurationProperty("email")]
        public string Email
        {
            get { return (string)this["email"]; }
        }
    }
}
