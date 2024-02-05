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
            List<Dictionary<string, object>> result = dbManager.ExecuteQuery(query);

            if (result.Count > 0 && result[0].ContainsKey("LAST_INSERT_ID()"))
            {
                return Convert.ToInt64(result[0]["LAST_INSERT_ID()"]);
            }

            // Handle the case where the ID retrieval fails (optional)
            Console.WriteLine("Error retrieving last inserted ID.");
            return -1; // or throw an exception
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


        public List<Modification> GetAllModificationsByProductId(long productId)
        {
            string query = $"SELECT * FROM modifications WHERE product_id = {productId};";
            var result = dbManager.ExecuteQuery(query);

            List<Modification> modifications = new List<Modification>();

            foreach (var row in result)
            {
                if (row.TryGetValue("operation_type", out var operationTypeObj) &&
                    row.TryGetValue("quantity_changed", out var quantityObj) &&
                    row.TryGetValue("date", out var dateObj))
                {
                    Modification.Operation operation = (Modification.Operation)Enum.Parse(typeof(Modification.Operation), operationTypeObj.ToString());
                    int quantity = Convert.ToInt32(quantityObj);
                    DateTime date = Convert.ToDateTime(dateObj);

                    Modification modification = new Modification(productId, operation, quantity, date);
                    modifications.Add(modification);
                }
            }

            return modifications;
        }
    }
}