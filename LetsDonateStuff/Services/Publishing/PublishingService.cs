using LetsDonateStuff.DAL;
using LetsDonateStuff.Services.Publishing.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetsDonateStuff.Services.Publishing
{
    public class PublishingService : IPublishingService
    {
        IEnumerable<IPublishingTarget> targets;

        public PublishingService(IEnumerable<IPublishingTarget> targets)
        {
            this.targets = targets;
        }

        public bool Publish(PostedItem item)
        {
            var publishers = targets.Where(target => target.Accepts(item)).ToList();
            
            foreach (IPublishingTarget target in publishers)
                target.Publish(item);

            return publishers.Any();
        }
    }
}