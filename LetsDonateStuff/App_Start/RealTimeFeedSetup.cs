using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LetsDonateStuff.App_Start;
using System.Threading.Tasks;
using System.Threading;
using LetsDonateStuff.Helpers.PubSubHubbub;
using System.Web.Configuration;
using System.Configuration;
using LetsDonateStuff.Helpers;

[assembly: WebActivator.PostApplicationStartMethod(typeof(RealTimeFeedSetup), "Start")]
[assembly: WebActivator.ApplicationShutdownMethod(typeof(RealTimeFeedSetup), "Shutdown")]

namespace LetsDonateStuff.App_Start
{
    public static class RealTimeFeedSetup
    {
        static CancellationTokenSource cancelSource;

        public static void Start()
        {
            cancelSource = new CancellationTokenSource();
            Task.Factory.StartNew(PushLoop, cancelSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        public static void Shutdown()
        {
            cancelSource.Cancel();
        }

        static void PushLoop()
        {
            IEnumerable<string> hubUrls = MiscUtility.GetHubUrls();

            var publisher = new PubSubHubbubPublisher(hubUrls);
            var feedPublisher = new FeedPublisher(FeedSignal.Instance, publisher);

            var timeout = TimeSpan.Parse(ConfigurationManager.AppSettings["feedPushInterval"]);
            feedPublisher.Start(timeout, cancelSource);
        }
    }
}