using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Elmah;

namespace LetsDonateStuff.Helpers
{
    public class ExceptionMonster
    {
        public static void EatException(Action action)
        {
            try
            {
                action();
            }
            catch (System.Net.WebException ex)
            {
                Log(ex);
            }
        }

        static void Log(Exception ex)
        {
            ErrorLog.GetDefault(null).Log(new Error(ex));
        }
    }
}