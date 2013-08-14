using Mvc.Mailer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace LetsDonateStuff.Mailers
{
    class YahooMailMessage : MvcMailMessage
    {
        NetworkCredential credential;

        public YahooMailMessage(MailAddress email, string password)
        {
            From = email;
            this.credential = new NetworkCredential(email.Address, password);
        }

        public override ISmtpClient GetSmtpClient()
        {
            var client = base.GetSmtpClient() as SmtpClientWrapper;
            if (client != null)
            {
                client.InnerSmtpClient.Host = "smtp.mail.yahoo.com";
                client.InnerSmtpClient.Port = 587;
                client.InnerSmtpClient.EnableSsl = false;
                client.InnerSmtpClient.Credentials = credential;
            }
            return client;
        }
    }
}