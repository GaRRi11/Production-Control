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
        public City city { get; set; }
        public int quantity { get; set; }
        public DateTime LastModified { get; set; }

        public static long lastId = 0;
        public List<Modification> Modifications { get; set; }

        public enum City
        {
            TBILISI,
            KUTAISI
        }

        private long GenerateId()
        {
            return ++lastId; 
        }

        public Product(string name, City city)
        {
            this.id = GenerateId();
            this.name = name;
            this.city = city;
            this.quantity = 0;
            RecordModification(Modification.Operation.Add, 0);
        }

        public List<Product> Products = new List<Product>();
        public void AddProduct(Product product)
        {
            Products.Add(product);
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

        public void AddQuantity(int amount)
        {
            if (amount > 0) // Ensure we're adding a positive quantity
            {
                quantity += amount; 
                RecordModification(Modification.Operation.Addition, amount);
            }
            else {
                MessageBox.Show("Type valid number");
            }
        }

        // Method to subtract quantity
        public void SubtractQuantity(int amount)
        {
            if (amount > 0 && amount <= quantity) // Ensure valid amount to subtract
            {
                quantity -= amount;
                RecordModification(Modification.Operation.Substraction, amount);
            }
            else {
                MessageBox.Show("Type valid number");
            }
        }

        private void RecordModification(Modification.Operation operationType, int quantityChanged)
        {
            if (Modifications == null)
            {
                Modifications = new List<Modification>();
            }

            LastModified = DateTime.Now;

            Modification modification = new Modification(this.id,operationType,quantityChanged,DateTime.Now);

            Modifications.Add(modification);
            
        }





        //  public void RemoveProduct(Product product) { }



    }
}
