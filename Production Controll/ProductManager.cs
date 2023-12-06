using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Production_Controll
{
    internal class ProductManager
    {
        List<Product> products = new List<Product>();

        public ProductManager()
        {
             products.Add(
                 new Product(1,"Coca Cola","Zero",0.5,)
        }

        public List<Product> getProducts()
        {
            return products;
        }

        public List<Product> getTbilisiProducts()
        {

            return products.Where(p => p.city == Product.City.Tbilisi).ToList();
        }



        public List<Product> getNatakhtariProducts()
        {
            return products.Where(p => p.city == Product.City.Natakhtari).ToList();
        }

        public void add(Product product)
        {
            products.Add(product);
        }

 

    }
}
