using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using LetsDonateStuff.DAL;
using LetsDonateStuff.Helpers.GeoIP;
using LetsDonateStuff.Jobs;
using LetsDonateStuff.Services;
using WebBackgrounder;

namespace LetsDonateStuff
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
            //manager.Fail(ex => Elmah.ErrorLog.GetDefault(null).Log((new Error(ex))));

            return manager;
        }

        static IEnumerable<Job> GetJobs()
        {
            var service = new DonationService(() => new DonationRepository(), new GeoIPHelper(), UserToken.Admin);

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