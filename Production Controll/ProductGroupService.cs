using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Production_Controll
{
    internal class ProductGroupService
    {
        private DatabaseManager dbManager;

        public ProductGroupService()
        {
            dbManager = new DatabaseManager();
        }

        public long GetLastInsertedId()
        {
            string query = "SELECT LAST_INSERT_ID() FROM production_control.product_group;";
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


        public long SaveProductGroup(ProductGroup productGroup)
        {
            try
            {
                string query = $"INSERT INTO production_control.product_group (name, liter) " +
                               $"VALUES ('{productGroup.Name}', {productGroup.Liter})";

                if (dbManager.ExecuteNonQuery(query))
                {
                    return GetLastInsertedId();
                }

                return -1; // Error occurred while saving
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving product group: {ex.Message}");
                return -1; // Error occurred while saving
            }
        }

        public List<ProductGroup> GetAllProductGroups()
        {
            try
            {
                string query = "SELECT * FROM production_control.product_group;";
                var (resultList, rowsAffected) = dbManager.ExecuteQuery(query);

                if (resultList == null)
                {
                    // Handle database connectivity or table not found issue
                    Console.WriteLine("Error retrieving product groups: Database connectivity issue.");
                    return null;
                }

                List<ProductGroup> productGroups = new List<ProductGroup>();

                foreach (var result in resultList)
                {
                    if (result.TryGetValue("id", out var idObj) &&
                        result.TryGetValue("name", out var nameObj) &&
                        result.TryGetValue("liter", out var literObj))
                    {
                        long id = Convert.ToInt64(idObj);
                        string name = Convert.ToString(nameObj);
                        decimal liter = Convert.ToDecimal(literObj);

                        ProductGroup productGroup = new ProductGroup(id, name, liter);
                        productGroups.Add(productGroup);
                    }
                }

                return productGroups;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving product groups: {ex.Message}");
                return null;
            }
        }

        public bool DeleteProductGroup(long productGroupId)
        {
            try
            {
                string query = $"DELETE FROM production_control.product_group WHERE id = {productGroupId};";

                return dbManager.ExecuteNonQuery(query);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting product group: {ex.Message}");
                return false;
            }
        }





    }
}
