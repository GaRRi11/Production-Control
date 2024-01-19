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

        public City SaveCity(City city)
        {
            string query = $"INSERT INTO city (id, name, capacity) " +
                           $"VALUES ('{city.id}', '{city.name}', '{city.capacity}');";
            dbManager.ExecuteNonQuery(query);
            return city;
        }
    }
}
