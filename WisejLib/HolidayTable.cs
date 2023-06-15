using System;
using System.Collections.Generic;

namespace WisejLib
{
    /// <summary>
    /// Public enum that contains all German States
    /// </summary>
    [Flags]
    public enum GermanStates
    {
        /// <summary>
        /// All German states
        /// </summary>
        All = 0,
        /// <summary>A German state</summary>
        BadenWürttemberg = 1 << 0,
        /// <summary>A German state</summary>
        Bayern = 1 << 1,
        /// <summary>A German state</summary>
        Berlin = 1 << 2,
        /// <summary>A German state</summary>
        Brandenburg = 1 << 3,
        /// <summary>A German state</summary>
        Bremen = 1 << 4,
        /// <summary>A German state</summary>
        Hamburg = 1 << 5,
        /// <summary>A German state</summary>
        Hessen = 1 << 6,
        /// <summary>A German state</summary>
        MecklenburgVorpommern = 1 << 7,
        /// <summary>A German state</summary>
        Niedersachsen = 1 << 8,
        /// <summary>A German state</summary>
        NordrheinWestfalen = 1 << 9,
        /// <summary>A German state</summary>
        RheinlandPfalz = 1 << 10,
        /// <summary>A German state</summary>
        Saarland = 1 << 11,
        /// <summary>A German state</summary>
        Sachsen = 1 << 12,
        /// <summary>A German state</summary>
        SachsenAnhalt = 1 << 13,
        /// <summary>A German state</summary>
        SchleswigHolstein = 1 << 14,
        /// <summary>A German state</summary>
        Thüringen = 1 << 15,
    }

    /// <summary>
    /// This class handles official German holidays.
    /// Some German states have different holidays than others
    /// </summary>
    public class HolidayTable
    {
        /// <summary>
        /// If you use a HolidayTableCache which can hold multiple years, use the HolidayTableCache to retrieve instances of HolidayTable
        /// </summary>
        public HolidayTable(int year, bool HoleEveAndNewYearsEveAreHolidays = true)
        {
            Initialize(year, HoleEveAndNewYearsEveAreHolidays);
        }

        /// <summary>
        /// The year that this HolidayTable is initiaized with
        /// </summary>
        public int Year { get; private set; }


        /// <summary>
        /// List of holiday definitions
        /// </summary>
        public List<HolidayItem> Items { get; } = new List<HolidayItem>();

        /// <summary>
        /// Retrieves the holiday name for a given date or null if not found
        /// </summary>
        public string this[DateTime date] => GetHolidayName(date);

        /// <summary>
        /// Retrieves the date for a given holiday name or null if not found
        /// </summary>
        public DateTime? this[string name] => GetHolidayDate(name);

        /// <summary>
        /// Returns true if a date is a holiday
        /// </summary>
        public bool IsHoliday(DateTime date)
        {
            return !string.IsNullOrEmpty(GetHolidayName(date));
        }


        /// <summary>
        /// Returns true if a date is not on the weekend and is not a holiday
        /// </summary>
        public bool IsWorkday(DateTime date)
        {
            DayOfWeek dow = date.DayOfWeek;
            return dow != DayOfWeek.Saturday && dow != DayOfWeek.Sunday && !IsHoliday(date);
        }


