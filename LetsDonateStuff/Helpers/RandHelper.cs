using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace LetsDonateStuff.Helpers
{
    public class RandHelper
    {
        static Random rand = new Random();

        static char[] randChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890 ".ToArray();
        public static string GetString(int length, bool useSpace = false)
        {
            var output = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                int count = useSpace ? randChars.Length : randChars.Length - 1;
                int index = rand.Next(count);
                char next = randChars[index];
                output.Append(next);
            }
            return output.ToString();
        }

        public static Tuple<float, float> GetLatLong()
        {
            float latitude = - 90 + (float)rand.NextDouble() * 180;
            float longitude = - 180 + (float)rand.NextDouble() * 360;
            var result = new Tuple<float, float>(latitude, longitude);
            return result;
        }
    }
}