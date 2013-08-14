using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using LetsDonateStuff.Helpers;
using System.ComponentModel;
using LetsDonateStuff.Helpers.Attributes;
using LetsDonateStuff.DAL;

namespace LetsDonateStuff.Models
{
    public class OfferCreateModel: PostCreateModel
    {
        public DonationCondition Condition { get; set; }        

        [File(AllowedFileExtensions=new [] {".jpg", ".jpeg", ".png"},
              AllowedContentTypes = new [] {"image/jpeg", "image/png"},
              MaxContentLength = 1024 * 1024,
              IsImage = true)]
        public HttpPostedFileBase Image { get; set; }
    }
}