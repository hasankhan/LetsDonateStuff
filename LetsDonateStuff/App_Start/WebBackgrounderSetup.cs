using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LetsDonateStuff.App_Start;
using WebBackgrounder;
using Elmah;
using System.Web.Configuration;
using LetsDonateStuff.Jobs;
using LetsDonateStuff.DAL;
using LetsDonateStuff.Services;
using System.Configuration;
using System.Security.Principal;
using LetsDonateStuff.Helpers.GeoIP;

[assembly: WebActivator.PostApplicationStartMethod(typeof(WebBackgrounderSetup), "Start")]
[assembly: WebActivator.ApplicationShutdownMethod(typeof(WebBackgrounderSetup), "Shutdown")]

namespace LetsDonateStuff.App_Start
{
    public static class WebBackgrounderSetup
    {
        static readonly JobManager jobManager = CreateJobWorkersManager();

        public static void Start()
        {
            jobManager.Start();
        }

        public static void Shutdown()
        {
            jobManager.Dispose();
        }

        static JobManager CreateJobWorkersManager()
        {
            var jobs = GetJobs().ToList();

            var coordinator = new SingleServerJobCoordinator();
            var manager = new JobManager(jobs, coordinator);
            manager.Fail(ex => Elmah.ErrorLog.GetDefault(null).Log((new Error(ex))));

            return manager;
        }

        static IEnumerable<Job> GetJobs()
        {
            var service = new DonationService(()=>new DonationRepository(), new GeoIPHelper(), UserToken.Admin);

            //yield return GetExtendExpiryJob(service);
            yield return GetCleanupJob(service);
        }

        static ExtendExpiryJob GetExtendExpiryJob(DonationService service)
        {
            var renewInterval = TimeSpan.Parse(ConfigurationManager.AppSettings["renewInterval"]);
            var renewBefore = TimeSpan.Parse(ConfigurationManager.AppSettings["renewBefore"]);

            var extendExpiryJob = new ExtendExpiryJob(renewInterval, renewBefore, service);
            return extendExpiryJob;
        }

        static CleanupJob GetCleanupJob(DonationService service)
        {
            var cleanupInterval = TimeSpan.Parse(ConfigurationManager.AppSettings["cleanupInterval"]);
            var cleanupOlderThan = TimeSpan.Parse(ConfigurationManager.AppSettings["cleanupOlderThan"]);

            var cleanupJob = new CleanupJob(cleanupInterval, cleanupOlderThan, service);
            return cleanupJob;
        }
    }
}