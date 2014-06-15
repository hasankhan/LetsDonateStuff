﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBackgrounder;
using System.Threading.Tasks;
using LetsDonateStuff.Services;

namespace LetsDonateStuff.Jobs
{
    public class CleanupJob : DelayedJob
    {
        DonationService donationService;
        TimeSpan olderThan;

        public CleanupJob(TimeSpan interval, TimeSpan olderThan, DonationService donationService)
            : base("CleanupJob", interval)
        {
            this.donationService = donationService;
            this.olderThan = olderThan;
        }

        protected override void OnExecute()
        {
            var beforeDate = DateTime.UtcNow.Subtract(olderThan);
            donationService.PurgeAll(beforeDate);
        }
    }
}