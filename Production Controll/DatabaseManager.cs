using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.Xml;

namespace Production_Controll
{
    public class DatabaseManager
    {
        private const string ConnectionString = "server=localhost;uid=root;pwd=garomysql1852;database=production_control";
        private MySqlConnection connection;

        public DatabaseManager()
        {
            connection = new MySqlConnection(ConnectionString);
        }

        public void InitializeDB()
        {
            string createProductTableQuery = @"
                 CREATE TABLE IF NOT EXISTS production_control.products (
                 id BIGINT NOT NULL AUTO_INCREMENT,
                 name VARCHAR(255) NOT NULL UNIQUE,
                 city_id BIGINT NOT NULL,
                 quantity INT NOT NULL,
                 last_modified DATETIME NOT NULL,
                 PRIMARY KEY (id),
                 FOREIGN KEY (city_id) REFERENCES production_control.city(id) ON DELETE CASCADE);";

            string createModificationTableQuery = @"CREATE TABLE IF NOT EXISTS production_control.Modifications (
    id BIGINT NOT NULL AUTO_INCREMENT,
    product_id BIGINT NOT NULL,
    operation_type VARCHAR(255) NOT NULL,
    quantity_changed INT NOT NULL,
    date DATETIME NOT NULL,
    PRIMARY KEY (id),
    FOREIGN KEY (product_id) REFERENCES production_control.products(id) ON DELETE CASCADE
);";

            string createCityTableQuery = @"
                CREATE TABLE IF NOT EXISTS production_control.city (
                id BIGINT NOT NULL AUTO_INCREMENT,
                name VARCHAR(255) NOT NULL UNIQUE,
                capacity INT NOT NULL,
                available_space INT NOT NULL,
                PRIMARY KEY (id));";

            ExecuteNonQuery(createCityTableQuery);
            ExecuteNonQuery(createProductTableQuery);
            ExecuteNonQuery(createModificationTableQuery);
        }

        public void ExecuteNonQuery(string query)
        {
            try
            {
                using (var cmd = new MySqlCommand(query, connection))
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing non-query: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }

        }

            public List<Dictionary<string, object>> ExecuteQuery(string query)
        {
            var resultList = new List<Dictionary<string, object>>();
            try
            {

                using (var cmd = new MySqlCommand(query, connection))
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

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
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error executing query: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }

            return resultList;
        }

        //public void DropTables()
        //{
        //    string dropModificationConstraintQuery = "ALTER TABLE production_control.Modifications DROP FOREIGN KEY modifications_ibfk_1;";
        //    string dropProductTableQuery = "DROP TABLE IF EXISTS production_control.products;";
        //    string dropModificationTableQuery = "DROP TABLE IF EXISTS production_control.Modifications;";

        //    ExecuteNonQuery(dropModificationConstraintQuery);
        //    ExecuteNonQuery(dropProductTableQuery);
        //    ExecuteNonQuery(dropModificationTableQuery);
        //}
    }
}