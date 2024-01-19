using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Production_Controll
{
    public class City
    {
        public long id { get; set; }
        public string name { get; set; }
        public int capacity { get; set; }

        public City(string name, int capacity)
        {
            this.name = name;
            this.capacity = capacity;
        }
    }
}
