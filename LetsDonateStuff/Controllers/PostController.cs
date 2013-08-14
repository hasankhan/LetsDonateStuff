using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LetsDonateStuff.DAL;
using PagedList;
using LetsDonateStuff.Models;
using LetsDonateStuff.Helpers;
using LetsDonateStuff.Mailers;
using Mvc.Mailer;
using System.Configuration;
using System.IO;
using Elmah;
using MaxMind.GeoIP;
using System.Net.Mail;
using System.ServiceModel.Syndication;
using System.Drawing;
using LetsDonateStuff.Helpers.GeoIP;
using LetsDonateStuff.Helpers.PubSubHubbub;
using LetsDonateStuff.Services;
using LetsDonateStuff.Helpers.Attributes;
using LetsDonateStuff.Helpers.ActionResults;
using Microsoft.Practices.Unity;
using LetsDonateStuff.Filters;
using System.Web.Security;

namespace LetsDonateStuff.Controllers
{
    public class PostController : Controller
    {
        const int pageSize = 5;
        Lazy<DonationService> donationService;
        GeoIPHelper geoIPHelper;
        Imgur imgur;

        public PostController(Imgur imgur, GeoIPHelper geoIPHelper, Lazy<DonationService> service)
        {
            this.imgur = imgur;
            this.geoIPHelper = geoIPHelper;
            this.donationService = service;
        }

        [GeoIPCountry("country", "c")]
        public ActionResult Index([Bind(Prefix = "p")] int? pageNumber,
                                 [Bind(Prefix = "q")] string query,
                                 [Bind(Prefix = "c")] string country)
        {
            int pageIndex = Math.Max(pageNumber.GetValueOrDefault(1) - 1, 0);
            
            PostSearchResult result = donationService.Value.Search(query, country, pageIndex, pageSize, PostType.Both);

            foreach (var post in result.Posts)
                post.Title = post.Title.ToTitleCase();
            
            var pages = new StaticPagedList<PostedItem>(result.Posts, pageIndex + 1, pageSize, result.Count);

            ViewBag.Pages = pages;            

            SetCountry(country, !result.Posts.Any());

            ViewBag.Query = query;
            ViewBag.Country = (country ?? "").ToUpper();

            string viewName;
            if ((ModelState.ContainsKey("q") || ModelState.ContainsKey("p")))
                viewName = "Index2";
            else
            {
                viewName = "Index";
                int totalOffers = donationService.Value.GetPostsCount(country, PostType.Donation, User.IsModerator());
                ViewBag.TotalOffers = totalOffers;
            }

            return View(viewName, result.Posts);
        }

        public ActionResult ItemDoesNotExist()
        {
            return View();
        }

        [OutputCache(VaryByParam = "c;q", Duration = 600)]
        public ActionResult Feed([Bind(Prefix = "c")]string country, [Bind(Prefix = "q")] string query)
        {
            var result = donationService.Value.Search(query, country, 0, 100, PostType.Both, false);

            IEnumerable<string> hubUrls = MiscUtility.GetHubUrls();
            return new DonationFeed(result.Posts, hubUrls, Url);
        }

        [AuthorizeUser]
        public ActionResult ResendConfirmation(int id, string returnUrl)
        {
            returnUrl = returnUrl ?? Url.Action("Index");

            PostedItem post = donationService.Value.Get(id);
            if (post != null)
                donationService.Value.ResendConfirmation(id);

            return Redirect(returnUrl);
        }

        [AuthorizeUser]
        public ActionResult Offer(MembershipUser user)
        {
            var donation = GetCreateModel<OfferCreateModel>(user);
            donation.Condition = DonationCondition.Used;
            return View(donation);
        }

        [HttpPost/*, Captcha*/, AuthorizeUser, ValidateInput(false)]
        public ActionResult Offer(OfferCreateModel createModel, MembershipUser user)
        {
            if (!ModelState.IsValid)
                return View(createModel);

            var donation = new Donation();
            donation.Condition = createModel.Condition;
            CreateModelToPostedItem(createModel, donation, user);

            if (createModel.Image != null)
                ExceptionMonster.EatException(() =>
                {
                    var result = imgur.Upload(createModel.Image.InputStream);
                    donation.ImageUrlSmall = result.SmallSquare.ToString();
                    donation.ImageUrlOriginal = result.Original.ToString();
                    donation.ImageDelHash = result.DeleteHash;
                });

            donationService.Value.Add(donation);

            return RedirectToAction("Thankyou");
        }

