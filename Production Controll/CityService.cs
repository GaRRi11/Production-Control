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

        public int UpdateAvailableSpace(long cityId, Modification modification)
        {
            City city = FindById(cityId);

            if (city == null)
            {
                // Handle the case where the city is not found
                Console.WriteLine($"City with ID {cityId} not found.");
                return -1; // or throw an exception
            }

            int updatedSpace = city.availableSpace;

            if (modification.operation == Modification.Operation.Addition)
            {
                if (modification.quantity > updatedSpace)
                {
                    // Handle the case where the subtraction is not possible (optional)
                    Console.WriteLine("Error: Subtraction quantity exceeds available capacity.");
                    return -1; // or throw an exception
                }
                updatedSpace -= modification.quantity;
            }
            else if (modification.operation == Modification.Operation.Substraction)
            {
                
                updatedSpace += modification.quantity;

            }
            else if (modification.operation == Modification.Operation.DELETE) {
                updatedSpace += modification.quantity;
            }

            if (updatedSpace < 0)
            {
                // Handle the case where the updated capacity becomes negative (optional)
                Console.WriteLine("Error: Updated capacity is negative.");
                return -1; // or throw an exception
            }

            string updateQuery = $"UPDATE city SET available_space = {updatedSpace} WHERE id = {cityId};";
            dbManager.ExecuteNonQuery(updateQuery);

            return updatedSpace;
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

            // Handle the case where the city with the specified ID is not found
            Console.WriteLine($"City with ID {cityId} not found.");
            return null; // or throw an exception
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

            int rowsAffected = dbManager.ExecuteNonQuery(query);

            if (rowsAffected > 0)
            {
                // Insert was successful, update the city ID
                city.id = GetLastInsertedId();
                return city;
            }

            // Insert failed
            return null;
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
