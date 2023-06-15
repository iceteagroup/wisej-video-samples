using System.Linq;
using TalkCRM.Classes;

namespace DemoApp.Classes
{
    /// <summary>
    /// This static class defines the salutations (Mr., Mrs., Company) to be used with the Lookup class
    /// </summary>
    public static class Salutations
    {
        // Indexes to the Items array
        public const int SALUTATION_Male = 0;
        public const int SALUTATION_Female = 1;
        public const int SALUTATION_Company = 2;

        /// <summary>
        /// The actual lookup data
        /// </summary>
        public static readonly LookupPair[] Items = new LookupPair[]
        {
            new LookupPair{ Id=SALUTATION_Male, Text = "Mr." },
            new LookupPair{ Id=SALUTATION_Female, Text = "Mrs." },
            new LookupPair{ Id=SALUTATION_Company, Text = "Company" },
        };

        /// <summary>
        /// Returns Items but without SALUTATION_Company
        /// </summary>
        public static LookupPair[] ItemsWithoutCompany => Items.Where(x => x.Id != SALUTATION_Company).ToArray();
    }
}
