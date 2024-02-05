using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

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
            string createCityTableQuery = @"
        CREATE TABLE IF NOT EXISTS production_control.city (
        id BIGINT NOT NULL AUTO_INCREMENT,
        name VARCHAR(255) NOT NULL UNIQUE,
        capacity INT NOT NULL,
        available_space INT NOT NULL,
        PRIMARY KEY (id));";

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

            ExecuteNonQuery(createCityTableQuery);
            ExecuteNonQuery(createProductTableQuery);
            ExecuteNonQuery(createModificationTableQuery);
            CloseConnection();
        }



        public IDbTransaction BeginTransaction()
        {
            if (OpenConnection())
            {
                return connection.BeginTransaction();
            }

            // Handle the case where the connection is not open.
            Console.WriteLine("Error: Database connection is not open.");
            return null;
        }

        public bool OpenConnection()
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error opening database connection: {ex.Message}");
                return false;
            }
        }

        public void CloseConnection()
        {
            try
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error closing database connection: {ex.Message}");
            }
        }

        public bool ExecuteNonQuery(string query, IDictionary<string, object> parameters = null)
        {
            try
            {
                OpenConnection();
                using (var cmd = new MySqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
                        }
                    }

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing non-query: {ex.Message}");
                return false;
            }
        }

        public List<Dictionary<string, object>> ExecuteQuery(string query)
        {
            var resultList = new List<Dictionary<string, object>>();
            try
            {
                if (OpenConnection())  // Move OpenConnection call outside the MySqlCommand block
                {
                    using (var cmd = new MySqlCommand(query, connection))
                    {
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing query: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }

            return resultList;
        }

    }
}
