using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LetsDonateStuff.DAL;
using System.ServiceModel.Syndication;
using System.Xml;

namespace LetsDonateStuff.Helpers.ActionResults
{
    public class RssResult<T> : FileResult
    {
        public IEnumerable<T> Items { get; private set; }
        public string Title { get; set; }

        Uri currentUrl;
        Func<T, SyndicationItem> feedItemConverter;

        public RssResult() : base("text/xml") { }

        public RssResult(IEnumerable<T> items, string title, Func<T, SyndicationItem> feedItemConverter)
            : this()
        {
            this.Items = items;
            this.Title = title;
            this.feedItemConverter = feedItemConverter;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            currentUrl = context.RequestContext.HttpContext.Request.Url;
            base.ExecuteResult(context);
        }

        protected override void WriteFile(System.Web.HttpResponseBase response)
        {
            var items = new List<SyndicationItem>();

            foreach (T item in this.Items)
            {
                var result = feedItemConverter(item);
                items.Add(result);
            }

            SyndicationFeed feed = CreateFeed(items);

            Rss20FeedFormatter formatter = new Rss20FeedFormatter(feed);

            using (XmlWriter writer = XmlWriter.Create(response.Output))
                formatter.WriteTo(writer);
        }

        protected virtual SyndicationFeed CreateFeed(List<SyndicationItem> items)
        {
            var feed = new SyndicationFeed(
                this.Title,
                this.Title, /* Using Title also as Description */
                currentUrl,
                items);
            return feed;
        }
    }
}