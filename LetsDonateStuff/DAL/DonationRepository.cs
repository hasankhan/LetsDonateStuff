using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LinqKit;
using System.Linq.Expressions;
using System.Data;
using System.Data.Entity;

namespace LetsDonateStuff.DAL
{
    public class DonationRepository: Repository
    {
        public IEnumerable<PostedItem> GetPosts(string query, string country, int pageNo, int pageSize, PostType type, bool all)
        {
            int skip = pageNo * pageSize;

            IQueryable<PostedItem> posts = GetPostsByType(type);

            var donations = GetPostQuery(posts, query, country, all)
                            .OrderByDescending(d => d.PostedOn)
                            .Skip(skip)
                            .Take(pageSize);
                            
            return donations.ToList();
        }

        public int GetPostsCount(string query, string country, PostType type, bool all)
        {
            IQueryable<PostedItem> posts = GetPostsByType(type);
            int count = GetPostQuery(posts, query, country, all).Count();
            return count;
        }

        public PostedItem GetPost(int id)
        {
            PostedItem post = Context.Posts
                                    .Include("Responses")
                                    .FirstOrDefault(d => d.Id == id);
            return post;
        }

        public IEnumerable<PostedItem> GetDeleted(DateTime beforeDate)
        {
            var posts = from post in Context.Posts
                        where post.ExpiresOn < beforeDate
                        select post;

            return posts.ToList();
        }        

        public void AddPost(PostedItem item)
        {
            Context.Posts.Add(item);
        }

        public void DeletePost(PostedItem item)
        {
            Context.Posts.Remove(item);
        }

        public IEnumerable<PostedItem> GetRenewableItems(TimeSpan expiringIn)
        {
            DateTime expiry = DateTime.UtcNow + expiringIn;

            var posts = from post in Context.Posts
                        where post.Approved && 
                        !post.Deleted &&
                        post.ExpiresOn < expiry && 
                        !post.Responses.Any()
                        select post;

            return posts.ToList();

        }        

        public void UpdatePost(PostedItem item)
        {
            Update(Context.Posts, item);
        }

        IQueryable<PostedItem> GetPostsByType(PostType type)
        {
            IQueryable<PostedItem> posts;

            if (type == PostType.Donation)
                posts = Context.Posts.OfType<Donation>();
            else if (type == PostType.DonationRequest)
                posts = Context.Posts.OfType<DonationRequest>();
            else
                posts = Context.Posts;
            return posts;
        }

        static IQueryable<T> GetPostQuery<T>(IQueryable<T> query, string keywords, string country, bool all) where T:PostedItem
        {
            if (!String.IsNullOrWhiteSpace(country))
            {
                country = country.Trim().ToUpper();
                query = query.Where(d => d.Country == country);
            }

            if (!all)
            {
                DateTime now = DateTime.UtcNow;
                query = query.Where(d => d.Approved && !d.Deleted && d.ExpiresOn > now);
            }
            query = LinqHelper.SearchByKeywords(query, keywords, keyword => d => d.Title.Contains(keyword) || d.Description.Contains(keyword));
            return query;
        }
    }
}