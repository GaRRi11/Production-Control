using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Production_Controll
{
    public class ProductGroup
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal Liter { get; set; }

        public ProductGroup()
        {
        }

        public ProductGroup(string name, decimal liter)
        {
            Name = name;
            Liter = liter;
        }

        public ProductGroup(long id, string name, decimal liter)
        {
            Id = id;
            Name = name;
            Liter = liter;
        }
    }
}
