using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LetsDonateStuff.DAL;
using System.Web.Mvc;
using System.ServiceModel.Syndication;

namespace LetsDonateStuff.Helpers.ActionResults
{
    public class DonationFeed: RssResult<PostedItem>
    {
        IEnumerable<string> hubs;

        public DonationFeed(IEnumerable<PostedItem> items, IEnumerable<string> hubs, UrlHelper urlHelper) : base(items, "Latest Donations", new FeedItemConverter(urlHelper).Convert)
        {
            this.hubs = hubs;
        }

        protected override SyndicationFeed CreateFeed(List<SyndicationItem> items)
        {
            SyndicationFeed feed = base.CreateFeed(items);

            foreach (string hub in hubs)
                feed.Links.Add(new SyndicationLink(new Uri(hub, UriKind.RelativeOrAbsolute), "hub", null, null, 0));

            return feed;
        }

        class FeedItemConverter
        {
            UrlHelper urlHelper;

            public FeedItemConverter (UrlHelper urlHelper)
	        {
                this.urlHelper = urlHelper;
	        }

            public SyndicationItem Convert(PostedItem postedItem)
            {
                string contentString = String.Format("{0} by {1} on {2:MMM dd, yyyy} at {3}. Where: {4}",
                                        postedItem.Description,
                                        postedItem.Name,
                                        postedItem.PostedOn,
                                        postedItem.PostedOn.ToShortTimeString(),
                                        postedItem.Address);

                string detailLink = urlHelper.SiteUrl() + urlHelper.Action("Details", new { id = postedItem.Id, slug= postedItem.Slug });
                string prefix = postedItem is Donation ? "Offer": "Need";
                string title = String.Format("{0} - {1}", prefix, postedItem.Title.ToTitleCase());
                
                var item = new SyndicationItem(
                    title: title,
                    content: contentString,
                    itemAlternateLink: new Uri(detailLink),
                    id: detailLink,
                    lastUpdatedTime: postedItem.PostedOn.ToUniversalTime()
                    );
                item.PublishDate = postedItem.PostedOn.ToUniversalTime();
                item.Summary = new TextSyndicationContent(contentString, TextSyndicationContentKind.Plaintext);

                return item;
            }
        }
    }
}