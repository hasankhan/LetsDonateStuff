using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LetsDonateStuff.DAL
{
    public class Response
    {
        [Key]
        public int Id { get; set; }
        
        public int PostedItemId { get; set; }
        
        [Required, StringLength(50)]
        public string Name { get; set; }
        
        [Required, StringLength(255)]
        public string Email { get; set; }
        
        [Required]
        public DateTime SentOn { get; set; }
        
        [Required, StringLength(39)]
        public string IP { get; set; }
        
        [Required, NotMapped, StringLength(500)]
        public string Message { get; set; }
    }
}