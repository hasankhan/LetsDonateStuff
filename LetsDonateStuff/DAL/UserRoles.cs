using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace LetsDonateStuff.DAL
{
    public class UserRoles
    {
        public static readonly string Admin;
        public static readonly string Moderator;

        static UserRoles()
        {
            var fields = typeof(UserRoles).GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (FieldInfo field in fields)
                field.SetValue(null, field.Name);
        }
    }
}