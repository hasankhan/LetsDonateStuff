using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LetsDonateStuff.DAL;
using LetsDonateStuff.Helpers.GeoIP;
using System.Security.Principal;
using LetsDonateStuff.Helpers;

namespace LetsDonateStuff.Services
{
    public class ItemEventArgs : EventArgs
    {
        public PostedItem Item { get; private set; }

        public ItemEventArgs(PostedItem item)
        {
            this.Item = item;
        }
    }

    public class ResponseAddedEventArgs : EventArgs
    {
        public PostedItem Post { get; private set; }
        public Response Request { get; private set; }

        public ResponseAddedEventArgs (PostedItem item, Response request)
	    {
                this.Post = item;
                this.Request = request;
	    }
    }

    public class DonationService: Service<DonationRepository>
    {
        GeoIPHelper geoIPHelper;
        UserToken token;

        public event EventHandler<ItemEventArgs> PostAdded = delegate { };
        public event EventHandler<ItemEventArgs> PostPurged = delegate { };
        public event EventHandler<ItemEventArgs> PostApproved = delegate { };
        public event EventHandler<ItemEventArgs> ResendConfirmationEmail = delegate { };
        public event EventHandler<ResponseAddedEventArgs> ResponseAdded = delegate { };

        public DonationService(Func<DonationRepository> repositoryFactory, GeoIPHelper geoIPHelper, UserToken user): base(repositoryFactory)
        {
            this.geoIPHelper = geoIPHelper;
            this.token = user;
        }

        public int GetPostsCount(string country, PostType type, bool showAll = true)
        {
            return GetItem(repository => repository.GetPostsCount(String.Empty, country, type, showAll));
        }

        public PostSearchResult Search(string query, string country, int pageIndex, int pageSize, PostType type, bool showAll = true)
        {
            return GetItem(repository =>
            {
                showAll = showAll && token.Principal.IsModerator(); // only mods can do show all
                IEnumerable<PostedItem> donations = repository.GetPosts(query, country, pageIndex, pageSize, type, showAll);
                int total = repository.GetPostsCount(query, country, type, showAll);

                var result = new PostSearchResult()
                {
                    Posts = donations,
                    Count = total
                };

                return result;
            });
        }

        public void Add(PostedItem post)
        {
            if (String.IsNullOrEmpty(post.Country))
                post.Country = geoIPHelper.GetCountry(post.IP);

            post.Username = token.Username;

            UpdateItem(repository => repository.AddPost(post));

            PostAdded(this, new ItemEventArgs(post));
        }

        public void AddResponse(PostedItem donation, Response response)
        {
            if (IsValid(donation))
            {
                UpdateItem(repository =>
                {
                    donation = repository.GetPost(donation.Id);
                    donation.Responses.Add(response);
                });
                
                ResponseAdded(this, new ResponseAddedEventArgs(donation, response));
            }
        }        

        public void Delete(int postId, string code)
        {
            UpdateItem(repository =>
            {
                PostedItem item = repository.GetPost(postId);
                if (item != null && item.Code == code)
                    item.Deleted = true;
            });
        }

        public bool Update(PostedItem post)
        {
            if (!token.Principal.IsModerator())
                return false;

            PostedItem existing = GetItem(repository => repository.GetPost(post.Id));            
            UpdateItem(repository => repository.UpdatePost(post));

            if (!existing.Approved && post.Approved)
                PostApproved(this, new ItemEventArgs(post));

            return true;
        }

        public void RenewAll(TimeSpan exipringIn, TimeSpan renewSpan)
        {
            if (!token.Principal.IsAdmin())
                return;

            UpdateItem(repository =>
            {
                IEnumerable<PostedItem> items = repository.GetRenewableItems(exipringIn);
                foreach (PostedItem item in items)
                    item.ExpiresOn += renewSpan;
            });
        }

        public void ResendConfirmation(int donationId)
        {
            if (!token.Principal.IsModerator())
                return;

            PostedItem donation = GetItem(repository => repository.GetPost(donationId));
            if (donation != null && !(donation.Expired || donation.Deleted))
                ResendConfirmationEmail(this, new ItemEventArgs(donation));
        }

        public void PurgeAll(DateTime beforeDate)
        {
            if (!token.Principal.IsAdmin())
                return;

            IEnumerable<PostedItem> result = GetAndUpdate(repository =>
            {
                var items = repository.GetDeleted(beforeDate);
                foreach (PostedItem item in items)
                    repository.DeletePost(item);
                return items;
            });
            
            foreach (PostedItem item in result)
                OnItemPurged(item);
        }

        public bool Purge(int donationId)
        {
            if (!token.Principal.IsAdmin())
                return false;

            PostedItem result = GetAndUpdate(repository =>
            {
                PostedItem item = repository.GetPost(donationId);
                if (item != null && item.Deleted)
                {
                    repository.DeletePost(item);
                    return item;
                }
                return null;
            });

            bool purged = result != null;
            
            if (purged)
                OnItemPurged(result);

            return purged;
        }

        public bool Restore(int id)
        {
            if (!token.Principal.IsModerator())
                return false;

            bool restored = GetAndUpdate(repository =>
            {
                PostedItem item = repository.GetPost(id);
                if (item != null)
                {
                    item.Deleted = false;
                    return true;
                }
                return false;
            });

            return restored;
        }

        public PostedItem Get(int id)
        {
            PostedItem item = GetItem(repository => repository.GetPost(id));
            if (!IsValid(item))
                item = null;

            return item;
        }

        void OnItemPurged(PostedItem item)
        {
            PostPurged(this, new ItemEventArgs(item));
        }

        bool IsValid(PostedItem item)
        {
            return item != null && (token.Principal.IsModerator() || item.IsValid);
        }        
    }
}