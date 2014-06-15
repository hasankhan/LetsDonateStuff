﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LetsDonateStuff.Services;

namespace LetsDonateStuff.Jobs
{
    public class ExtendExpiryJob : DelayedJob
    {
        DonationService donationService;
        TimeSpan renewBefore;

        public ExtendExpiryJob(TimeSpan interval, TimeSpan renewBefore, DonationService donationService)
            : base("ExtendExpiryJob", interval)
        {
            this.donationService = donationService;
            this.renewBefore = renewBefore;
        }

        protected override void OnExecute()
        {
            donationService.RenewAll(renewBefore, TimeSpan.FromDays(7));
        }
    }
}