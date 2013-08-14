using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LetsDonateStuff.Helpers.PubSubHubbub
{
    public interface IPublisher
    {
        void Publish(string topicUrl);
    }
}
