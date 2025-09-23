using ATM_Simulator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ATM_Simulator.Services
{
    public static class JsonManager // რატომ სტატიკური კლასი და არა ინტერნალ სტატიკის გარეშე?
    {
        public static List<User> LoadUsers(string filePath)
        {
            try
            {
                string jsonString = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<List<User>>(jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading JSON file: {ex.Message}");
                return new List<User>();
            }
        }

        public static void SaveUsers(List<User> users, string filePath)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(users, options);
            File.WriteAllText(filePath, jsonString);
        }
    }
}
