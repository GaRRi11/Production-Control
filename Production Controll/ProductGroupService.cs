using System;
using System.Collections.Generic;

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




        public List<ProductGroup> GetAllProductGroups()
        {
            try
            {
                string query = "SELECT * FROM production_control.product_group;";
                var (resultList, rowsAffected) = dbManager.ExecuteQuery(query);

                if (resultList == null)
                {
                    Console.WriteLine("Error retrieving product groups: Database connectivity issue.");
                    return null;
                }

                List<ProductGroup> productGroups = new List<ProductGroup>();

                foreach (var result in resultList)
                {
                    productGroups.Add(ExtractProductGroupFromResult(result));
                }

                return productGroups;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving product groups: {ex.Message}");
                return null;
            }
        }

        public bool DeleteProductGroupById(long productGroupId)
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

        public string GetNameById(long productGroupId)
        {
            try
            {
                string query = $"SELECT CONCAT(name, ' - ', packaging_type) AS groupName FROM production_control.product_group WHERE id = {productGroupId};";
                var (resultList, rowsAffected) = dbManager.ExecuteQuery(query);

                if (resultList == null || resultList.Count == 0)
                {
                    Console.WriteLine($"No product group found with ID: {productGroupId}");
                    return null;
                }

                return Convert.ToString(resultList[0]["groupName"]);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving product group name and packaging type: {ex.Message}");
                return null;
            }
        }


        public ProductGroup FindById(long productGroupId)
        {
            try
            {
                string query = $"SELECT * FROM production_control.product_group WHERE id = {productGroupId};";
                var (resultList, rowsAffected) = dbManager.ExecuteQuery(query);

                if (resultList == null || resultList.Count == 0)
                {
                    Console.WriteLine($"No product group found with ID: {productGroupId}");
                    return null;
                }

                return ExtractProductGroupFromResult(resultList[0]);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving product group: {ex.Message}");
                return null;
            }
        }

        public ProductGroup SaveProductGroup(ProductGroup productGroup)
        {
            try
            {
                string query = $"INSERT INTO production_control.product_group (name, liter, packaging_type) " +
                               $"VALUES ('{productGroup.Name}', {productGroup.Liter}, '{productGroup.PackagingType}')";

                if (dbManager.ExecuteNonQuery(query))
                {
                    long lastInsertedId = GetLastInsertedId();
                    return FindById(lastInsertedId);
                }

                Console.WriteLine("Product group save failed.");
                return null; // Error occurred while saving
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving product group: {ex.Message}");
                return null; // Error occurred while saving
            }
        }


        private ProductGroup ExtractProductGroupFromResult(Dictionary<string, object> result)
        {
            if (result.TryGetValue("id", out var idObj) &&
                result.TryGetValue("name", out var nameObj) &&
                result.TryGetValue("liter", out var literObj) &&
                result.TryGetValue("packaging_type", out var packagingTypeObj))
            {
                long id = Convert.ToInt64(idObj);
                string name = Convert.ToString(nameObj);
                decimal liter = Convert.ToDecimal(literObj);
                string packagingType = Convert.ToString(packagingTypeObj);

                return new ProductGroup(id, name, liter, packagingType);
            }

            Console.WriteLine("Error parsing product group data from result.");
            return null;
        }
    }
}