        /// <summary>
        /// Get the date associated with the holiday name
        /// </summary>
        /// <param name="name">Name of the holiday</param>
        /// <returns>Returns the holiday date or null if the passed name is not a holiday</returns>
        private DateTime? GetHolidayDate(string name)
        {
            foreach (HolidayItem item in Items)
                if (item.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    return item.Date;
            return null;
        }

        /// <summary>
        /// Get the holiday name associated with the date
        /// </summary>
        /// <param name="date">Date of the holiday</param>
        /// <returns>Returns the holiday name or null if the passed date is not a holiday</returns>
        private string GetHolidayName(DateTime date)
        {
            date = date.Date;
            foreach (HolidayItem item in Items)
                if (item.Date == date)
                    return item.Name;
            return string.Empty;
        }

        /// <summary>
        /// Initialzes the table with all holidays in the given year.
        /// THis is called from the constructor
        /// </summary>
        /// <param name="year">Must pass the year for which the holidays have to be calculated</param>
        /// <param name="HoleEveAndNewYearsEveAreHolidays">Defines whether xmas an new year are holidays or not</param>
        private void Initialize(int year, bool HoleEveAndNewYearsEveAreHolidays)
        {
            var minYear = 1900;
            var maxYear = 2200;
            Year = year;
            if (year < minYear || year > maxYear)
                throw new ArgumentOutOfRangeException($"Cannot calculate holidays, year must be betwen {minYear} and {maxYear}");

            Items.Clear();
            DateTime easter = DateUtils.CalcEasterSunday(year);

            // valid in all German states
            Items.Add(new HolidayItem(new DateTime(year, 1, 1), "Neujahr"));
            Items.Add(new HolidayItem(new DateTime(year, 5, 1), "Maifeiertag"));
            Items.Add(new HolidayItem(new DateTime(year, 10, 1), "Tag der Einheit"));
            Items.Add(new HolidayItem(new DateTime(year, 12, 25), "1. Weihnachtstag"));
            Items.Add(new HolidayItem(new DateTime(year, 12, 26), "2. Weihnachtstag"));
            Items.Add(new HolidayItem(easter.AddDays(-2), "Karfreitag"));
            Items.Add(new HolidayItem(easter.AddDays(1), "Ostermontag"));
            Items.Add(new HolidayItem(easter.AddDays(39), "Himmelfahrt"));
            Items.Add(new HolidayItem(easter.AddDays(50), "Pfingstmontag"));

            if (HoleEveAndNewYearsEveAreHolidays)
            {
                Items.Add(new HolidayItem(new DateTime(year, 12, 24), "Heiligabend"));
                Items.Add(new HolidayItem(new DateTime(year, 12, 31), "Sylvester"));
            }

            // valid only in certain German states
            Items.Add(new HolidayItem(new DateTime(year, 1, 6), "Heilige Drei Könige",
                GermanStates.BadenWürttemberg | GermanStates.Bayern | GermanStates.SachsenAnhalt));
            Items.Add(new HolidayItem(new DateTime(year, 3, 8), "Internationaler Frauentag",
                GermanStates.Berlin));
            Items.Add(new HolidayItem(easter, "Ostersonntag",
                GermanStates.Brandenburg));
            Items.Add(new HolidayItem(easter.AddDays(49), "Pfingstsonntag",
                GermanStates.Brandenburg));
            Items.Add(new HolidayItem(new DateTime(year, 6, 16), "Fronleichnam",
                GermanStates.BadenWürttemberg | GermanStates.Bayern | GermanStates.Hessen |
                GermanStates.NordrheinWestfalen | GermanStates.RheinlandPfalz | GermanStates.Saarland));
            Items.Add(new HolidayItem(new DateTime(year, 8, 15), "Mariä Himmelfahrt",
                GermanStates.Saarland));
            Items.Add(new HolidayItem(new DateTime(year, 9, 20), "Weltkindertag",
                GermanStates.Thüringen));
            Items.Add(new HolidayItem(new DateTime(year, 11, 1), "Allerheiligen",
                GermanStates.BadenWürttemberg | GermanStates.Bayern | GermanStates.NordrheinWestfalen |
                GermanStates.RheinlandPfalz | GermanStates.Saarland));
            Items.Add(new HolidayItem(new DateTime(year, 11, 16), "Buß- und Bettag",
                GermanStates.Sachsen));

            // special: in 2017 the law changed for Reformationstag
            if (Year >= 2017)
                Items.Add(new HolidayItem(new DateTime(year, 10, 31), "Reformationstag",
                    GermanStates.Brandenburg | GermanStates.Bremen | GermanStates.Hamburg | GermanStates.MecklenburgVorpommern |
                    GermanStates.Niedersachsen | GermanStates.Sachsen | GermanStates.SachsenAnhalt | GermanStates.SchleswigHolstein |
                    GermanStates.Thüringen));

        }

    }

    /// <summary>
    /// Represents 1 holiday of the HolidayTable
    /// </summary>
    public class HolidayItem
    {
        /// <summary>
        /// Creates a HolidayItem
        /// </summary>
        /// <param name="date">The date of the holiday</param>
        /// <param name="name">The german name of the holiday</param>
        /// <param name="states">The German states where these holidays are legal</param>
        public HolidayItem(DateTime date, string name, GermanStates states = GermanStates.All)
        {
            Date = date;
            Name = name;
            States = states;
        }

        /// <summary>
        /// the date of the holiday
        /// </summary>
        public DateTime Date { get; }
        /// <summary>
        /// the name of the holiday
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Flags that define for which German states this is a holiday
        /// </summary>
        public GermanStates States { get; }
        /// <summary>
        /// A list of German states where this is a holiday
        /// </summary>
        public List<string> StatesNames => CollectStateNames();

        private List<string> CollectStateNames()
        {
            var list = new List<string>();

            if (States == GermanStates.All)
                return list;

            foreach (GermanStates x in Enum.GetValues(typeof(GermanStates)))
            {
                if (x != GermanStates.All && States.HasFlag(x))
                    list.Add(Enum.GetName(typeof(GermanStates), x));
            }
            return list;
        }
    }

    /// <summary>
    /// HolidayTable instances retrieved through HolidayTableCache.CreateHolidayTable() are cached
    /// </summary>
    public static class HolidayTableCache
    {
        /// <summary>
        /// The dictionary that stroes all HolidayTables that have been retrieved by calling CreateHolidayTable
        /// </summary>
        public static readonly Dictionary<int, HolidayTable> Pool = new Dictionary<int, HolidayTable>();

        /// <summary>
        /// Fetches a HolidayTable from the cache if it exists. If not, a new one is 
        /// created and stored in the cache
        /// </summary>
        /// <param name="year">The year whcih HolidayTable is desired</param>
        /// <returns>A cached HolidayTable instance</returns>
        public static HolidayTable CreateHolidayTable(int year)
        {
            if (!Pool.ContainsKey(year))
                Pool.Add(year, new HolidayTable(year));
            return Pool[year];
        }
    }
}
