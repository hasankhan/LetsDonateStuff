using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetsDonateStuff.Helpers.PubSubHubbub
{
    public class PublishException : Exception
    {
        public PublishException(string message) : base(message) { }
        public PublishException(string message, Exception innerException) : base(message, innerException) { }

        public string HubUrl { get; set; }
        public string TopicUrl { get; set; }
    }
}