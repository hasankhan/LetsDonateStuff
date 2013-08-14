using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace LetsDonateStuff.Services.Publishing.Targets.Freecycle
{
    public class FreecycleSettings
    {
        public IEnumerable<FreecycleGroup> Groups { get; set; }
        public MailAddress From { get; set; }
    }
}