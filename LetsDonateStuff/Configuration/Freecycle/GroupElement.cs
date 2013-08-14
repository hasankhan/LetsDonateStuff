using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace LetsDonateStuff.Configuration.Freecycle
{
    public class GroupElement: ConfigurationElement
    {
        [ConfigurationProperty("country")]
        public string Country
        {
            get { return (string)this["country"]; }
        }

        [ConfigurationProperty("locality")]
        public string Locality
        {
            get { return (string)this["locality"]; }
        }

        [ConfigurationProperty("email")]
        public string Email
        {
            get { return (string)this["email"]; }
        }
    }
}
