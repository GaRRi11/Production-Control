using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;


namespace Production_Controll
{
    public class Product
    {
        public long id { get; set; }
        public string name { get; set; }

        public int cityId { get; set; }
        public int quantity { get; set; }
        public DateTime lastModified { get; set; }

        public static long lastId = 0;

        public enum City
        {
            TBILISI,
            KUTAISI
        }

        private long GenerateId()
        {
            return ++lastId; 
        }

        public Product(string name, int cityId)
        {
            this.id = GenerateId();
            this.name = name;
            this.cityId = cityId;
            this.quantity = 0;
            this.lastModified = DateTime.Now;
        }

        public Product(long id, string name, int cityId, int quantity, DateTime lastModified)
        {
            this.id = id;
            this.name = name;
            this.cityId = cityId;
            this.quantity = quantity;
            this.lastModified = lastModified;
        }
    }
}
