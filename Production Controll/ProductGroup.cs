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

        public string PackagingType { get; set; }

        public ProductGroup()
        {
        }

        public ProductGroup(string name, decimal liter, string packagingType)
        {
            Name = name;
            Liter = liter;
            PackagingType = packagingType;
        }

        public ProductGroup(long id, string name, decimal liter, string packagingType)
        {
            Id = id;
            Name = name;
            Liter = liter;
            PackagingType = packagingType;
        }
    }
}
