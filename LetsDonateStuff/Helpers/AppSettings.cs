using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Configuration;

namespace LetsDonateStuff.Helpers
{
    public class AppSettings
    {
        public string AdminEmail { get; private set; }
        public string AdminOpenId { get; private set; }

        public AppSettings()
        {
            var properties = typeof(AppSettings).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead && p.CanWrite && p.SetMethod.IsPrivate);
            foreach (PropertyInfo property in properties)
                property.SetValue(this, WebConfigurationManager.AppSettings[property.Name]);
        }
    }
}