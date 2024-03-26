using MySql.Data.MySqlClient;
using System;
using System.Text;
using static Production_Controll.Product;

namespace Production_Controll
{
    public class ProductService
    {
        private DatabaseManager dbManager;
        private ModificationService modificationService;
        private CityService cityService;

        public ProductService()
        {
            dbManager = new DatabaseManager();
            modificationService = new ModificationService();
            cityService = new CityService();
        }

        public bool TransferProductToCity(long productId, int quantity, long targetCityId)
        {

            Product product = GetProductById(productId);

            if (product == null)
            {
                Console.WriteLine($"Product with ID {productId} not found.");
                return false;
            }

            if (!cityService.CityExists(targetCityId))
            {
                Console.WriteLine($"City with ID {targetCityId} not found.");
                return false;
            }

            if (product.cityId == targetCityId)
            {
                Console.WriteLine("The product is already in the target city.");
                return false;
            }

            if (product.quantity < quantity)
            {
                Console.WriteLine("Insufficient quantity for transfer.");
                return false;
            }

            if (!UpdateQuantity(product.id, Modification.Operation.Substraction, quantity))
            {
                Console.WriteLine("First product quantity update failed.");
                return false;
            }

            if (DoesProductExistInCity(product.name, targetCityId))
            {
                Product existingProduct = GetProductByNameAndCityId(product.name, targetCityId);

                if (!UpdateQuantity(existingProduct.id, Modification.Operation.Addition, quantity))
                {
                    Console.WriteLine("Second product found but quantity update failed.");
                    return false;
                }
            }
            else
            {
                Product newProduct = new Product(
                    product.name,
                    product.productGroupId,
                    product.price,
                    product.expirationDate,
                    targetCityId);

                newProduct = SaveProduct(newProduct);

                if (newProduct == null)
                {
                    Console.WriteLine("Second product save in target city failed.");
                    return false;
                }

                if (!UpdateQuantity(newProduct.id, Modification.Operation.Addition, quantity))
                {
                    Console.WriteLine("Second product created but quantity update failed.");
                    return false;
                }
            }

            return true;


        }

        public long GetLastInsertedId()
        {
            string query = "SELECT LAST_INSERT_ID() FROM production_control.products;";
            var (resultList, rowsAffected) = dbManager.ExecuteQuery(query);

            if (resultList == null)
            {
                Console.WriteLine("Error retrieving last inserted ID: Database connectivity issue.");
                return -1;
            }

            if (!rowsAffected || !resultList[0].ContainsKey("LAST_INSERT_ID()"))
            {
                Console.WriteLine("Error retrieving last inserted ID: No rows affected or key not found.");
                return -1;
            }

            return Convert.ToInt64(resultList[0]["LAST_INSERT_ID()"]);
        }

        public bool UpdateProduct(long productId, string newName = null, decimal? newPrice = null, DateTime? newExpirationDate = null)
        {
            StringBuilder queryBuilder = new StringBuilder("UPDATE products SET ");

            if (!string.IsNullOrEmpty(newName))
            {
                queryBuilder.Append($"name = '{newName}', ");
            }

            if (newPrice.HasValue)
            {
                queryBuilder.Append($"price = {newPrice}, ");
            }

            if (newExpirationDate.HasValue)
            {
                queryBuilder.Append($"expiration_date = '{newExpirationDate:yyyy-MM-dd HH:mm:ss}', ");
            }

            queryBuilder.Remove(queryBuilder.Length - 2, 2);

            queryBuilder.Append($" WHERE id = {productId};");

            string query = queryBuilder.ToString();

            if (!dbManager.ExecuteNonQuery(query))
            {
                Console.WriteLine("Error updating product fields");
                return false;
            }

            return true;

        }

