using System;
using static Production_Controll.Product;

namespace Production_Controll
{
    public class ProductService
    {
        private readonly DatabaseManager dbManager;
        private readonly ModificationService modificationService;

        public ProductService()
        {
            dbManager = new DatabaseManager();
            modificationService = new ModificationService();
        }

        public Product CreateProduct(Product product)
        {
            string query = $"INSERT INTO products (id, name, city, quantity, lastModified) " +
                           $"VALUES ('{product.id}', '{product.name}', '{product.city}', {product.quantity}, '{product.lastModified:yyyy-MM-dd HH:mm:ss}');";
            dbManager.ExecuteNonQuery(query);
            RecordModification(product.id, Modification.Operation.CREATE, 0);
            return product;
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
                // Assuming the city column is a string type in the database
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
                (Product.City)Enum.Parse(typeof(Product.City), result["city"].ToString()),
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
            dbManager.ExecuteNonQuery(query);
            // Handle checks and return accordingly
            return false;
        }

        private void RecordModification(long id, Modification.Operation operationType, int quantityChanged)
        {
            string updateQuery = $"UPDATE products SET LastModified = NOW() WHERE id = {id};";
            dbManager.ExecuteNonQuery(updateQuery);

            Modification modification = new Modification(id, operationType, quantityChanged, DateTime.Now);
            modificationService.SaveModification(modification);
        }
    }
}