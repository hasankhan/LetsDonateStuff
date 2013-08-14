using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace LetsDonateStuff.DAL
{
    public class Donation: PostedItem
    {
        [DefaultValue(2)]
        public int ConditionCode { get; set; }

        [NotMapped]
        public DonationCondition Condition
        {
            get { return (DonationCondition)ConditionCode; }
            set { ConditionCode = (int)value; }
        }
        
        [StringLength(250)]
        public string ImageUrlSmall { get; set; }
        
        [StringLength(250)]
        public string ImageUrlOriginal { get; set; }
        
        [StringLength(250)]
        public string ImageDelHash { get; set; }
    }
}