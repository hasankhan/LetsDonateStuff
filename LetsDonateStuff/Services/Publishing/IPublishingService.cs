using LetsDonateStuff.DAL;
using System;
namespace LetsDonateStuff.Services.Publishing
{
    public interface IPublishingService
    {
        bool Publish(PostedItem item);
    }
}
