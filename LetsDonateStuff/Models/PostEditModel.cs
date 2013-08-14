using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LetsDonateStuff.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using LetsDonateStuff.DAL;

namespace LetsDonateStuff.Models
{
    public enum ModelType
    {
        Request,
        Donation
    }

    [LatLong(ErrorMessage = "Please type a valid address or select a point on map.")]
    public class PostEditModel
    {
        public ModelType Type { get; set; }

        public int Id { get; set; }

        public string Slug { get; set; }

        [Required, StringLength(160)]
        public string Title { get; set; }

        [Required, StringLength(50), DisplayName("Posted By")]
        public string Name { get; set; }

        [Required, StringLength(255), DataType(DataType.EmailAddress), RegularExpression("^.+?@.+?\\..+?$", ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required, StringLength(500), DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required, DisplayName("Posted On")]
        public DateTime PostedOn { get; set; }

        [Required, DataType(DataType.Date), DisplayName("Expires On")]
        public DateTime ExpiresOn { get; set; }

        [Required]
        public float Latitude { get; set; }

        [Required]
        public float Longitude { get; set; }

        [StringLength(250)]
        public string Locality { get; set; }

        [StringLength(2)]
        public string Country { get; set; }

        [DisplayName("GeoIP Country")]
        public string GeoIPCountry { get; set; }

        [Required, StringLength(500), DataType(DataType.MultilineText)]
        public string Address { get; set; }

        public string IP { get; set; }

        public bool Approved { get; set; }

        public bool Deleted { get; set; }

        public DonationCondition Condition { get; set; }

        [DisplayName("Image Small"), StringLength(255)]
        public string ImageUrlSmall { get; set; }

        [DisplayName("Image Original"), StringLength(255)]
        public string ImageUrlOriginal { get; set; }

        [DisplayName("Also publish on other sites (e.g. Freecycle)")]
        public bool PublishOnOtherSites { get; set; }
    }
}