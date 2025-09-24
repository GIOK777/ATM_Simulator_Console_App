using ATM_Simulator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ATM_Simulator.Services
{
    // JsonManager.cs კლასი პასუხისმგებელი იქნება JSON-დან მონაცემების წაკითხვასა და ჩაწერაზე.
    // ამისთვის დაგჭირდებათ System.Text.Json ან Newtonsoft.Json ბიბლიოთეკა
    public static class JsonManager // რატომ სტატიკური კლასი - 
                                    // ერთჯერადი დანიშნულება: ეს კლასი ემსახურება ერთ კონკრეტულ დავალებას — JSON-თან მუშაობას.
                                    // პროგრამას არ სჭირდება ამ დავალებისთვის მრავალი ინსტანცია. ობიექტის შექმნის გარეშე პირდაპირ გამოიყენება
    {
        // ეს მეთოდი კითხულობს მომხმარებლის მონაცემებს JSON ფაილიდან და გარდაქმნის მათ C#-ის ობიექტების სიაში (List<User>).
        public static List<User> LoadUsers(string filePath)
        {
            try
            {
                string jsonString = File.ReadAllText(filePath); // კითხულობს მთელ ტექსტს მოცემული ფაილიდან (filePath) და ინახავს მას jsonString ცვლადში
                return JsonSerializer.Deserialize<List<User>>(jsonString); //მეთოდი იღებს JSON ფორმატის ტექსტს (jsonString) და გარდაქმნის მას C#-ის ობიექტების სიაში (List<User>)
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading JSON file: {ex.Message}");
                return new List<User>(); //შეცდომის შემთხვევაში, მეთოდი აბრუნებს ცარიელ სიას, რომ პროგრამა არ დაიხუროს და გააგრძელოს მუშაობა
            }
        }

        // ეს მეთოდი იღებს მომხმარებელთა სიას და ინახავს მას JSON ფორმატში, ფაილში.
        public static void SaveUsers(List<User> users, string filePath)
        {
            var options = new JsonSerializerOptions { WriteIndented = true }; // ეს პარამეტრი უზრუნველყოფს, რომ JSON ფაილი იყოს ფორმატირებული და ადვილად წასაკითხი
            string jsonString = JsonSerializer.Serialize(users, options); //  იღებს C#-ის ობიექტების სიას (users) და გარდაქმნის მას JSON ფორმატის ტექსტად.
            File.WriteAllText(filePath, jsonString); // წერს შექმნილ JSON ტექსტს მოცემულ ფაილში, ფაილის წინა შიგთავსის გადაწერით
        }
    }
}