        [AuthorizeUser]
        public ActionResult Need(MembershipUser user)
        {
            var request = GetCreateModel<PostCreateModel>(user);
            return View(request);
        }

        [HttpPost/*, Captcha*/, AuthorizeUser, ValidateInput(false)]
        public ActionResult Need(PostCreateModel createModel, MembershipUser user)
        {
            if (!ModelState.IsValid)
                return View(createModel);

            var request = new DonationRequest();
            CreateModelToPostedItem(createModel, request, user);

            donationService.Value.Add(request);
            return RedirectToAction("Thankyou");
        }        

        [HttpPost, Captcha, ValidateInput(false)]
        public ActionResult ContactPoster(ContactModel model)
        {
            if (!ModelState.IsValid)
            {
                var detail = GetPostDetail(model.ID);
                model.Show = true;
                ViewBag.ContactModel = model;
                return View("Details", detail);
            }

            var request = new Response()
            {
                Name = model.Name,
                Email = model.Email.Trim().ToLower(),
                Message = model.Message,
                SentOn = DateTime.UtcNow,
                IP = Request.UserHostAddress,
            };

            PostedItem post = donationService.Value.Get(model.ID);
            donationService.Value.AddResponse(post, request);

            return RedirectToAction("MessageSent");
        }

        public ActionResult MessageSent()
        {
            return View();
        }

        public ActionResult Thankyou()
        {
            return View();
        }

        public ActionResult Delete(int id, string code, string mode)
        {
            ViewBag.Ajax = ((mode ?? "").ToLower() == "ajax");
            PostedItem post = donationService.Value.Get(id);
            if (post != null && !User.IsModerator() && post.Code != code)
                post = null;
            return View(post);
        }

        [HttpPost]
        public ActionResult Delete2(int id, string code)
        {
            donationService.Value.Delete(id, code);
            return RedirectToAction("Index");
        }

        [AuthorizeUser(Roles = "Admin")]
        public ActionResult Purge(int id)
        {
            donationService.Value.Purge(id);
            
            return RedirectToAction("Index");
        }

        [AuthorizeUser]
        public ActionResult Restore(int id)
        {
            donationService.Value.Restore(id);

            return RedirectToAction("Details", new { id = id });
        }

        [AuthorizeUser]
        public ActionResult Edit(int id)
        {
            PostedItem item = donationService.Value.Get(id);
            if (item == null)
                return FileNotFound("Post does not exist.");

            PostEditModel editModel = PropertyCopyPolymorphic<PostEditModel, PostEditModel, PostedItem, DonationRequest, Donation>(item);
            
            editModel.Type = item is Donation ? ModelType.Donation : ModelType.Request;
            editModel.GeoIPCountry = geoIPHelper.GetCountry(item.IP);

            return View(editModel);
        }        

        [Authorize, HttpPost, ValidateInput(false)]
        public ActionResult Edit(PostEditModel editModel)
        {
            if (!ModelState.IsValid)
                return View(editModel);

            PostedItem post = donationService.Value.Get(editModel.Id);
            if (post != null)
            {
                bool wasApproved = post.Approved;

                post.Address = editModel.Address;
                post.Approved = editModel.Approved;                
                post.Country = editModel.Country;
                post.Description = editModel.Description;
                post.ExpiresOn = editModel.ExpiresOn;
                post.Latitude = editModel.Latitude;
                post.Longitude = editModel.Longitude;
                post.Locality = editModel.Locality;
                post.Title = editModel.Title;
                post.Name = editModel.Name;
                if (User.IsAdmin())
                    post.Email = editModel.Email;

                if (post is Donation)
                {
                    var donation = (Donation)post;
                    donation.Condition = editModel.Condition;
                    donation.ImageUrlOriginal = editModel.ImageUrlOriginal;
                    donation.ImageUrlSmall = editModel.ImageUrlSmall;                                   
                }                

                donationService.Value.Update(post);
                if (!wasApproved && post.Approved)
                    OnDonationPublished(post);
            }
            return RedirectToAction("Details", new { id = editModel.Id, slug = post.Slug });
        }

