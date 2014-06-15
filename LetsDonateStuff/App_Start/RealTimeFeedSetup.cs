using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using LetsDonateStuff;
using LetsDonateStuff.Helpers;
using LetsDonateStuff.Helpers.PubSubHubbub;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(RealTimeFeedSetup), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(RealTimeFeedSetup), "Shutdown")]

namespace LetsDonateStuff
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