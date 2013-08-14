using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LetsDonateStuff.Helpers.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class LatLongAttribute: ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;

            dynamic model = value;
            bool isValid = !(model.Latitude == 0 && model.Longitude == 0);
            return isValid;
        }
    }
}