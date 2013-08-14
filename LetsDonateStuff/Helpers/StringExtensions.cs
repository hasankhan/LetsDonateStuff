using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Text;

namespace LetsDonateStuff.Helpers
{
    public static class StringExtensions
    {
        const int MaxSlugLength = 45;

        /// <summary>
        /// Generates a slug for the specified string.
        /// </summary>
        /// <param name="value">The string for which to generate the slug.</param>
        /// <returns>A URL-friendly version of the given string.</returns>
        public static string GenerateSlug(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("The given value cannot be null or empty.");
            }
            string str = value.RemoveAccent().ToLower(CultureInfo.CurrentCulture);
            str = Regex.Replace(str, @"[^a-z0-9\s-]", string.Empty); // Remove invalid chars
            str = Regex.Replace(str, @"\s+", " ").Trim(); // Convert multiple spaces into one space
            str = str.Substring(0, str.Length <= MaxSlugLength ? str.Length : MaxSlugLength).Trim(); // Cut and trim
            str = Regex.Replace(str, @"\s", "-"); // convert spaces to hyphens
            return str;
        }

        /// <summary>
        /// Removes accents from a string.
        /// </summary>
        /// <param name="value">The string from which to remove accents.</param>
        /// <returns>The string without any accents.</returns>
        static string RemoveAccent(this string value)
        {
            byte[] bytes = Encoding.GetEncoding("Cyrillic").GetBytes(value);
            return Encoding.ASCII.GetString(bytes);
        }

        const string Ellipsis = "...";

        /// <summary>
        /// Trims a string and shows an ellipsis at the end if it was trimmed.
        /// </summary>
        /// <param name="value">The value to trim.</param>
        /// <param name="maxLength">The maximum length for the string.</param>
        /// <returns>The trimmed string with an ellipsis at the end if it was trimmed.</returns>
        public static string TrimWithEllipsis(this string value, int maxLength=50)
        {
            if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
                return value;
            var trimmed = value.Substring(0, maxLength - Ellipsis.Length).Trim() + Ellipsis;
            return trimmed;
        }

        public static string ToTitleCase(this string value)
        {
            string result = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value);
            return result;
        }

        public static string NullIfEmpty(this string value)
        {
            return String.IsNullOrWhiteSpace(value) ? null : value;
        }
    }
}