﻿using System;
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

        public bool transferProduct(Modification modification)
        {
            long sourceCityId = modification.SourceCityId;
            long targetCityId = modification.TargetCityId;
            City sourceCity = FindById(modification.SourceCityId);
            City targetCity = FindById(modification.TargetCityId);
            int sourceCityUpdatedSpace = sourceCity.availableSpace + modification.quantity;
            int targetCityUpdatedSpace = targetCity.availableSpace - modification.quantity;

            string sourceCityUpdateQuery = $"UPDATE city SET available_space = {sourceCityUpdatedSpace} WHERE id = {sourceCityId};";
            string targetCityUpdateQuery = $"UPDATE city SET available_space = {targetCityUpdatedSpace} WHERE id = {targetCityId};";


            // using (var transaction = dbManager.BeginTransaction())

            try
            {
                    if (dbManager.ExecuteNonQuery(sourceCityUpdateQuery) && dbManager.ExecuteNonQuery(targetCityUpdateQuery))
                    {
                        // transaction.Commit();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating available space: {ex.Message}");
                }

                // Rollback if the update fails
                // transaction.Rollback();
                return false;

        }
        public bool UpdateAvailableSpace(long cityId,Modification.Operation operation,int quantity)
        {
            City city = FindById(cityId);

            if (city == null)
            {
                Console.WriteLine($"City with ID {cityId} not found.");
                return false;
            }

            int updatedSpace = city.availableSpace;

            if (operation == Modification.Operation.Addition)
            {
                if (quantity > updatedSpace)
                {
                    Console.WriteLine("Error: Subtraction quantity exceeds available capacity.");
                    return false;
                }
                updatedSpace -= quantity;
            }
            else if (operation == Modification.Operation.Substraction)
            {
                updatedSpace += quantity;
            }
            else if (operation == Modification.Operation.DELETE)
            {
                updatedSpace += quantity;
            }

            if (updatedSpace < 0)
            {
                Console.WriteLine("Error: Updated capacity is negative.");
                return false;
            }

            string updateQuery = $"UPDATE city SET available_space = {updatedSpace} WHERE id = {cityId};";

           // using (var transaction = dbManager.BeginTransaction())
            {
                try
                {
                    if (dbManager.ExecuteNonQuery(updateQuery))
                    {
                       // transaction.Commit();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating available space: {ex.Message}");
                }

                // Rollback if the update fails
               // transaction.Rollback();
                return false;
            }
        }


        public City FindById(long cityId)
        {
            string query = $"SELECT * FROM city WHERE id = {cityId};";
            var (resultList, rowsAffected) = dbManager.ExecuteQuery(query);

            if (resultList == null)
            {
                return null; // Return null if there's an issue with database connectivity or finding the table
            }

            if (rowsAffected)
            {
                if (resultList.Count > 0 &&
                                resultList[0].TryGetValue("id", out var idObj) &&
                                resultList[0].TryGetValue("name", out var nameObj) &&
                                resultList[0].TryGetValue("capacity", out var capacityObj) &&
                                resultList[0].TryGetValue("available_space", out var available_spaceObj))
                {
                    long id = Convert.ToInt64(idObj);
                    string name = nameObj.ToString();
                    int capacity = Convert.ToInt32(capacityObj);
                    int available_space = Convert.ToInt32(available_spaceObj);


                    return new City(id, name, capacity, available_space);
                }
            }
            return null; 
        }


        public long GetLastInsertedId()
        {
            string query = "SELECT LAST_INSERT_ID() FROM production_control.city;";
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


        public bool IsCityNameUnique(string cityName)
        {
            string query = $"SELECT COUNT(*) AS count FROM city WHERE name = '{cityName}';";
            var (resultList, rowsAffected) = dbManager.ExecuteQuery(query);

            if (resultList == null)
            {
                // Handle database connectivity or table not found issue
                Console.WriteLine("Error querying city table: Database connectivity issue.");
                return false;
            }

            if (!rowsAffected || !resultList[0].ContainsKey("count"))
            {
                // Handle case where no rows are affected or "count" key not found
                Console.WriteLine("Error querying city table: No rows affected or key not found.");
                return false;
            }

            return Convert.ToInt32(resultList[0]["count"]) == 0;
        }



        public City SaveCity(City city)
        {
            string query = $"INSERT INTO city (name, capacity, available_space) " +
                           $"VALUES ('{city.name}', '{city.capacity}', '{city.capacity}');";

            //using (var transaction = dbManager.BeginTransaction())
            {
                try
                {
                    if (dbManager.ExecuteNonQuery(query))
                    {
                        city.id = GetLastInsertedId();
                        //transaction.Commit();
                        return city;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving city: {ex.Message}");
                }

                // Rollback if the save operation fails
                //transaction.Rollback();
                return null;
            }
        }

        public bool CityExists(long targetCityId)
        {
            string query = $"SELECT COUNT(*) AS count FROM city WHERE id = {targetCityId};";
            var (resultList, rowsAffected) = dbManager.ExecuteQuery(query);

            if (resultList == null)
            {
                // Handle database connectivity or table not found issue
                Console.WriteLine("Error querying city table: Database connectivity issue.");
                return false;
            }

            if (!rowsAffected || !resultList[0].ContainsKey("count"))
            {
                // Handle case where no rows are affected or "count" key not found
                Console.WriteLine("Error querying city table: No rows affected or key not found.");
                return false;
            }

            return Convert.ToInt32(resultList[0]["count"]) > 0;
        }

        public bool modify(long cityId, string newName, int newCapacity)
        {
            // Retrieve the city information
            City city = FindById(cityId);

            if (city == null)
            {
                Console.WriteLine($"City with ID {cityId} not found.");
                return false;
            }

            // Update the city's name and capacity
            int usedSpace = city.capacity - city.availableSpace;
            city.name = newName;
            city.capacity = newCapacity;
            int availableSpace = city.capacity - usedSpace;


            // Execute the SQL query to update the city information in the database
            string updateQuery = $"UPDATE city SET name = '{newName}', capacity = {newCapacity}, available_space = {availableSpace} WHERE id = {cityId};";

            try
            {
                // Execute the update query
                if (dbManager.ExecuteNonQuery(updateQuery))
                {
                    return true; // Return true if the update is successful
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating city information: {ex.Message}");
            }

            return false; // Return false if the update fails
        }




        public List<City> GetAllCities()
        {
            string query = "SELECT * FROM city;";

            var (resultList, rowsAffected) = dbManager.ExecuteQuery(query);

            if (resultList == null)
            {
                return null; // Return null if there's an issue with database connectivity or finding the table
            }

            if (!rowsAffected)
            {
                return new List<City>(); // Return an empty list if the table is empty
            }

            List<City> cities = new List<City>();

            foreach (var row in resultList)
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
