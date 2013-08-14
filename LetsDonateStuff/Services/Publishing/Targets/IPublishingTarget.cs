using LetsDonateStuff.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetsDonateStuff.Services.Publishing.Targets
{
    public interface IPublishingTarget
    {        
        bool Accepts(PostedItem item);
        void Publish(PostedItem item);
    }
}