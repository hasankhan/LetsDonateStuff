using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using LetsDonateStuff;
using LetsDonateStuff.DAL;
using WebMatrix.WebData;

[assembly: WebActivator.PostApplicationStartMethod(typeof(DatabaseConfig), "Start")]

namespace LetsDonateStuff
{
    public class DatabaseConfig
    {
        public static void Start()
        {
            Database.SetInitializer<DonationsContext>(new DonationsInitializer());
            using (var context = new DonationsContext())
                context.Database.Initialize(force: false);
            WebSecurity.InitializeDatabaseConnection("DonationsContext", "Users", "Id", "Name", autoCreateTables: true);
        }
    }
}