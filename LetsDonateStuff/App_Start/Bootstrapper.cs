using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using LetsDonateStuff.DAL;
using LetsDonateStuff.Helpers;
using LetsDonateStuff.Helpers.GeoIP;
using LetsDonateStuff.Mailers;
using LetsDonateStuff.Services;
using Microsoft.AspNet.Identity.Owin;

namespace LetsDonateStuff
{
    public class Bootstrapper
    {
        public static void Initialize()
        {
            IContainer container = null;
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.Register<HttpContextBase>(_ => new HttpContextWrapper(HttpContext.Current));
            builder.Register<IPrincipal>(_ => HttpContext.Current.User);
            builder.Register<ApplicationUserManager>(_ => HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>());
            builder.RegisterType<AppSettings>();
            builder.RegisterType<DonationServiceObserver>();
            builder.RegisterType<Imgur>()
                   .WithParameter("apiKey", ConfigurationManager.AppSettings["imgurKey"]);
            builder.RegisterType<GeoIPHelper>();
            builder.RegisterType<UserMailer>();
            builder.RegisterType<UserToken>();
            builder.RegisterType<DonationRepository>();
            builder.RegisterType<UserResolver>();
            builder.RegisterType<DonationService>()
                   .OnActivated(e => e.Context.Resolve<DonationServiceObserver>(new TypedParameter(typeof(DonationService), e.Instance)));

            container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}