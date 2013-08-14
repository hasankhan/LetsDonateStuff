using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using LetsDonateStuff.Services;
using LetsDonateStuff.Helpers;
using System.Data.Entity.Migrations;

namespace LetsDonateStuff.DAL
{
    class DonationsInitializer : CreateDatabaseIfNotExists<DonationsContext>
    {
        protected override void Seed(DonationsContext context)
        {
            base.Seed(context);

#if DEBUG
            AddDummyData(context, UserToken.Admin.Username);
#endif
        }

        static void AddDummyData(DonationsContext context, string username)
        {
            if (context.Posts.Any())
                return;

            var countries = CountryList.All.ToList();
            var random = new Random();

            for (int i = 0; i < 100; i++)
            {
                var latlong = RandHelper.GetLatLong();
                var donation = new Donation()
                {
                    Address = RandHelper.GetString(50, true),
                    Approved = true,
                    Description = RandHelper.GetString(50, true),
                    Title = RandHelper.GetString(20, true),
                    IP = "192.168.0.1",
                    ExpiresOn = DateTime.UtcNow.AddDays(3),
                    Latitude = latlong.Item1,
                    Longitude = latlong.Item2,
                    PostedOn = DateTime.UtcNow,
                    Code = RandHelper.GetString(5),
                    Country = countries[random.Next(countries.Count)].Code,
                    Name = "Hasan",
                    Email = "email@domain.com",
                    Username = username
                };
                context.Posts.Add(donation);
            }

            context.SaveChanges();
        }
    }
}