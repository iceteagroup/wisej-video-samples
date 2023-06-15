using System;

namespace WisejLib
{
    /// <summary>
    /// Specifies the date range when GetDateRange() is called
    /// </summary>
    public enum DateRangeKind
    {
        /// <summary>A DateRangeKind</summary>
        CurrentDay,
        /// <summary>A DateRangeKind</summary>
        CurrentWeek,
        /// <summary>A DateRangeKind</summary>
        CurrentMonth,
        /// <summary>A DateRangeKind</summary>
        CurrentQuarter,
        /// <summary>A DateRangeKind</summary>
        CurrentYear,
        /// <summary>A DateRangeKind</summary>
        NextDay,
        /// <summary>A DateRangeKind</summary>
        NextWeek,
        /// <summary>A DateRangeKind</summary>
        NextMonth,
        /// <summary>A DateRangeKind</summary>
        NextQuarter,
        /// <summary>A DateRangeKind</summary>
        NextYear,
        /// <summary>A DateRangeKind</summary>
        PreviousDay,
        /// <summary>A DateRangeKind</summary>
        PreviousWeek,
        /// <summary>A DateRangeKind</summary>
        PreviousMonth,
        /// <summary>A DateRangeKind</summary>
        PreviousQuarter,
        /// <summary>A DateRangeKind</summary>
        PreviousYear,
    }

    /// <summary>
    /// A helper class with extensions dealing with dates
    /// </summary>
    public static class DateUtils
    {
        /// <summary>(Extension) returns first date and last date of a given DateRangeKind enum</summary>
        public static void GetDateRange(this DateRangeKind dateRangeKind, out DateTime start, out DateTime end)
        {
            DateTime date;
            switch (dateRangeKind)
            {
                case DateRangeKind.CurrentDay:
                    start = DateTime.Today;
                    end = DateTime.Today;
                    break;
                case DateRangeKind.CurrentWeek:
                    start = DateTime.Today.StartOfTheWeek();
                    end = DateTime.Today.EndOfTheWeek();
                    break;
                case DateRangeKind.CurrentMonth:
                    start = DateTime.Today.StartOfTheMonth();
                    end = DateTime.Today.EndOfTheMonth();
                    break;
                case DateRangeKind.CurrentQuarter:
                    start = DateTime.Today.StartOfTheQuarter();
                    end = DateTime.Today.EndOfTheQuarter();
                    break;
                case DateRangeKind.CurrentYear:
                    start = DateTime.Today.StartOfTheYear();
                    end = DateTime.Today.EndOfTheYear();
                    break;
                case DateRangeKind.NextDay:
                    start = DateTime.Today.AddDays(1);
                    end = start;
                    break;
                case DateRangeKind.NextWeek:
                    date = DateTime.Today.AddDays(7);
                    start = date.StartOfTheWeek();
                    end = date.EndOfTheWeek();
                    break;
                case DateRangeKind.NextMonth:
                    date = DateTime.Today.AddMonths(1);
                    start = date.StartOfTheMonth();
                    end = date.EndOfTheMonth();
                    break;
                case DateRangeKind.NextQuarter:
                    date = DateTime.Today.AddQuarter(1);
                    start = date.StartOfTheQuarter();
                    end = date.EndOfTheQuarter();
                    break;
                case DateRangeKind.NextYear:
                    date = DateTime.Today.AddYears(1);
                    start = date.StartOfTheYear();
                    end = date.EndOfTheYear();
                    break;
                case DateRangeKind.PreviousDay:
                    date = DateTime.Today.AddDays(-1);
                    start = date;
                    end = date;
                    break;
                case DateRangeKind.PreviousWeek:
                    date = DateTime.Today.AddDays(-7);
                    start = date.StartOfTheWeek();
                    end = date.EndOfTheWeek();
                    break;
                case DateRangeKind.PreviousMonth:
                    date = DateTime.Today.AddMonths(-1);
                    start = date.StartOfTheMonth();
                    end = date.EndOfTheMonth();
                    break;
                case DateRangeKind.PreviousQuarter:
                    date = DateTime.Today.AddQuarter(-1);
                    start = date.StartOfTheQuarter();
                    end = date.EndOfTheQuarter();
                    break;
                case DateRangeKind.PreviousYear:
                    date = DateTime.Today.AddYears(-1);
                    start = date.StartOfTheYear();
                    end = date.EndOfTheYear();
                    break;
                default:
                    throw new ArgumentException("Unhandled DateRangeKind in GetDateRange(DateRangeKind dateRangeKind, out DateTime start, out DateTime end)");
            }
        }

