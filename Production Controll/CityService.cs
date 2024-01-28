using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Production_Controll
{
    public class CityService
    {
        private readonly DatabaseManager dbManager;

        public CityService()
        {
            dbManager = new DatabaseManager();
        }

        public int UpdateAvailableSpace(City city, Modification modification)
        {
            //wavides bazashi da shecvalos space modifikaciidan aigos mimatebaa tu gamokleba da raodenoba 
        }

        public int GetAvailableSpace(City city)
        {
            //wavides da wamoigos available space columni
            //am metods gamovikenebt capacity label is gansanaxleblad extensionshi ikneba update label da ik 
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

                string query = $"INSERT INTO city (name, capacity) " +
                               $"VALUES ('{city.name}', '{city.capacity}');";
                dbManager.ExecuteNonQuery(query);
                city.id = GetLastInsertedId();
                return city;
            
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
                    row.TryGetValue("capacity", out var capacityObj))
                {
                    long id = Convert.ToInt64(idObj);
                    string name = nameObj.ToString();
                    int capacity = Convert.ToInt32(capacityObj);

                    City city = new City(id, name, capacity);
                    cities.Add(city);
                }
            }

            return cities;
        }


    }
}
