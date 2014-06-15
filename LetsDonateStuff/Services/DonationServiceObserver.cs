using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LetsDonateStuff.DAL;
using LetsDonateStuff.Helpers;
using LetsDonateStuff.Mailers;
using Mvc.Mailer;

namespace LetsDonateStuff.Services
{
    public class DonationServiceObserver
    {
        Imgur imgur;
        UserMailer mailer;

        public DonationService DonationService { get; private set; }

        public DonationServiceObserver(Imgur imgur, DonationService donationService, UserMailer mailer)
        {
            this.imgur = imgur;
            this.DonationService = donationService;
            this.mailer = mailer;

            donationService.PostAdded += donationService_PostAdded;
            donationService.PostPurged += donationService_PostPurged;
            donationService.ResendConfirmationEmail += donationService_ResendConfirmationEmail;
            donationService.ResponseAdded += donationService_ResponseAdded;
            donationService.PostApproved += donationService_PostApproved;
        }        

        void donationService_ResponseAdded(object sender, ResponseAddedEventArgs e)
        {
            ExceptionMonster.EatException(() => mailer.ContactPoster(e.Request, e.Post).SendAsync());
        }

        void donationService_ResendConfirmationEmail(object sender, ItemEventArgs e)
        {
            SendConfirmation(e.Item);
        }

        void donationService_PostPurged(object sender, ItemEventArgs e)
        {
            var donation = e.Item as Donation;
            if (donation != null && donation.ImageDelHash != null)
                ExceptionMonster.EatException(() => imgur.Delete(donation.ImageDelHash));
        }

        void donationService_PostAdded(object sender, ItemEventArgs e)
        {
            SendConfirmation(e.Item);
            ExceptionMonster.EatException(() => mailer.Approval(e.Item).SendAsync());
        }

        void donationService_PostApproved(object sender, ItemEventArgs e)
        {
            
        }

        void SendConfirmation(PostedItem item)
        {
            ExceptionMonster.EatException(() => mailer.Confirmation(item).SendAsync());
        }
    }
}