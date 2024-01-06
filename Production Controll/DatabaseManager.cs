using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Production_Controll
{
    public class DatabaseManager
    {
        private const string ConnectionString = "server=localhost;uid=root;pwd=garomysql1852;database=production_control";

        public void InitializeDB()
        {
            string createProductTableQuery = @"
                CREATE TABLE IF NOT EXISTS production_control.products (
                id BIGINT NOT NULL,
                name VARCHAR(255) NOT NULL,
                city VARCHAR(255) NOT NULL,
                quantity INT NOT NULL,
                lastModified DATETIME NOT NULL,
                PRIMARY KEY (id));";

            string createModificationTableQuery = @"
                CREATE TABLE IF NOT EXISTS production_control.Modifications (
                id BIGINT NOT NULL,
                productId BIGINT NOT NULL,
                operationType VARCHAR(255) NOT NULL,
                quantityChanged INT NOT NULL,
                date DATETIME NOT NULL,
                PRIMARY KEY (id),
                FOREIGN KEY (ProductId) REFERENCES production_control.products(id) ON DELETE CASCADE);";

            ExecuteNonQuery(createProductTableQuery);
            ExecuteNonQuery(createModificationTableQuery);
        }

        public void ExecuteNonQuery(string query)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<Dictionary<string, object>> ExecuteQuery(string query)
        {
            var resultList = new List<Dictionary<string, object>>();

            using (var conn = new MySqlConnection(ConnectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var row = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string fieldName = reader.GetName(i);
                            object value = reader.GetValue(i);
                            row[fieldName] = value;
                        }

                        resultList.Add(row);
                    }
                }
            }

            return resultList;
        }

        public void DropTables()
        {
            string dropModificationConstraintQuery = "ALTER TABLE production_control.Modifications DROP FOREIGN KEY modifications_ibfk_1;";
            string dropProductTableQuery = "DROP TABLE IF EXISTS production_control.products;";
            string dropModificationTableQuery = "DROP TABLE IF EXISTS production_control.Modifications;";

            ExecuteNonQuery(dropModificationConstraintQuery);
            ExecuteNonQuery(dropProductTableQuery);
            ExecuteNonQuery(dropModificationTableQuery);
        }
    }
}