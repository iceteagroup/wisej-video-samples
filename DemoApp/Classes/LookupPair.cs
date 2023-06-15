namespace TalkCRM.Classes
{
    /// <summary>
    /// LookupPair ist the standard data structure to be used with the Lookup class.
    /// When setting a ComboBoxes DataSource property, it expects Id as the ValueMember 
    /// and Text as the DisplayMember
    /// </summary>
    public class LookupPair
    {
        public int Id { get; set; }
        public string Text { get; set; }
    }
}