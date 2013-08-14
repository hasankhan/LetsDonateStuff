using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.ModelConfiguration;

namespace LetsDonateStuff.DAL
{
    public class DonationsContext: DbContext
    {
        public DbSet<PostedItem> Posts { get; set; }
        public DbSet<Response> Responses { get; set; }
    }
}