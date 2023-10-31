using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace WisejLib
{
    public enum WildcardMode { Left, Right, Both }

    public static class Extensions
    {
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

        public static string DigitsOnly(this string value)
        {
            return Clean(value, "0123456789");
        }

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

        public static bool SameText(this string value1, string value2)
        {
            if (value1 is null || value2 is null)
                return false;
            return value1.Equals(value2, StringComparison.CurrentCultureIgnoreCase);
        }

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

        public static bool IsValidUrl(this string value) => Uri.TryCreate(value, UriKind.Absolute, out _);

        public static string LeftTillStop(this string value, string stop = "\n")
        {
            int p = value.IndexOf(stop);
            return p < 0 ? value : value.Substring(0, p);
        }

        public static string LimitLength(this string value, int maxLength, bool appendDots = false)
        {
            if (value.Length <= maxLength)
                return value;
            return (appendDots && maxLength >= 3) ? value.Substring(0, maxLength - 3) + "..." : value.Substring(0, maxLength);
        }

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

        public static string ListAsString(this List<string> list, Func<string, string> buildFunction, string separator = ", ")
        {
            return list.ListAsString<string>(buildFunction, separator);
        }

        public static string ListAsString(this List<int> list, Func<int, string> buildFunction, string separator = ", ")
        {
            return list.ListAsString<int>(buildFunction, separator);
        }

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

        public static string Right(this string value, int length)
        {
            if (value.Length <= length)
                return value;
            return value.Substring(value.Length - length, length);
        }

        public static string Strip(this string value, params string[] stripValues)
        {
            foreach (var strip in stripValues)
                value = value.Replace(strip, string.Empty);
            return value;
        }

        public static decimal Round(this decimal value, int decimals) => Math.Round(value, decimals, MidpointRounding.AwayFromZero);

        public static decimal RoundMoney(this decimal value) => Round(value, 2);
    }
}
