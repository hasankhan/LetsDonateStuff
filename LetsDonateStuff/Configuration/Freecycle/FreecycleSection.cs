using LetsDonateStuff.Services.Publishing.Targets.Freecycle;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace LetsDonateStuff.Configuration.Freecycle
{
    public class FreecycleSection: ConfigurationSection
    {
        public static FreecycleSection GetConfig()
        {
            return (FreecycleSection)ConfigurationManager.GetSection("freecycle") ?? new FreecycleSection();
        }

        public static FreecycleSettings GetSettings()
        {
            var section = GetConfig();

            var settings = new FreecycleSettings();
            settings.Groups = section.Groups.OfType<GroupElement>()
                                            .Select(g =>
                                                new FreecycleGroup(g.Country, g.Email, g.Locality )
                                             ).ToList();
            settings.From = new MailAddress(section.From.Email, section.From.Name);

            return settings;
        }

        [ConfigurationProperty("from")]
        public MailAddressElement From
        {
            get { return (MailAddressElement)this["from"] ?? new MailAddressElement(); }
        }

        [ConfigurationProperty("groups")]
        public GroupElementCollection Groups
        {
            get { return (GroupElementCollection)this["groups"] ?? new GroupElementCollection(); }
        }
    }
}