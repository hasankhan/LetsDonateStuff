using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc3;
using LetsDonateStuff.Services;
using DotNetOpenAuth.OpenId.RelyingParty;
using LetsDonateStuff.Helpers;
using System.Configuration;
using LetsDonateStuff.DAL;
using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using LetsDonateStuff.Services.Publishing;
using LetsDonateStuff.Services.Publishing.Targets;
using System.Collections.Generic;
using LetsDonateStuff.Services.Publishing.Targets.Freecycle;
using LetsDonateStuff.Mailers;
using LetsDonateStuff.Controllers;
using LetsDonateStuff.Configuration;
using LetsDonateStuff.Configuration.Freecycle;

namespace LetsDonateStuff
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            container.RegisterType<AppSettings>(new ContainerControlledLifetimeManager());
            container.RegisterInstance(new OpenIdRelyingParty());
            container.RegisterInstance(new Imgur(ConfigurationManager.AppSettings["imgurKey"]));
            container.RegisterInstance<MembershipProvider>(Membership.Provider);
            container.RegisterInstance<RoleProvider>(Roles.Provider);
            container.RegisterType<HttpContextBase>(new InjectionFactory(_ => new HttpContextWrapper(HttpContext.Current)));
            container.RegisterType<IPrincipal>(new InjectionFactory(_ => HttpContext.Current.User));
            container.RegisterType<IEnumerable<IPublishingTarget>>(new InjectionFactory(_ => container.ResolveAll<IPublishingTarget>()));
            container.RegisterType<IMembershipService, MembershipService>();
            
            RegisterFreecycle(container);

            RegisterService(container);
            
            return container;
        }

        static void RegisterFreecycle(UnityContainer container)
        {
            FreecycleSettings settings = FreecycleSection.GetSettings();
            container.RegisterType<FreecycleMailer>(new InjectionFactory(_ => new FreecycleMailer(settings.From)));
            container.RegisterType<IPublishingTarget>("freecycle", new InjectionFactory(_ =>
                container.Resolve<FreecycleTarget>(new ParameterOverride("groups", settings.Groups))));
        }

        static void RegisterService(UnityContainer container)
        {
            container.RegisterType<IAuthenticationService, AuthenticationService>(new ContainerControlledLifetimeManager());
            //container.RegisterType<IMembershipService, MembershipService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IPublishingService, PublishingService>();
            container.RegisterType<Lazy<DonationService>>(new InjectionFactory(_ =>
                new Lazy<DonationService>(() => container.Resolve<DonationServiceObserver>().DonationService)));
        }
    }
}