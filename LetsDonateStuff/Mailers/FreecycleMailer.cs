using LetsDonateStuff.DAL;
using Mvc.Mailer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace LetsDonateStuff.Mailers
{
    public class FreecycleMailer: MailerBase
    {
        readonly MailAddress fromEmail;

        public FreecycleMailer(MailAddress fromEmail)
        {
            this.fromEmail = fromEmail;

            MasterName = "_Layout";
        }

        public virtual MvcMailMessage Wanted(DonationRequest item, string groupEmail)
        {
            return CreateMail(item, groupEmail, "Wanted");
        }        

        public virtual MvcMailMessage Offer(Donation item, string groupEmail)
        {
            return CreateMail(item, groupEmail, "Offer");            
        }

        MvcMailMessage CreateMail(PostedItem item, string groupEmail, string operation)
        {
            ViewData.Model = item;

            var mailMessage = new MvcMailMessage();

            mailMessage.From = fromEmail;
            mailMessage.To.Add(groupEmail);
            mailMessage.Subject = String.Format("{0}: {1}", operation, item.Title);

            PopulateBody(mailMessage, operation);

            return mailMessage;
        }
    }
}