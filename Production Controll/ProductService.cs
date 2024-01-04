using System;
using System.Collections.Generic;
using System.Linq;

namespace Production_Controll
{
    public class ProductService
    {
        private static List<Product> Products = new List<Product>(); // Simulating storage

        public Product CreateProduct(Product product)
        {
            Products.Add(product);
            return product;
        }

        public DateTime getLastModifiedDate(long productId)
        {
            return GetProductById(productId).LastModified;
        }

        public int getQuantityById(long productId)
        {
            return GetProductById(productId).quantity;
        }

        public Product GetProductById(long id)
        {
            foreach (var product in Products)
            {
                if (product.id == id)
                {
                    return product;
                }
            }
            return null;
        }

        public string getProductNameById(long id)
        {
            return GetProductById(id).name;
        }

        public void AddQuantity(long id, int amount)
        {
            var product = GetProductById(id);
            if (product != null && amount > 0)
            {
                product.AddQuantity(amount);
            }
        }

        public void SubtractQuantity(long id, int amount)
        {
            var product = GetProductById(id);
            if (product != null && amount > 0 && amount <= product.quantity)
            {
                product.SubtractQuantity(amount);
            }
        }

        public bool checkQuantityForSubstraction(long id,int amount)
        {
            var product = GetProductById(id);
            if (product.quantity >= amount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool DeleteProduct(long id)
        {
            var productToRemove = GetProductById(id);
            if (productToRemove != null)
            {
                Products.Remove(productToRemove);
                return true;
            }
            return false;
        }
    }
}