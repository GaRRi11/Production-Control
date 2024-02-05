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
            using (var transaction = dbManager.BeginTransaction())
            {
                try
                {
                    string query = $"INSERT INTO products (name, city_id, quantity, last_modified) " +
                                   $"VALUES ('{product.name}', '{product.cityId}', {product.quantity}, '{product.lastModified:yyyy-MM-dd HH:mm:ss}');";

                    if (dbManager.ExecuteNonQuery(query))
                    {
                        product.id = GetLastInsertedId();
                        Modification modification = new Modification(product.id, Modification.Operation.CREATE, 0, DateTime.Now);
                        if (RecordModification(modification))
                        {
                            transaction.Commit();
                            return product;
                        }
                    }

                    return null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving product: {ex.Message}");
                    transaction.Rollback();
                    return null;
                }
            }
        }
        public bool DoesProductExistInCity(string productName, long cityId)
        {
            string query = $"SELECT COUNT(*) AS count FROM products WHERE name = '{productName}' AND city_id = {cityId};";
            var result = dbManager.ExecuteQuery(query);

            return result.Count > 0 && result[0].ContainsKey(key: "count") && Convert.ToInt32(result[0]["count"]) > 0;
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
                Convert.ToString(result["name"]).ToString(),
                Convert.ToInt32(result["city_id"]),
                Convert.ToInt32(result["quantity"]),
                Convert.ToDateTime(result["last_modified"])
            );
        }


        public bool UpdateQuantity(Modification modification)
        {
            int amount = modification.quantity;
            long productId = modification.productId;
            Product product = GetProductById(productId);
            long cityId = product.cityId;

            bool success = false;

            using (var transaction = dbManager.BeginTransaction())
            {
                try
                {
                    if (modification.operation == Modification.Operation.Addition)
                    {
                        string query = $"UPDATE products SET quantity = quantity + {amount}, last_modified = NOW() WHERE id = {productId};";
                        if (dbManager.ExecuteNonQuery(query))
                        {
                            if (cityService.UpdateAvailableSpace(cityId, modification))
                            {
                                if (RecordModification(modification))
                                {
                                    success = true;
                                }
                                else
                                {
                                    // Rollback if RecordModification fails
                                    transaction.Rollback();
                                    return false;
                                }
                            }
                            else
                            {
                                // Rollback if UpdateAvailableSpace fails
                                transaction.Rollback();
                                return false;
                            }
                        }
                    }
                    else if (modification.operation == Modification.Operation.Substraction)
                    {
                        string query = $"UPDATE products SET quantity = GREATEST(quantity - {amount}, 0), last_modified = NOW() WHERE id = {productId};";
                        if (dbManager.ExecuteNonQuery(query))
                        {
                            if (cityService.UpdateAvailableSpace(cityId, modification))
                            {
                                if (RecordModification(modification))
                                {
                                    success = true;
                                }
                                else
                                {
                                    // Rollback if RecordModification fails
                                    transaction.Rollback();
                                    return false;
                                }
                            }
                            else
                            {
                                // Rollback if UpdateAvailableSpace fails
                                transaction.Rollback();
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
                    transaction.Commit();
                    dbManager.CloseConnection();
                    return true;
                }
                else
                {
                    // Rollback for any unexpected failure
                    transaction.Rollback();
                    dbManager.CloseConnection();
                    return false;
                }
            }
        }



        public bool CheckQuantityForSubtraction(long id, int amount)
        {
            string query = $"SELECT quantity >= {amount} AS result FROM products WHERE id = {id};";
            var result = dbManager.ExecuteQuery(query);
            return result.Count > 0 && result[0].ContainsKey("result") && Convert.ToBoolean(result[0]["result"]);
        }

        public bool DeleteProduct(long id)
        {
            using (var transaction = dbManager.BeginTransaction())
            {
                try
                {
                    string query = $"DELETE FROM products WHERE id = {id};";
                    Product product = GetProductById(id);
                    long cityId = product.cityId;

                    if (dbManager.ExecuteNonQuery(query))
                    {
                        Modification modification = new Modification(id, Modification.Operation.DELETE, product.quantity, DateTime.Now);
                        cityService.UpdateAvailableSpace(cityId, modification); //akac rollback unda
                        if (RecordModification(modification))
                        {
                            transaction.Commit();
                            return true;
                        }
                    }

                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting product: {ex.Message}");
                    transaction.Rollback();
                    return false;
                }
            }
        }

        private bool RecordModification(Modification modification)
        {
             if(modificationService.SaveModification(modification) == null)
            {
                return false;
            }
             return true;
        }

        public List<Product> GetAllProducts()
        {
            string query = "SELECT * FROM products;";
            var result = dbManager.ExecuteQuery(query);

            List<Product> products = new List<Product>();

            foreach (var row in result)
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


        public List<Product> GetAllProductsByCityId(long cityId)
        {
            string query = $"SELECT * FROM products WHERE city_id = {cityId};";
            var result = dbManager.ExecuteQuery(query);

            List<Product> products = new List<Product>();

            foreach (var row in result)
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