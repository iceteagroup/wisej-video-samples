using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Schematix.Classes
{
    internal class Options
    {
        public string TableAlias { get; set; }
        public bool ColumnAlias { get; set; } = false;
        public bool FullyQualifiedColumns { get; set; } = false;
        public int ColumnsPerLine { get; set; } = 5;
        public string NameSpace { get; set; }
        public bool SupportINotifyPropertyChanged { get; set; } = true;

        public string DatabaseName { get; set; }

        public static Options Load()
        {
            var options = new Options();

            var filename = Path.ChangeExtension(Application.ExecutablePath, "config");
            if (File.Exists(filename))
            {
                var json = File.ReadAllText(filename);
                options = JsonConvert.DeserializeObject<Options>(json);
            }

            return options;
        }

        public void Save()
        {
            var filename = Path.ChangeExtension(Application.ExecutablePath, "config");
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(filename, json);
        }
    }
}
