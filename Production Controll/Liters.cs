using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Production_Controll
{
    internal class Liters
    {
        public long LitersId {  get; set; }
        public double literCapacity { get; set;}

        public Liters(long litersId, double literCapacity)
        {
            LitersId = litersId;
            this.literCapacity = literCapacity;
        }
    }
}
