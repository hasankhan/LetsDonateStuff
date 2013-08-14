using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LetsDonateStuff.Helpers.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LetsDonateStuff.Models
{
    [LatLong(ErrorMessage = "Please type a valid address or select a point on map.")]
    public class PostCreateModel
    {
        public string Name { get; set; }
        public string Email { get; set; }

        [Required, StringLength(160), DisplayName("Title")]
        public string Title { get; set; }

        [Required, StringLength(500), DataType(DataType.MultilineText), DisplayName("Description")]
        public string Description { get; set; }

        [Required]
        public float Latitude { get; set; }

        [Required]
        public float Longitude { get; set; }

        [Required, StringLength(500), DataType(DataType.MultilineText), DisplayName("Address")]
        public string Address { get; set; }

        [StringLength(250)]
        public string Locality { get; set; }

        [StringLength(2)]
        public string Country { get; set; }

        [DisplayName("Also publish on other sites (e.g. Freecycle)")]
        public bool PublishOnOtherSites { get; set; }
    }
}