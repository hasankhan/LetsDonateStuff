using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace LetsDonateStuff.Helpers.PubSubHubbub
{
    public class PubSubHubbubPublisher: IPublisher
    {
        IEnumerable<string> hubUrls;

        public PubSubHubbubPublisher(IEnumerable<string> hubUrls)
        {
            if (hubUrls == null || !hubUrls.Any())
                throw new ArgumentException("You must provide at least one hubUrl!", "hubUrls");
            
            this.hubUrls = hubUrls.ToList();
        }

        public void Publish(string topicUrl)
        {
            var exceptions = new ConcurrentQueue<Exception>();

            Parallel.ForEach(hubUrls, hubUrl =>
            {
                try
                {
                    Publish(hubUrl, topicUrl);
                }
                catch (Exception ex)
                {
                    exceptions.Enqueue(ex);
                }
            });

            if (exceptions.Any()) 
                throw new AggregateException(exceptions);
        }

        /// <summary>
        /// Publish a topic on a PubSubHubbub-hub, notifies the hub that there's an update.
        /// </summary>
        /// <param name="hubUrl">URL to the PubSubHubbub-hub</param>
        /// <param name="topicUrl">URL to the topic</param>
        static void Publish(string hubUrl, string topicUrl)
        {
            if (String.IsNullOrEmpty(topicUrl))
                throw new ArgumentException("Error publishing to PubSubHubbub-hub, topicUrl is not defined!", "topicURL");

            try
            {
                string postDataStr = "hub.mode=publish&hub.url=" + HttpUtility.UrlEncode(topicUrl);
                byte[] postData = Encoding.UTF8.GetBytes(postDataStr);

                var httpRequest = (HttpWebRequest)WebRequest.Create(hubUrl);
                httpRequest.Method = "POST";
                httpRequest.ContentType = "application/x-www-form-urlencoded";
                httpRequest.ContentLength = postData.Length;

                using (Stream requestStream = httpRequest.GetRequestStream())
                    requestStream.Write(postData, 0, postData.Length);

                var webResponse = (HttpWebResponse)httpRequest.GetResponse();
                if (httpRequest.HaveResponse)
                {
                    using (Stream responseStream = webResponse.GetResponseStream())
                    using (var responseReader = new StreamReader(responseStream, Encoding.UTF8))
                        responseReader.ReadToEnd();

                    if (webResponse.StatusCode != HttpStatusCode.NoContent)
                        throw new PublishException("Received unexpected statusCode from hub: '" + webResponse.StatusCode.ToString() + "' (should be 204 'No Content')") { HubUrl = hubUrl, TopicUrl = topicUrl };
                }
                else
                    throw new PublishException("Didn't receive any response from the hub") { HubUrl = hubUrl, TopicUrl = topicUrl };
            }
            catch (Exception ex)
            {
                throw new PublishException("Error publishing topicUrl '" + topicUrl + "' to pubSubHubbub-hub '" + hubUrl + "'", ex) { HubUrl = hubUrl, TopicUrl = topicUrl };
            }
        }
    }
}