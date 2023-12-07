using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Production_Controll
{
    internal class ComboBoxItem
    {
        public string DisplayName { get; set; }
        public int ID { get; set; }

        public ComboBoxItem(string name, int id)
        {
            DisplayName = name;
            ID = id;
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }

}
