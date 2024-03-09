using MySql.Data.MySqlClient;
using System;
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

        public bool TransferProductToCity(long productId, int quantity, long targetCityId)
        {
            try
            {
                // Retrieve the product details
                Product product = GetProductById(productId);
                if (product == null)
                {
                    Console.WriteLine($"Product with ID {productId} not found.");
                    return false;
                }

                // Check if the target city exists
                if (!cityService.CityExists(targetCityId))
                {
                    Console.WriteLine($"City with ID {targetCityId} not found.");
                    return false;
                }

                // Check if the product is already in the target city
                if (product.cityId == targetCityId)
                {
                    Console.WriteLine("The product is already in the target city.");
                    return false;
                }

                // Check if the product quantity is sufficient for transfer
                if (product.quantity < quantity)
                {
                    Console.WriteLine("Insufficient quantity for transfer.");
                    return false;
                }

                // Begin transaction
               // using (var transaction = dbManager.BeginTransaction())
                {
                    // Deduct quantity from current city
                    if (!UpdateQuantity(product.id, Modification.Operation.Substraction, quantity))
                    {
                        // Rollback transaction if quantity deduction fails
                       // transaction.Rollback();
                        return false;
                    }

                    // Check if the product already exists in the target city
                    if (DoesProductExistInCity(product.name, targetCityId))
                    {
                        Product existingProduct = GetProductByNameAndCityId(product.name,targetCityId);
                        // Add quantity to existing product in target city
                        if (!UpdateQuantity(existingProduct.id, Modification.Operation.Addition, quantity))
                        {
                            // Rollback transaction if quantity addition fails
                          //  transaction.Rollback();
                            return false;
                        }
                    }
                    else
                    {
                        // Create new product in target city
                        Product newProduct = new Product(product.name, targetCityId);
                        newProduct = SaveProduct(newProduct);
                        if (newProduct == null)
                        {
                            // Rollback transaction if new product creation fails
                            //transaction.Rollback();
                            return false;
                        }
                        // Add quantity to new product in target city
                        if (!UpdateQuantity(newProduct.id, Modification.Operation.Addition, quantity))
                        {
                            // Rollback transaction if quantity addition fails
                            //transaction.Rollback();
                            return false;
                        }
                    }

                    // Commit transaction
                   // transaction.Commit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error transferring product to city: {ex.Message}");
                return false;
            }
        }


        public Product GetProductByName(string productName)
        {
            string query = $"SELECT * FROM products WHERE name = '{productName}';";
            var (resultList, rowsAffected) = dbManager.ExecuteQuery(query);

            if (resultList == null)
            {
                return null; // Handle database connectivity or table not found issue
            }

            if (!rowsAffected)
            {
                return null; // Return null if no rows are affected
            }

            return resultList.Count > 0 ? ExtractProductFromResult(resultList[0]) : null;
        }



        public long GetLastInsertedId()
        {
            string query = "SELECT LAST_INSERT_ID() FROM production_control.product;";
            var (resultList, rowsAffected) = dbManager.ExecuteQuery(query);

            if (resultList == null)
            {
                // Handle database connectivity or table not found issue
                Console.WriteLine("Error retrieving last inserted ID: Database connectivity issue.");
                return -1;
            }

            if (!rowsAffected || !resultList[0].ContainsKey("LAST_INSERT_ID()"))
            {
                // Handle case where no rows are affected or "LAST_INSERT_ID()" key not found
                Console.WriteLine("Error retrieving last inserted ID: No rows affected or key not found.");
                return -1;
            }

            return Convert.ToInt64(resultList[0]["LAST_INSERT_ID()"]);
        }


        public Product SaveProduct(Product product)
        {
           // using (var transaction = dbManager.BeginTransaction())
            {
                try
                {
                    string query = $"INSERT INTO products (name, city_id, quantity, last_modified) " +
                                   $"VALUES ('{product.name}', '{product.cityId}', {product.quantity}, '{product.lastModified:yyyy-MM-dd HH:mm:ss}');";

                    if (dbManager.ExecuteNonQuery(query))
                    {
                        product.id = GetLastInsertedId();
                        
                            //transaction.Commit();
                            return product;
                        
                    }

                    return null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving product: {ex.Message}");
                    //transaction.Rollback();
                    return null;
                }
            }
        }
        public bool DoesProductExistInCity(string productName, long cityId)
        {
            string query = $"SELECT COUNT(*) AS count FROM products WHERE name = '{productName}' AND city_id = {cityId};";
            var (resultList, rowsAffected) = dbManager.ExecuteQuery(query);

            if (resultList == null)
            {
                return false; // Handle database connectivity or table not found issue
            }

            if (!rowsAffected || !resultList[0].ContainsKey("count"))
            {
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
                return null; // Handle database connectivity or table not found issue
            }

            if (!rowsAffected)
            {
                return null; // Return null if no rows are affected
            }

            return resultList.Count > 0 ? ExtractProductFromResult(resultList[0]) : null;
        }


        private Product ExtractProductFromResult(Dictionary<string, object> result)
        {
            return new Product(
                Convert.ToInt64(result["id"]),
                Convert.ToString(result["name"]).ToString(),
                Convert.ToInt32(result["city_id"]),
                Convert.ToInt32(result["quantity"]),
                Convert.ToDateTime(result["last_modified"])
            );
        }


        public bool UpdateQuantity(long productId, Modification.Operation operation,int quantity)
        {
            int amount = quantity;
            Product product = GetProductById(productId);
            long cityId = product.cityId;

            bool success = false;

           // using (var transaction = dbManager.BeginTransaction())
            {
                try
                {
                    if (operation == Modification.Operation.Addition)
                    {
                        string query = $"UPDATE products SET quantity = quantity + {amount}, last_modified = NOW() WHERE id = {productId};";
                        if (dbManager.ExecuteNonQuery(query))
                        {
                            if (cityService.UpdateAvailableSpace(cityId, operation, quantity))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                            
                        }
                    }
                    else if (operation == Modification.Operation.Substraction)
                    {
                        string query = $"UPDATE products SET quantity = GREATEST(quantity - {amount}, 0), last_modified = NOW() WHERE id = {productId};";
                        if (dbManager.ExecuteNonQuery(query))
                        {
                            if (cityService.UpdateAvailableSpace(cityId, operation, quantity))
                            {
                                return true;
                            }
                            else
                            {
                                // Rollback if UpdateAvailableSpace fails
                               // transaction.Rollback();
                                return false;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating quantity: {ex.Message}");
                }

                if (success)
                {
                    //transaction.Commit();
                    //dbManager.CloseConnection();
                    return true;
                }
                else
                {
                    // Rollback for any unexpected failure
                   // transaction.Rollback();
                   // dbManager.CloseConnection();
                    return false;
                }
            }
        }



        public bool CheckQuantityForSubtraction(long id, int amount)
        {
            string query = $"SELECT quantity >= {amount} AS result FROM products WHERE id = {id};";
            var (resultList, rowsAffected) = dbManager.ExecuteQuery(query);

            if (resultList == null)
            {
                return false; // Handle database connectivity or table not found issue
            }

            if (!rowsAffected || !resultList[0].ContainsKey("result"))
            {
                return false; // Return false if no rows are affected or "result" key not found
            }

            return Convert.ToBoolean(resultList[0]["result"]);
        }


        public bool DeleteProduct(long id)
        {
            // using (var transaction = dbManager.BeginTransaction())
            {
                try
                {
                    // First, delete related modifications
                    if (!modificationService.DeleteModificationsByProductId(id))
                    {
                        // Handle error deleting modifications
                        Console.WriteLine($"Error deleting modifications related to product with ID {id}.");
                        return false;
                    }

                    // Then, delete the product
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
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting product: {ex.Message}");
                    //transaction.Rollback();
                    return false;
                }
            }
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
                if (row.TryGetValue("id", out var idObj) &&
                    row.TryGetValue("name", out var nameObj) &&
                    row.TryGetValue("city_id", out var cityIdObj) &&
                    row.TryGetValue("quantity", out var quantityObj) &&
                    row.TryGetValue("last_modified", out var lastModifiedObj))
                {
                    long id = Convert.ToInt64(idObj);
                    string name = Convert.ToString(nameObj);
                    int cityId = Convert.ToInt32(cityIdObj);
                    int quantity = Convert.ToInt32(quantityObj);
                    DateTime lastModified = Convert.ToDateTime(lastModifiedObj);

                    Product product = new Product(id, name, cityId, quantity, lastModified);
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
                return null; // Handle database connectivity or table not found issue
            }

            if (!rowsAffected)
            {
                return null; // Return null if no rows are affected
            }

            return resultList.Count > 0 ? ExtractProductFromResult(resultList[0]) : null;
        }

        public bool DeleteAllProductsInCity(long cityId)
        {
            try
            {
                // Get all products in the given city
                List<Product> products = GetAllProductsByCityId(cityId);

                if (products != null)
                {
                    foreach (var product in products)
                    {
                        // Delete each product using the DeleteProduct method
                        if (!DeleteProduct(product.id))
                        {
                            // If any product deletion fails, return false
                            Console.WriteLine($"Failed to delete product with ID {product.id} from the city.");
                            return false;
                        }
                    }

                    // If all products are successfully deleted, return true
                    return true;
                }
                else
                {
                    // If no products found in the city, return true
                    Console.WriteLine("No products found in the city.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the deletion process
                Console.WriteLine($"Error deleting products from the city: {ex.Message}");
                return false;
            }
        }






        public List<Product> GetAllProductsByCityId(long cityId)
        {
            string query = $"SELECT * FROM products WHERE city_id = {cityId};";
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
                if (row.TryGetValue("id", out var idObj) &&
                    row.TryGetValue("name", out var nameObj) &&
                    row.TryGetValue("quantity", out var quantityObj) &&
                    row.TryGetValue("last_modified", out var lastModifiedObj))
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