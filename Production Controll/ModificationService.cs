using System;
using System.Collections.Generic;

namespace Production_Controll
{
    public class ModificationService
    {
        private readonly DatabaseManager dbManager;

        public ModificationService()
        {
            dbManager = new DatabaseManager();
        }

        public long GetLastInsertedId()
        {
            string query = "SELECT LAST_INSERT_ID();";
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


        public Modification SaveModification(Modification modification)
        {
            string operationType = modification.operation.ToString();
            string formattedDate = modification.date.ToString("yyyy-MM-dd HH:mm:ss");

            string query = $"INSERT INTO modifications (product_id, operation_type, quantity_changed, date) " +
                           $"VALUES ({modification.productId}, '{operationType}', {modification.quantity}, '{formattedDate}');";
            if (dbManager.ExecuteNonQuery(query))
            {
                modification.id = GetLastInsertedId();
                return modification;    
            }
            return null;
        }

        public bool DeleteModificationsByProductId(long productId)
        {
            string query = $"DELETE FROM modifications WHERE product_id = {productId};";
            return dbManager.ExecuteNonQuery(query);
        }



        public List<Modification> GetAllModificationsByProductId(long productId)
        {
            string query = $"SELECT * FROM modifications WHERE product_id = {productId};";
            var (resultList, rowsAffected) = dbManager.ExecuteQuery(query);

            if (resultList == null)
            {
                // Handle database connectivity or table not found issue
                Console.WriteLine("Error querying modifications table: Database connectivity issue.");
                return null;
            }

            if (!rowsAffected)
            {
                // Return an empty list if no rows are affected
                return new List<Modification>();
            }

            List<Modification> modifications = new List<Modification>();

            foreach (var row in resultList)
            {
                if (row.TryGetValue("id", out var idObj) &&
                    row.TryGetValue("operation_type", out var operationTypeObj) &&
                    row.TryGetValue("source_city_id", out var sourceCityIdObj) &&
                    row.TryGetValue("target_city_id", out var targetCityIdObj) &&
                    row.TryGetValue("quantity_changed", out var quantityObj) &&
                    row.TryGetValue("date", out var dateObj))
                {
                    long id = Convert.ToInt64(idObj);
                    Modification.Operation operation = (Modification.Operation)Enum.Parse(typeof(Modification.Operation), operationTypeObj.ToString());
                    long sourceCityId = Convert.ToInt64(sourceCityIdObj);
                    long targetCityId = Convert.ToInt64(targetCityIdObj);
                    int quantity = Convert.ToInt32(quantityObj);
                    DateTime date = Convert.ToDateTime(dateObj);

                    Modification modification = new Modification(id,productId, operation,sourceCityId,targetCityId, quantity, date);
                    modifications.Add(modification);
                }
            }

            return modifications;
        }

    }
}