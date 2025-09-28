using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ATM_Simulator.Services
{
    class Logger
    {
        private static readonly string LogFilePath = "logs.txt";

        public static void Log(string message)
        {
            try
            {
                // ჩანაწერს ვუმატებთ თარიღსა და დროს
                string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";

                // ვწერთ ჩანაწერს ფაილში, ვამატებთ ახალ ხაზზე (true-ის გამო)
                // logs.txt ფაილის შექმნა არ გჭირდებათ.როდესაც თქვენ გამოიძახებთ Logger.Log() მეთოდს პირველად,
                // File.AppendAllText() მეთოდი ავტომატურად შექმნის logs.txt ფაილს იმავე საქაღალდეში,
                // სადაც თქვენი პროგრამის შესრულებადი ფაილი(.exe) მდებარეობს.
                File.AppendAllText(LogFilePath, logEntry + Environment.NewLine);
            }
            catch (Exception ex)
            {
                // თუ ლოგის ფაილში წერა ვერ მოხერხდა
                Console.WriteLine($"Error writing to log file: {ex.Message}");
            }
        }
    }
}
