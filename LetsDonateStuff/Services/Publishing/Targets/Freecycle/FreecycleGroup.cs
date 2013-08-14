using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LetsDonateStuff.Helpers;

namespace LetsDonateStuff.Services.Publishing.Targets.Freecycle
{
    public class FreecycleGroup
    {
        public string Country { get; private set; }
        public string Locality { get; private set; }
        public string Email { get; private set; }

        public FreecycleGroup(string country, string email, string locality)
        {
            this.Country = country.ToUpperInvariant();
            this.Locality = Normalize(locality);
            this.Email = email;
        }        

        public bool IsMatch(string country, string locality)
        {
            locality = Normalize(locality);

            bool isMatch = Country == country.ToUpperInvariant() &&
                           (locality == null || // item doesn't have locality so it will be posted to all groups
                            Locality == null || // group is catch all for all localities e.g. Amsterdam-NL Freecycle
                            locality.Contains(Locality)); // item contains locality string for group

            return isMatch;
        }

        string Normalize(string locality)
        {
            return locality.NullIfEmpty().Coalesce(l => l.ToUpperInvariant());
        }
    }
}