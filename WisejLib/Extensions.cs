using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace WisejLib
{
    /// <summary>
    /// Defines where to add a wildcard (see AppendWidlcards method)
    /// </summary>
    public enum WildcardMode { 
        /// <summary>
        /// Insert the percent wildcard at the beginning
        /// </summary>
        Left, 
        /// <summary>
        /// Append the percent wildcard at the end
        /// </summary>
        Right, 
        /// <summary>
        /// Add a percent wildcard at the beginning and at the end
        /// </summary>
        Both 
    }

    /// <summary>
    /// Class with a couple of extension methods, mainly for strings
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Append 2 strings with a separator
        /// </summary>
        /// <param name="currentValue">The original value (can be empty)</param>
        /// <param name="valueToAppend">The value to append (can be null or empty)</param>
        /// <param name="separator">The separator between original value and value to append</param>
        /// <returns>The combined string</returns>
        public static string Append(this string currentValue, string valueToAppend, string separator = ", ")
        {
            if (string.IsNullOrEmpty(currentValue))
                return valueToAppend;

            if (string.IsNullOrEmpty(valueToAppend))
                return currentValue;

            if (separator is null)
                separator = string.Empty;
            return $"{currentValue}{separator}{valueToAppend}";
        }

        /// <summary>
        /// Replaces DOS wildcards and then adds SQL wildcards to a string if they don't already exist
        /// </summary>
        /// <param name="originalValue">The value to which wildcards are added</param>
        /// <param name="mode">WildcardMode { Left, Right, Both }</param>
        /// <returns>If the original value contains at least 1 wildcard, the original value is 
        /// returned otherwise the function adds wildcards</returns>
        public static string AppendWildcards(this string originalValue, WildcardMode mode = WildcardMode.Both)
        {
            if (string.IsNullOrEmpty(originalValue))
                return string.Empty;

            originalValue = originalValue.Replace("*", "%").Replace("?", "_");
            if (originalValue.Contains("%") || originalValue.Contains("_"))
                return originalValue;

            switch (mode)
            {
                case WildcardMode.Left: return $"%{originalValue}";
                case WildcardMode.Right: return $"{originalValue}%";
                default: return $"%{originalValue}%";
            }
        }

        /// <summary>
        /// Clean creates a new string from the passed value that only contains 
        /// characters that are present in allowedValues
        /// </summary>
        /// <param name="value">The string to clean</param>
        /// <param name="allowedValues">Sting with the characters that are allowed</param>
        /// <returns>Returns cleaned string</returns>
        public static string Clean(this string value, string allowedValues)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(allowedValues))
                return string.Empty;

            var sb = new StringBuilder();
            foreach (var c in value)
                if (allowedValues.Contains(c))
                    sb.Append(c);

            return sb.ToString();
        }

        /// <summary>
        /// Returns a string with only digits, all aother letters are removed
        /// </summary>
        public static string DigitsOnly(this string value)
        {
            return Clean(value, "0123456789");
        }

        /// <summary>
        /// Rudimentary function to extract the domain name from a given URL.
        /// Works with URLs and email addresses
        /// </summary>
        public static string ExtractDomain(this string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return string.Empty;

            string s = url.Trim().ToLower();
            int p = s.IndexOf('@');
            if (p >= 0)
                return s.Substring(p + 1);

            p = s.IndexOf("://");
            if (p >= 0)
                s = s.Substring(p + 3);

            string[] parts = s.Split('.');
            if (parts.Length < 2)
                return url;
            if (parts.Length == 2)
                return s;
            return parts[parts.Length - 2] + "." + parts[parts.Length - 1];
        }

        /// <summary>
        /// Replaces German Umlauts ä, ö, ü etc. with ae, oe, ue, etc.
        /// See also RemoveDiacritics()
        /// </summary>
        public static string ExpandGermanUmlauts(this string value)
        {
            if (value is null)
                return string.Empty;

            string[] umlauts = new string[] { "ä", "ö", "ü", "Ä", "Ö", "Ü", "ß" };
            string[] replacements = new string[] { "ae", "oe", "ue", "Ae", "Oe", "Ue", "ss" };

            string result = value;
            for (int i = 0; i < umlauts.Length; i++)
                result = result.Replace(umlauts[i], replacements[i]);

            return result;
        }

        /// <summary>
        /// Returns true if both strings are equal (case-insensitive).
        /// </summary>
        /// <param name="value1">1st string (can be null)</param>
        /// <param name="value2">2nd string (can be null)</param>
        /// <returns>Retuns true if both strings are equal (case-insensitive)</returns>
        public static bool SameText(this string value1, string value2)
        {
            if (value1 is null || value2 is null)
                return false;
            return value1.Equals(value2, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Returns true if a IBAN is valid.
        /// I found this somewhere on the internet, don't know 100% if it works
        /// </summary>
        public static bool IsValidIBAN(this string iban)
        {
            if (string.IsNullOrWhiteSpace(iban))
                return false;

            string ibanCleared = iban.ToUpper().Replace(" ", "").Replace("-", "");
            string ibanSwapped = ibanCleared.Substring(4) + ibanCleared.Substring(0, 4);
            string sum = ibanSwapped.Aggregate("", (current, c) => current + (char.IsLetter(c) ? (c - 55).ToString() : c.ToString()));

            var d = decimal.Parse(sum);
            return ((d % 97) == 1);
        }

        /// <summary>
        /// Rudimentary check if a phone number is valid (German phone numbers only)
        /// This doesn't cover all possible errors but at least it's a start.
        /// There's quite a few RegEx out there, go and implement this function to your needs
        /// </summary>
        public static bool IsValidPhoneNumber(this string value)
        {
            //
            if (string.IsNullOrWhiteSpace(value))
                return false;

            value = value
                .Replace(" ", "")
                .Replace("(", "")
                .Replace(")", "")
                .Replace("-", "")
                .Replace("/", "");

            if (string.IsNullOrWhiteSpace(value))
                return false;

            string tmp = value[0] == '+' ? value.Substring(1) : value;
            foreach (var c in tmp)
                if (!char.IsDigit(c))
                    return false;
            return true;
        }

        /// <summary>
        /// Checks if an URL is valid.
        /// I found this somewhere on the internet, don't know 100% if it works
        /// </summary>
        public static bool IsValidUrl(this string value) => Uri.TryCreate(value, UriKind.Absolute, out _);

        /// <summary>
        /// Returns everything vom the given value before stop
        /// </summary>
        public static string LeftTillStop(this string value, string stop = "\n")
        {
            int p = value.IndexOf(stop);
            return p < 0 ? value : value.Substring(0, p);
        }

        /// <summary>
        /// Makes sure a string is never longer than the given maxLength. If the string was longer, it is cut.
        /// If the string was longer and appendDots is true, the resulting string is cut and ends with 3 dots
        /// If the string is shorter than maxLength it is returned unchanged
        /// </summary>
        /// <param name="value">The string to be limited in length</param>
        /// <param name="maxLength">The maximum length of the resulting string</param>
        /// <param name="appendDots">If true and the string is longer than mxLength, the string is cur and 3 dots are appended</param>
        /// <returns>A string that is never longer than maxLength</returns>
        public static string LimitLength(this string value, int maxLength, bool appendDots = false)
        {
            if (value.Length <= maxLength)
                return value;
            return (appendDots && maxLength >= 3) ? value.Substring(0, maxLength - 3) + "..." : value.Substring(0, maxLength);
        }

        /// <summary>
        /// This method combines all elements of a list in a string. The method uses a buildFunction delegate
        /// to modifiy each individual element before it is added to the resulting string.
        /// However, when your buildFunction parameter looks like this: "s => s" then 
        /// consider to use "string.Join(separator, list)"
        /// </summary>
        /// <param name="list">The list whose elements are to be put into the resulting string</param>
        /// <param name="buildFunction">A function delegate that passes the element before it is 
        /// added to the resulting and expects the string that is to be added eventually. This
        /// enables the caller to add prefixes or postfixes to the elements before they are added 
        /// to the resulting string</param>
        /// <param name="separator">Separates the elements, default is a comma</param>
        /// <returns>A string that contains all elements of the list</returns>
        public static string ListAsString<T>(this List<T> list, Func<T, string> buildFunction, string separator = ", ")
        {
            var sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                var expression = buildFunction(list[i]);
                sb.Append(i > 0 ? $"{separator}{expression}" : expression);
            }
            return sb.ToString();
        }

        /// <summary>
        /// This method combines all elements of a list in a string. The difference to 
        /// string.Join(separator, list) is that this method uses a buildFunction delegate
        /// to modifiy each individual element before it is added to the resulting string.
        /// However, when your buildFunction parameter looks like this: "s => s" then 
        /// consider to use "string.Join(separator, list)"
        /// </summary>
        /// <param name="list">The list whose elements are to be put into the resulting string</param>
        /// <param name="buildFunction">A function delegate that passes the element before it is 
        /// added to the resulting and expects the string that is to be added eventually. This
        /// enables the caller to add prefixes or postfixes to the elements before they are added 
        /// to the resulting string</param>
        /// <param name="separator">Separates the elements, usually a comma</param>
        /// <returns>A string that contains all elements of the list</returns>
        public static string ListAsString(this List<string> list, Func<string, string> buildFunction, string separator = ", ")
        {
            return list.ListAsString<string>(buildFunction, separator);
        }

        /// <summary>
        /// This method combines all elements of a list in a string. The difference to 
        /// string.Join(separator, list) is that this method uses a buildFunction delegate
        /// to modifiy each individual element before it is added to the resulting string.
        /// However, when your buildFunction parameter looks like this: "i => i.ToString()" then 
        /// consider to use "string.Join(separator, list)"
        /// </summary>
        /// <param name="list">The list whose elements are to be put into the resulting string</param>
        /// <param name="buildFunction">A function delegate that passes the element before it is 
        /// added to the resulting and expects the string that is to be added eventually. This
        /// enables the caller to add prefixes or postfixes to the elements before they are added 
        /// to the resulting string</param>
        /// <param name="separator">Separates the elements, usually a comma</param>
        /// <returns>A string that contains all elements of the list</returns>
        public static string ListAsString(this List<int> list, Func<int, string> buildFunction, string separator = ", ")
        {
            return list.ListAsString<int>(buildFunction, separator);
        }

        /// <summary>
        /// Removes diacritics from a string, for example André becomes Andre
        /// </summary>
        //public static string RemoveDiacritics(this string text)
        //{
        //    var normalizedString = text.Normalize(NormalizationForm.FormD);
        //    var stringBuilder = new StringBuilder(capacity: normalizedString.Length);

        //    for (int i = 0; i < normalizedString.Length; i++)
        //    {
        //        char c = normalizedString[i];
        //        var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
        //        if (unicodeCategory != UnicodeCategory.NonSpacingMark)
        //        {
        //            stringBuilder.Append(c);
        //        }
        //    }

        //    return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        //}

        /// <summary>
        /// Returns [length] characters from the end of a given string
        /// </summary>
        public static string Right(this string value, int length)
        {
            if (value.Length <= length)
                return value;
            return value.Substring(value.Length - length, length);
        }

        /// <summary>
        /// Removes values from a string
        /// </summary>
        /// <param name="value">The original string</param>
        /// <param name="stripValues">These values are being removed</param>
        public static string Strip(this string value, params string[] stripValues)
        {
            foreach (var strip in stripValues)
                value = value.Replace(strip, string.Empty);
            return value;
        }

        /// <summary>
        /// Round a decimal number. 0.5 rounds to 1.0
        /// </summary>
        /// <param name="value"></param>
        /// <param name="decimals">Number of digits after the dot</param>
        public static decimal Round(this decimal value, int decimals) => Math.Round(value, decimals, MidpointRounding.AwayFromZero);

        /// <summary>
        /// Rounds a decimal number to 2 digits after the dot. 0.50 rounds to 1.00
        /// </summary>
        public static decimal RoundMoney(this decimal value) => Round(value, 2);
    }
}
