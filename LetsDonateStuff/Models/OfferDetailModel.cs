using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using LetsDonateStuff.DAL;

namespace LetsDonateStuff.Models
{
    public class OfferDetailModel: PostDetailModel
    {
        public DonationCondition Condition { get; set; }               
        
        public string ImageUrlSmall { get; set; }
        
        public string ImageUrlOriginal { get; set; }
    }
}