//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Production_Controll
//{
//    internal class ProductManager
//    {
//        List<ProductType> productType = new List<ProductType>();
//        List<Location> location = new List<Location>();
//        List<Liters> liters = new List<Liters>();




//        public ProductManager()
//        {
//            liters.Add(new Liters(1,0.5));
//            location.Add(new Location(1, "Tbilisi"));
//            productType.Add(new ProductType(1, "koka kola Classic", 1));
//        }

//        public List<Product> getProducts()
//        {
//            return products;
//        }

//        public List<Product> getTbilisiProducts()
//        {

//            return products.Where(p => p.city == Product.City.Tbilisi).ToList();
//        }



//        public List<Product> getNatakhtariProducts()
//        {
//            return products.Where(p => p.city == Product.City.Natakhtari).ToList();
//        }

//        public void add(int nameId,int litersId,int cityId,int quantity)
//        {

//        }

 

//    }
//}