        /// <summary>(Extension) Adds a quarter (=3 months) to a date</summary>
        public static DateTime AddQuarter(this DateTime date, int quarters)
        {
            return date.AddMonths(3 * quarters);
        }

        /// <summary>
        /// (Extension) Calculates easter sunday of the year of the DateTime
        /// </summary>
        public static DateTime CalcEasterSunday(this DateTime date)
        {
            return CalcEasterSunday(date.Year);
        }

        /// <summary>
        /// Calculates easter sunday of a given year
        /// </summary>
        public static DateTime CalcEasterSunday(int year = 0)
        {
            if (year == 0)
                year = DateTime.Today.Year;
            int a = year % 19;
            int b = year / 100;
            int c = (b - (b / 4) - ((8 * b + 13) / 25) + (19 * a) + 15) % 30;
            int d = c - (c / 28) * (1 - (c / 28) * (29 / (c + 1)) * ((21 - a) / 11));
            int e = d - ((year + (year / 4) + d + 2 - b + (b / 4)) % 7);
            int month = 3 + ((e + 40) / 44);
            int day = e + 28 - (31 * (month / 4));
            return new DateTime(year, month, day);
        }

        /// <summary>(Extension) returns the last day of the month specified by a given date</summary>
        public static DateTime EndOfTheMonth(this DateTime date)
        {
            return date.AddMonths(1).Date.StartOfTheMonth().AddDays(-1);
        }

        /// <summary>(Extension) returns the last day of the quarter specified by a given date</summary>
        public static DateTime EndOfTheQuarter(this DateTime date)
        {
            DateTime start = date.StartOfTheQuarter();
            return start.AddMonths(3).AddSeconds(-1);
            //return new DateTime(date.Year, (date.Quarter() + 1) * 3 - 2, 1).AddDays(-1);
        }

        /// <summary>(Extension) returns the last day of the week specified by a given date</summary>
        public static DateTime EndOfTheWeek(this DateTime date)
        {
            return date.StartOfTheWeek().AddDays(6);
        }

        /// <summary>(Extension) returns the last day of the year specified by a given date</summary>
        public static DateTime EndOfTheYear(this DateTime date)
        {
            return new DateTime(date.Year, 12, 31);
        }

        /// <summary>(Extension) returns the number of the quarter (1..4) for a given date</summary>
        public static int Quarter(this DateTime date)
        {
            return ((date.Month - 1) / 3) + 1;
        }

        /// <summary>(Extension) returns the the quarter for a given date as a string with the format yyyy-mm</summary>
        public static string QuarterStr(this DateTime date)
        {
            return $"{date.Year}-{Quarter(date):D2}";
        }

        /// <summary>(Extension) returns the first day of the month specified by a given date</summary>
        public static DateTime StartOfTheMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        /// <summary>(Extension) returns the first day of the quarter specified by a given date</summary>
        public static DateTime StartOfTheQuarter(this DateTime date)
        {
            return new DateTime(date.Year, (date.Quarter() - 1) * 3 + 1, 1);
        }

        /// <summary>(Extension) returns the first day of the week specified by a given date</summary>
        public static DateTime StartOfTheWeek(this DateTime date)
        {
            // DayOfWeek: Sun = 0, Sat = 6
            switch ((int)date.DayOfWeek)
            {
                case 0:
                    return date.AddDays(1);
                case 1:
                    return date.AddDays(0);
                case 2:
                    return date.AddDays(-1);
                case 3:
                    return date.AddDays(-2);
                case 4:
                    return date.AddDays(-3);
                case 5:
                    return date.AddDays(-4);
                default:
                    return date.AddDays(-5);
            }
        }

        /// <summary>(Extension) returns the first day of the year specified by a given date</summary>
        public static DateTime StartOfTheYear(this DateTime date)
        {
            return new DateTime(date.Year, 1, 1);
        }

        /// <summary>(Extension) Calculates the number of years between 2 dates</summary>
        public static int YearsBetween(DateTime now, DateTime then)
        {
            if (then > now)
                (then, now) = (now, then);

            int years = now.Year - then.Year;
            if (now.DayOfYear < then.DayOfYear)
                years--;
            return years;
        }
    }
}
