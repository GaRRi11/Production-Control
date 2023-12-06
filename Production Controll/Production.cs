using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Production_Controll
{
    internal class Production
    {
        public long ProductionID { get; set; }
        public long ProductTypeID { get; set; }
        public long LocationID { get; set; }
        public int Quantity { get; set; }
    }
}