        public Product SaveProduct(Product product)
        {
            string query = $"INSERT INTO products (name, product_group_id, price, expiration_date, city_id, quantity, last_modified) " +
                           $"VALUES ('{product.name}','{product.productGroupId}','{product.price}','{product.expirationDate:yyyy-MM-dd HH:mm:ss}', '{product.cityId}', {product.quantity}, '{product.lastModified:yyyy-MM-dd HH:mm:ss}');";

            if (!dbManager.ExecuteNonQuery(query))
            {
                Console.WriteLine("Product save failed");
                return null;
            }

            product.id = GetLastInsertedId();

            return product;


        }
        public bool DoesProductExistInCity(string productName, long cityId)
        {
            string query = $"SELECT COUNT(*) AS count FROM products WHERE name = '{productName}' AND city_id = {cityId};";
            var (resultList, rowsAffected) = dbManager.ExecuteQuery(query);

            if (resultList == null)
            {
                Console.WriteLine("Database connectivity error or table not found");
                return false; // Handle database connectivity or table not found issue
            }

            if (!rowsAffected || !resultList[0].ContainsKey("count"))
            {
                Console.WriteLine("No rows are effected or 'count' key not found");
                return false; // Return false if no rows are affected or "count" key not found
            }

            return Convert.ToInt32(resultList[0]["count"]) > 0;
        }

        public Product GetProductById(long id)
        {
            string query = $"SELECT * FROM products WHERE id = {id};";
            var (resultList, rowsAffected) = dbManager.ExecuteQuery(query);

            if (resultList == null)
            {
                Console.WriteLine("Database connectivity error or table not found");
                return null; // Handle database connectivity or table not found issue
            }

            if (!rowsAffected)
            {
                Console.WriteLine("No rows are effected or 'count' key not found");
                return null; // Return null if no rows are affected
            }

            return resultList.Count > 0 ? ExtractProductFromResult(resultList[0]) : null;
        }

        private Product ExtractProductFromResult(Dictionary<string, object> result)
        {
            return new Product(
                Convert.ToInt64(result["id"]),
                Convert.ToString(result["name"]).ToString(),
                Convert.ToInt64(result["product_group_id"]),
                Convert.ToDecimal(result["price"]),
                Convert.ToDateTime(result["expiration_date"]),
                Convert.ToInt32(result["city_id"]),
                Convert.ToInt32(result["quantity"]),
                Convert.ToDateTime(result["last_modified"])
            );
        }

        public bool UpdateQuantity(long productId, Modification.Operation operation, int quantity)
        {
            int amount = quantity;
            Product product = GetProductById(productId);
            long cityId = product.cityId;

            switch (operation)
            {
                case Modification.Operation.Addition:
                    string addQuery = $"UPDATE products SET quantity = quantity + {amount}, last_modified = NOW() WHERE id = {productId};";
                    if (dbManager.ExecuteNonQuery(addQuery))
                    {
                        return cityService.UpdateAvailableSpace(cityId, operation, quantity);
                    }
                    break;

                case Modification.Operation.Substraction:
                    string subtractQuery = $"UPDATE products SET quantity = GREATEST(quantity - {amount}, 0), last_modified = NOW() WHERE id = {productId};";
                    if (dbManager.ExecuteNonQuery(subtractQuery))
                    {
                        return cityService.UpdateAvailableSpace(cityId, operation, quantity);
                    }
                    break;

                default:
                    // Handle unsupported operation
                    break;
            }

            return false; // Default return if no query executed successfully
        }

        public bool CheckQuantityForSubtraction(long id, int amount)
        {
            string query = $"SELECT quantity >= {amount} AS result FROM products WHERE id = {id};";
            var (resultList, rowsAffected) = dbManager.ExecuteQuery(query);

            if (resultList == null)
            {
                Console.WriteLine("Database connectivity error or table not found");
                return false; // Handle database connectivity or table not found issue
            }

            if (!rowsAffected || !resultList[0].ContainsKey("result"))
            {
                Console.WriteLine("No rows are effected or 'count' key not found");
                return false; // Return false if no rows are affected or "result" key not found
            }

            return Convert.ToBoolean(resultList[0]["result"]);
        }


        public bool DeleteProduct(long id)
        {
            if (!modificationService.DeleteModificationsByProductId(id))
            {
                Console.WriteLine($"Error deleting modifications related to product with ID {id}.");
                return false;
            }

            string query = $"DELETE FROM products WHERE id = {id};";
            Product product = GetProductById(id);
            long cityId = product.cityId;

            if (dbManager.ExecuteNonQuery(query))
            {
                cityService.UpdateAvailableSpace(cityId, Modification.Operation.DELETE, product.quantity); //akac rollback unda
                return true;
            }

            return false;

        }

