using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Production_Controll
{
    internal class ProductDTO
    {
        public Product createProduct(
        String name, String version, double liters,
        string city, int quantity)
        {
            Product.City city1 = new Product.City();    
            if (city == "Tbilisi")
            {
                city1 = Product.City.Tbilisi;
            }
            if (city == "Natakhtari")
            {
                city1 = Product.City.Natakhtari;
            }
            return new Product(name, version, liters, city1, quantity);
        }
    }   
}
