using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LetsDonateStuff.DAL;

namespace LetsDonateStuff.Services
{
    public class PostSearchResult
    {
        public IEnumerable<PostedItem> Posts { get; set; }

        public int Count { get; set; }
    }
}