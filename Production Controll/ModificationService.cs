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

        public Modification SaveModification(Modification modification)
        {
            string operationType = modification.operation.ToString();
            string formattedDate = modification.date.ToString("yyyy-MM-dd HH:mm:ss");

            string query = $"INSERT INTO modifications (id, productId, operationType, quantityChanged, date) " +
                           $"VALUES ({modification.id}, {modification.productId}, '{operationType}', {modification.quantity}, '{formattedDate}');";

            dbManager.ExecuteNonQuery(query);
            return modification;
        }


        public List<Modification> GetAllModificationsById(long productId)
        {
            string query = $"SELECT * FROM modifications WHERE productId = {productId};";
            var result = dbManager.ExecuteQuery(query);

            List<Modification> modifications = new List<Modification>();

            foreach (var row in result)
            {
                if (row.TryGetValue("operationType", out var operationTypeObj) &&
                    row.TryGetValue("quantityChanged", out var quantityObj) &&
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