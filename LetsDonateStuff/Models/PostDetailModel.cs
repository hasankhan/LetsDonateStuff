using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using LetsDonateStuff.DAL;

namespace LetsDonateStuff.Models
{
    public class PostDetailModel
    {
        public int Id { get; set; }

        public string Slug { get; set; }

        [DisplayName("Posted By")]
        public string Name { get; set; }

        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName("Posted On")]
        public DateTime PostedOn { get; set; }

        [DisplayName("Expires On")]
        public DateTime ExpiresOn { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        public string Country { get; set; }
        public string CountryCode { get; set; }

        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        public IEnumerable<Response> Responses { get; set; }
    }
}