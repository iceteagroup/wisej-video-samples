using Wisej.Web;

namespace WisejLib
{
    /// <summary>
    /// Help dealing with lookup combo boxes, based on the LookupPair class
    /// </summary>
    public static class Lookup
    {
        /// <summary>
        /// Fills the ComboBox.DataSource property with a list of LookupPair items and defines ValueMembr and DisplayMember
        /// </summary>
        /// <param name="comboBox">The ComboBox to fill</param>
        /// <param name="items">The items to point the ComboBox.DataSource property to</param>
        /// <param name="selectedIndex">The initially display item index or -1 to not display any item (default = -1)</param>
        public static void Populate(ComboBox comboBox, LookupPair[] items, int selectedIndex = -1)
        {
            comboBox.DataSource = items;
            comboBox.ValueMember = nameof(LookupPair.Id);
            comboBox.DisplayMember = nameof(LookupPair.Text);
            if (selectedIndex >= items.Length || selectedIndex < 0)
                selectedIndex = -1;
            comboBox.SelectedIndex = selectedIndex;
        }

        /// <summary>
        /// Fills the ComboBox.DataSource property with a list of LookupPair items and defines ValueMembr and DisplayMember
        /// </summary>
        /// <param name="comboBoxColumn">The column to fill</param>
        /// <param name="items">The items to point the ComboBox.DataSource property to</param>
        public static void Populate(DataGridViewComboBoxColumn comboBoxColumn, LookupPair[] items)
        {
            comboBoxColumn.DataSource = items;
            comboBoxColumn.ValueMember = nameof(LookupPair.Id);
            comboBoxColumn.DisplayMember = nameof(LookupPair.Text);
        }

        /// <summary>
        /// Fills the ComboBox.Items property with a list of string items
        /// </summary>
        /// <param name="comboBox">The ComboBox to fill</param>
        /// <param name="items">The strings fill the ComboBox.Items property with</param>
        public static void Populate(ComboBox comboBox, string[] items)
        {
            comboBox.Items.Clear();
            comboBox.Items.AddRange(items);
        }
    }
}