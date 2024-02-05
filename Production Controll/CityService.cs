using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Production_Controll.Product;

namespace Production_Controll
{
    public class CityService
    {
        private readonly DatabaseManager dbManager;

        public CityService()
        {
            dbManager = new DatabaseManager();
        }

        public bool UpdateAvailableSpace(long cityId, Modification modification)
        {
            City city = FindById(cityId);

            if (city == null)
            {
                Console.WriteLine($"City with ID {cityId} not found.");
                return false;
            }

            int updatedSpace = city.availableSpace;

            if (modification.operation == Modification.Operation.Addition)
            {
                if (modification.quantity > updatedSpace)
                {
                    Console.WriteLine("Error: Subtraction quantity exceeds available capacity.");
                    return false;
                }
                updatedSpace -= modification.quantity;
            }
            else if (modification.operation == Modification.Operation.Substraction)
            {
                updatedSpace += modification.quantity;
            }
            else if (modification.operation == Modification.Operation.DELETE)
            {
                updatedSpace += modification.quantity;
            }

            if (updatedSpace < 0)
            {
                Console.WriteLine("Error: Updated capacity is negative.");
                return false;
            }

            string updateQuery = $"UPDATE city SET available_space = {updatedSpace} WHERE id = {cityId};";

            using (var transaction = dbManager.BeginTransaction())
            {
                try
                {
                    if (dbManager.ExecuteNonQuery(updateQuery))
                    {
                        transaction.Commit();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating available space: {ex.Message}");
                }

                // Rollback if the update fails
                transaction.Rollback();
                return false;
            }
        }


        public City FindById(long cityId)
        {
            string query = $"SELECT * FROM city WHERE id = {cityId};";
            var result = dbManager.ExecuteQuery(query);

            if (result.Count > 0 &&
                result[0].TryGetValue("id", out var idObj) &&
                result[0].TryGetValue("name", out var nameObj) &&
                result[0].TryGetValue("capacity", out var capacityObj) &&
                result[0].TryGetValue("available_space", out var available_spaceObj))
            {
                long id = Convert.ToInt64(idObj);
                string name = nameObj.ToString();
                int capacity = Convert.ToInt32(capacityObj);
                int available_space = Convert.ToInt32(available_spaceObj);


                return new City(id, name, capacity,available_space);
            }
            
            return null; 
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

        public bool IsCityNameUnique(string cityName)
        {
            string query = $"SELECT COUNT(*) AS count FROM city WHERE name = '{cityName}';";
            var result = dbManager.ExecuteQuery(query);

            return result.Count > 0 && result[0].ContainsKey("count") && Convert.ToInt32(result[0]["count"]) == 0;
        }


        public City SaveCity(City city)
        {
            string query = $"INSERT INTO city (name, capacity, available_space) " +
                           $"VALUES ('{city.name}', '{city.capacity}', '{city.capacity}');";

            using (var transaction = dbManager.BeginTransaction())
            {
                try
                {
                    if (dbManager.ExecuteNonQuery(query))
                    {
                        city.id = GetLastInsertedId();
                        transaction.Commit();
                        return city;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving city: {ex.Message}");
                }

                // Rollback if the save operation fails
                transaction.Rollback();
                return null;
            }
        }


        public List<City> GetAllCities()
        {
            string query = "SELECT * FROM city;";
            var result = dbManager.ExecuteQuery(query);

            List<City> cities = new List<City>();

            foreach (var row in result)
            {
                if (row.TryGetValue("id", out var idObj) &&
                    row.TryGetValue("name", out var nameObj) &&
                    row.TryGetValue("capacity", out var capacityObj) &&
                    row.TryGetValue("available_space",out var available_spaceObj))
                {
                    long id = Convert.ToInt64(idObj);
                    string name = nameObj.ToString();
                    int capacity = Convert.ToInt32(capacityObj);
                    int available_space = Convert.ToInt32(available_spaceObj);

                    City city = new City(id, name, capacity,available_space);
                    cities.Add(city);
                }
            }

            return cities;
        }


    }
}
