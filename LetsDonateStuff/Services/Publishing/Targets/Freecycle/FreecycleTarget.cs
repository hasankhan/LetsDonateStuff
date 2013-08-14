using LetsDonateStuff.DAL;
using LetsDonateStuff.Mailers;
using Mvc.Mailer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Web;

namespace LetsDonateStuff.Services.Publishing.Targets.Freecycle
{
    public class FreecycleTarget: IPublishingTarget
    {
        IDictionary<string, IEnumerable<FreecycleGroup>> groups;
        FreecycleMailer mailer;

        public FreecycleTarget(IEnumerable<FreecycleGroup> groups, FreecycleMailer mailer)
        {
            this.groups = groups.GroupBy(g=>g.Country)
                                .ToDictionary(g=>g.Key, g=>g.ToList().AsEnumerable());
            this.mailer = mailer;
        }

        public bool Accepts(PostedItem item)
        {
            bool accepts = groups.ContainsKey(item.Country) && 
                           groups[item.Country].Any(g=>g.IsMatch(item.Country, item.Locality));
            return accepts;
        }

        public void Publish(PostedItem item)
        {
            string groupEmail = groups[item.Country].FirstOrDefault(g=>g.IsMatch(item.Country, item.Locality)).Email;
            MvcMailMessage mail = null;

            if (item is Donation)
                mail = mailer.Offer((Donation)item, groupEmail);
            else if (item is DonationRequest)
                mail = mailer.Wanted((DonationRequest)item, groupEmail);

            if (mail != null)
                mail.SendAsync();
        }        
    }
}