using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mvc.Mailer;
using System.Net.Mail;
using LetsDonateStuff.DAL;
using LetsDonateStuff.Models;
using LetsDonateStuff.Configuration;

namespace LetsDonateStuff.Mailers
{
    public class UserMailer : MailerBase
    {
        string adminEmail;

        public UserMailer(AppSettings appSettings)
        {
            this.adminEmail = appSettings.AdminEmail;

            MasterName = "_Layout";
        }

        public virtual MvcMailMessage ContactPoster(Response request, PostedItem item)
        {
            var mailMessage = new MvcMailMessage();
            mailMessage.Subject = String.Format("{0} has contacted you for {1} on LetsDonateStuff.com", request.Name, item.Title);

            mailMessage.To.Add(item.Email);
            mailMessage.ReplyToList.Add(request.Email);

            ViewBag.Item = item;
            ViewData.Model = request;
            PopulateBody(mailMessage, viewName: "ContactPoster");

            return mailMessage;
        }


        public virtual MvcMailMessage Confirmation(PostedItem item)
        {
            var mailMessage = new MvcMailMessage();
            mailMessage.Subject = String.Format("Post Confirmation for {0} on LetsDonateStuff.com", item.Title);

            mailMessage.To.Add(item.Email);

            ViewData.Model = item;
            PopulateBody(mailMessage, viewName: "Confirmation");

            return mailMessage;
        }

        public virtual MvcMailMessage Approval(PostedItem item)
        {
            var mailMessage = new MvcMailMessage();
            mailMessage.Subject = String.Format("Post Approval for {0} by {1}", item.Title, item.Name);

            mailMessage.To.Add(adminEmail);
            ViewData.Model = item;

            PopulateBody(mailMessage, viewName: "Approval");

            return mailMessage;
        }
    }
}