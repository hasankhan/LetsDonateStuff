using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace LetsDonateStuff.Models
{
    public class ContactModel
    {
        public int ID { get; set; }
        
        [Required, DisplayName("Your Name"), StringLength(50)]
        public string Name { get; set; }
        
        [Required, DisplayName("Your Email"), StringLength(255), DataType(DataType.EmailAddress), RegularExpression("^.+?@.+?\\..+?$", ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required, StringLength(500), DataType(DataType.MultilineText)]
        public string Message { get; set; }

        public bool Show { get; set; }
    }
}