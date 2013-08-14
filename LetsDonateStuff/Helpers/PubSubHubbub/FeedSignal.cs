using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Collections.Concurrent;

namespace LetsDonateStuff.Helpers.PubSubHubbub
{
    public class FeedSignal
    {
        BlockingCollection<string> queue;

        public FeedSignal()
        {
            queue = new BlockingCollection<string>();
        }

        public static FeedSignal Instance = new FeedSignal();

        public string Next(TimeSpan timeout)
        {
            string topicUrl;
            queue.TryTake(out topicUrl, timeout);
            return topicUrl;
        }

        public void Signal(string topicUrl)
        {
            queue.Add(topicUrl);
        }
    }
}