        public ActionResult Details(int id)
        {
            PostDetailModel detailModel = GetPostDetail(id);           
            if (detailModel == null)
                return RedirectToActionPermanent("ItemDoesNotExist");

            ViewBag.ContactModel = new ContactModel() { ID = id };
            return View(detailModel);
        }

        ActionResult FileNotFound(string message)
        {
            return new FileNotFoundResult() { Message = message };
        }

        void CreateModelToPostedItem(PostCreateModel createModel, PostedItem item, MembershipUser user)
        {
            item.Address = createModel.Address;
            item.Locality = createModel.Locality;
            item.Country = createModel.Country;
            item.PublishOnOtherSites = createModel.PublishOnOtherSites;
            item.Approved = false;
            item.Title = createModel.Title;
            item.Description = createModel.Description;
            item.Latitude = createModel.Latitude;
            item.Longitude = createModel.Longitude;
            item.PostedOn = DateTime.UtcNow;
            item.ExpiresOn = DateTime.UtcNow.AddDays(7);
            item.IP = Request.UserHostAddress;
            item.Code = RandHelper.GetString(10);
            item.Name = user.UserName ?? createModel.Name;
            item.Email = user.Email;
        }

        PostDetailModel GetPostDetail(int id)
        {
            PostedItem item = donationService.Value.Get(id);
            if (item == null)
                return null;

            PostDetailModel detailModel = PropertyCopyPolymorphic<PostDetailModel, OfferDetailModel, PostedItem, DonationRequest, Donation>(item);

            detailModel.CountryCode = item.Country ?? String.Empty;
            detailModel.Country = CountryList.GetCountry(item.Country).Coalesce(c => c.Name, String.Empty);
            detailModel.Title = item.Title.ToTitleCase();

            if (User.IsAdmin())
                detailModel.Responses = item.Responses.ToList();

            return detailModel;
        }

        void SetCountry(string code, bool zoom)
        {
            var item = CountryList.GetCountry(code);
            ViewBag.CountryName = item.Coalesce(i => i.Name, CountryList.AllCountries);
            var location = item.Coalesce(i => i.Coordinates, new PointF());
            ViewBag.Latitude = location.X;
            ViewBag.Longitude = location.Y;
            if (zoom)
                ViewBag.Zoom = 3;
        }

        void OnDonationPublished(PostedItem post)
        {
            string countryFeed = Url.Action("Feed", new { c = post.Country });
            string globalFeed = Url.Action("Feed");

            HttpResponse.RemoveOutputCacheItem(countryFeed);
            HttpResponse.RemoveOutputCacheItem(globalFeed);

            string countryFeedUrl = Url.SiteUrl() + countryFeed;
            string globalFeedUrl = Url.SiteUrl() + globalFeed;

            FeedSignal.Instance.Signal(countryFeedUrl);
            FeedSignal.Instance.Signal(globalFeedUrl);
        }

        TCreateModel GetCreateModel<TCreateModel>(MembershipUser user) where TCreateModel : PostCreateModel, new()
        {
            var donation = new TCreateModel();

            donation.Name = user.UserName;
            donation.Email = user.Email;
            PointF location = geoIPHelper.GetLocation(Request.UserHostAddress);
            donation.Latitude = location.X;
            donation.Longitude = location.Y;
            donation.PublishOnOtherSites = true;

            return donation;
        }

        static TTargetBase PropertyCopyPolymorphic<TTargetBase, TTargetDerived, TSourceBase, TSourceDerived1, TSourceDervied2>(TSourceBase item)
            where TSourceBase : class
            where TSourceDerived1 : class, TSourceBase
            where TSourceDervied2 : class, TSourceBase
            where TTargetBase : class, new()
            where TTargetDerived: class, TTargetBase, new()
        {
            TTargetBase result = null;
            if (item is TSourceDerived1)
                result = PropertyCopy<TTargetBase>.Copy((TSourceDerived1)item);
            else if (item is TSourceDervied2)
                result = PropertyCopy<TTargetDerived>.Copy((TSourceDervied2)item);
            else
                result = PropertyCopy<TTargetBase>.Copy(item);
            return result;
        }
    }
}
