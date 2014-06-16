using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using LetsDonateStuff;
using LetsDonateStuff.DAL;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(DatabaseConfig), "Start")]

namespace LetsDonateStuff
{
    public class DatabaseConfig
    {
        public static void Start()
        {
            Database.SetInitializer<DonationsContext>(new DonationsInitializer());
        }
    }
}