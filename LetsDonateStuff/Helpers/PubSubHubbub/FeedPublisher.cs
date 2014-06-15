using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;

namespace LetsDonateStuff.Helpers.PubSubHubbub
{
    public class FeedPublisher
    {
        FeedSignal signal;
        IPublisher publisher;

        public FeedPublisher(FeedSignal signal, IPublisher publisher)
        {
            this.signal = signal;
            this.publisher = publisher;
        }

        public void Start(TimeSpan timeout, CancellationTokenSource cancelSource)
        {
            while (!cancelSource.IsCancellationRequested)
            {
                string topicUrl = signal.Next(timeout);
                if (topicUrl != null)
                    try
                    {
                        publisher.Publish(topicUrl);
                    }
                    catch (Exception ex)
                    {
                        if (ex is AggregateException)
                            ((AggregateException)ex).Handle(exception =>
                            {
                                LogException(exception);
                                return true;
                            });
                        else
                            LogException(ex);
                    }
            }
        }

        static void LogException(Exception exception)
        {
            //var log = Elmah.ErrorLog.GetDefault(null);
            //log.Log(new Elmah.Error(exception));
        }
    }
}