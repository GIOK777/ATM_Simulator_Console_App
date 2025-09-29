using ATM_Simulator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ATM_Simulator.Services
{
    public static class JsonManager 
    {
        public static List<User> LoadUsers(string filePath)
        {
            try
            {
                string jsonString = File.ReadAllText(filePath); 
                return JsonSerializer.Deserialize<List<User>>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true   
                }) ?? new List<User>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading JSON file: {ex.Message}");
                return new List<User>();
            }
        }


        public static void SaveUsers(List<User> users, string filePath)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(users, options);
                File.WriteAllText(filePath, jsonString);
            }
            catch (Exception ex)
            {       
                Console.WriteLine($"Error saving JSON file: {ex.Message}");
            }
        }
    }
}
