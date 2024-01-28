﻿using System;
using static Production_Controll.Product;

namespace Production_Controll
{
    public class ProductService
    {
        private readonly DatabaseManager dbManager;
        private readonly ModificationService modificationService;
        private CityService cityService = new CityService();

        public ProductService()
        {
            dbManager = new DatabaseManager();
            modificationService = new ModificationService();
        }

        public long GetLastInsertedId()
        {
            string query = "SELECT LAST_INSERT_ID();";
            List<Dictionary<string, object>> result = dbManager.ExecuteQuery(query);

            if (result.Count > 0 && result[0].ContainsKey("LAST_INSERT_ID()"))
            {
                return Convert.ToInt64(result[0]["LAST_INSERT_ID()"]);
            }

            // Handle the case where the ID retrieval fails (optional)
            Console.WriteLine("Error retrieving last inserted ID.");
            return -1; // or throw an exception
        }

        public Product SaveProduct(Product product)
        {
            
                string query = $"INSERT INTO products (name, cityId, quantity, lastModified) " +
                           $"VALUES ('{product.name}', '{product.cityId}', {product.quantity}, '{product.lastModified:yyyy-MM-dd HH:mm:ss}');";
                dbManager.ExecuteNonQuery(query);
                product.id = GetLastInsertedId();
                RecordModification(product.id, Modification.Operation.CREATE, 0);
                return product;
           
                throw new InvalidOperationException("City name must be unique.");
                return null;
            
        }

        public Product getProductByName(string productName)
        {
            string query = $"SELECT * FROM products WHERE name = '{productName}';";
            var result = dbManager.ExecuteQuery(query);

            if (result.Count > 0)
            {
                return ExtractProductFromResult(result[0]);
            }

            return null;
        }


        public bool DoesProductExistInCity(string productName, int cityId)
        {
            string query = $"SELECT COUNT(*) AS count FROM products WHERE name = '{productName}' AND cityId = {cityId};";
            var result = dbManager.ExecuteQuery(query);

            return result.Count > 0 && result[0].ContainsKey("count") && Convert.ToInt32(result[0]["count"]) > 0;
        }


        public DateTime GetLastModifiedDate(long productId)
        {
            string query = $"SELECT lastModified FROM products WHERE id = {productId};";
            var result = dbManager.ExecuteQuery(query);

            if (result.Count > 0 && result[0].ContainsKey("lastModified"))
            {
                return Convert.ToDateTime(result[0]["lastModified"]);
            }

            return DateTime.MinValue;
        }

        public int GetQuantityById(long productId)
        {
            string query = $"SELECT quantity FROM products WHERE id = {productId};";
            var result = dbManager.ExecuteQuery(query);

            return result.Count > 0 && result[0].ContainsKey("quantity") ? Convert.ToInt32(result[0]["quantity"]) : -1;
        }

        public string GetCityById(long productId)
        {
            string query = $"SELECT city FROM products WHERE id = {productId}";
            var result = dbManager.ExecuteQuery(query);

            if (result.Count > 0 && result[0].ContainsKey("city"))
            {
                return result[0]["city"].ToString();
            }

            return null;
        }

        public Product GetProductById(long id)
        {
            string query = $"SELECT * FROM products WHERE id = {id};";
            var result = dbManager.ExecuteQuery(query);

            return result.Count > 0 ? ExtractProductFromResult(result[0]) : null;
        }

        private Product ExtractProductFromResult(Dictionary<string, object> result)
        {
            return new Product(
                Convert.ToInt64(result["id"]),
                result["name"].ToString(),
                Convert.ToInt32(result["cityId"]),
                Convert.ToInt32(result["quantity"]),
                Convert.ToDateTime(result["lastModified"])
            );
        }

        public string GetProductNameById(long id)
        {
            var product = GetProductById(id);
            return product != null ? product.name : string.Empty;
        }

        public void AddQuantity(long id, int amount)
        {
            string query = $"UPDATE products SET quantity = quantity + {amount} WHERE id = {id};";
            dbManager.ExecuteNonQuery(query);
            cityService.UpdateAvailableSpace(city, modification);
            RecordModification(id, Modification.Operation.Addition, amount);
        }

        public void SubtractQuantity(long id, int amount)
        {
            string query = $"UPDATE products SET quantity = GREATEST(quantity - {amount}, 0) WHERE id = {id};";
            dbManager.ExecuteNonQuery(query);
            RecordModification(id, Modification.Operation.Substraction, amount);
        }

        public bool CheckQuantityForSubtraction(long id, int amount)
        {
            string query = $"SELECT quantity >= {amount} AS result FROM products WHERE id = {id};";
            var result = dbManager.ExecuteQuery(query);
            return result.Count > 0 && result[0].ContainsKey("result") && Convert.ToBoolean(result[0]["result"]);
        }

        public bool DeleteProduct(long id)
        {
            string query = $"DELETE FROM products WHERE id = {id};";
            Product product = GetProductById(id);
            if (product != null)
            {
                dbManager.ExecuteNonQuery(query);
                return true;
            }
            return false;
        }

        private void RecordModification(long id, Modification.Operation operationType, int quantityChanged)
        {
            string updateQuery = $"UPDATE products SET LastModified = NOW() WHERE id = {id};";
            dbManager.ExecuteNonQuery(updateQuery);

            Modification modification = new Modification(id, operationType, quantityChanged, DateTime.Now);
            modificationService.SaveModification(modification);
        }

        public List<Product> GetAllProductsByCityId(long cityId)
        {
            string query = $"SELECT * FROM products WHERE cityId = {cityId};";
            var result = dbManager.ExecuteQuery(query);

            List<Product> products = new List<Product>();

            foreach (var row in result)
            {
                if (row.TryGetValue("id", out var idObj) &&
                    row.TryGetValue("name", out var nameObj) &&
                    row.TryGetValue("quantity", out var quantityObj) &&
                    row.TryGetValue("lastModified", out var lastModifiedObj))
                {
                    long id = Convert.ToInt64(idObj);
                    string name = nameObj.ToString();
                    int quantity = Convert.ToInt32(quantityObj);
                    DateTime lastModified = Convert.ToDateTime(lastModifiedObj);

                    Product product = new Product(id, name, cityId, quantity, lastModified);
                    products.Add(product);
                }
            }

            return products;
        }


    }
}