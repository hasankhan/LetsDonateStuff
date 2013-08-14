using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using LetsDonateStuff.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace LetsDonateStuff.DAL
{
    public abstract class PostedItem
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(160)]
        public string Title { get; set; }

        [Required, StringLength(500)]
        public string Description { get; set; }

        [Required]
        public string Username { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        [Required, StringLength(255)]
        public string Email { get; set; }

        [Required]
        public DateTime ExpiresOn { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        [Required, StringLength(500)]
        public string Address { get; set; }

        [StringLength(250)]
        public string Locality { get; set; }

        [StringLength(2)]
        public string Country { get; set; }

        [Required]
        public DateTime PostedOn { get; set; }

        [Required, StringLength(39)]
        public string IP { get; set; }

        public bool Approved { get; set; }

        [StringLength(10)]
        public string Code { get; set; }

        public bool Deleted { get; set; }

        [NotMapped]
        public bool Expired
        {
            get { return ExpiresOn < DateTime.UtcNow; }
        }

        [NotMapped]
        public bool IsValid
        {
            get { return !Deleted && Approved; }
        }

        [NotMapped]
        public string Slug
        {
            get { return Title.GenerateSlug(); }
        }

        [NotMapped]
        public bool PublishPending
        {
            get { return !PublishedOnce && PublishOnOtherSites; }
        }

        // Whether the item was ever approved
        public bool PublishedOnce { get; set; }

        // Whether user agreed to publish the item to 3rd party sites
        public bool PublishOnOtherSites { get; set; }

        public ICollection<Response> Responses { get; set; }
    }
}