        public List<Product> GetAllProducts()
        {
            string query = "SELECT * FROM products;";
            var (resultList, rowsAffected) = dbManager.ExecuteQuery(query);

            if (resultList == null)
            {
                return null; // Handle database connectivity or table not found issue
            }

            if (!rowsAffected)
            {
                return new List<Product>(); // Return empty list if table is empty
            }

            List<Product> products = new List<Product>();

            foreach (var row in resultList)
            {
                Product product = ExtractProductFromResult(row);
                if (product != null)
                {
                    products.Add(product);
                }
            }

            return products;
        }


        public Product GetProductByNameAndCityId(string productName, long cityId)
        {
            string query = $"SELECT * FROM products WHERE name = '{productName}' AND city_id = {cityId};";
            var (resultList, rowsAffected) = dbManager.ExecuteQuery(query);

            if (resultList == null)
            {
                Console.WriteLine("Database connectivity error or table not found");
                return null; // Handle database connectivity or table not found issue
            }

            if (!rowsAffected)
            {
                Console.WriteLine("No rows are effected or 'count' key not found");
                return null; // Return null if no rows are affected
            }

            return resultList.Count > 0 ? ExtractProductFromResult(resultList[0]) : null;
        }

        public bool DeleteAllProductsInCity(long cityId)
        {
            List<Product> products = GetAllProductsByCityId(cityId);

            if (products != null)
            {
                foreach (var product in products)
                {
                    if (!DeleteProduct(product.id))
                    {
                        Console.WriteLine($"Failed to delete product with ID {product.id} from the city.");
                        return false;
                    }
                }

                return true;
            }
            else
            {
                Console.WriteLine("No products found in the city.");
                return true;
            }

        }

        public List<Product> getProductsByGroupId(long groupId)
        {
            string query = $"SELECT * FROM products WHERE product_group_id = {groupId};";
            var (resultList, rowsAffected) = dbManager.ExecuteQuery(query);

            if (resultList == null)
            {
                Console.WriteLine("Database connectivity error or table not found");
                return null; // Handle database connectivity or table not found issue
            }

            if (!rowsAffected)
            {
                Console.WriteLine("No rows are effected or 'count' key not found");
                return new List<Product>(); // Return empty list if no rows are affected
            }

            List<Product> products = new List<Product>();

            foreach (var row in resultList)
            {
                Product product = ExtractProductFromResult(row);
                if (product != null)
                {
                    products.Add(product);
                }
            }

            return products;
        }

        public List<Product> FindByProductGroupId(long productGroupId)
        {
            // Construct the SQL query to select products by product group ID
            string query = $"SELECT * FROM products WHERE product_group_id = {productGroupId};";

            // Execute the query using the DatabaseManager
            var (resultList, rowsAffected) = dbManager.ExecuteQuery(query);

            // Check if the query execution was successful and results were obtained
            if (resultList == null || !rowsAffected)
            {
                // Handle the case where no results were obtained or there was a database connectivity issue
                Console.WriteLine("No products found for the specified product group ID.");
                return new List<Product>();
            }

            // Convert the results into Product objects
            List<Product> products = new List<Product>();
            foreach (var row in resultList)
            {
                Product product = ExtractProductFromResult(row);
                if (product != null)
                {
                    products.Add(product);
                }
            }

            return products;
        }


        public List<Product> GetAllProductsByCityId(long cityId)
        {
            string query = $"SELECT * FROM products WHERE city_id = {cityId};";
            var (resultList, rowsAffected) = dbManager.ExecuteQuery(query);

            if (resultList == null)
            {
                Console.WriteLine("Database connectivity error or table not found");
                return null; // Handle database connectivity or table not found issue
            }

            if (!rowsAffected)
            {
                Console.WriteLine("No rows are effected or 'count' key not found");
                return new List<Product>(); // Return empty list if table is empty
            }

            List<Product> products = new List<Product>();

            foreach (var row in resultList)
            {
                Product product = ExtractProductFromResult(row);
                if (product != null)
                {
                    products.Add(product);
                }
            }

            return products;
        }

    }
}