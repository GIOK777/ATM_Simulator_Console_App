using ATM_Simulator.Models;
using ATM_Simulator.Services;

namespace ATM_Simulator
{
    // ეს არის მთავარი ფაილი, სადაც განხორციელდება ყველა ოპერაცია.აქ გამოვიყენებთ LINQ-ს.
    internal class Program
    {
        private static List<User> users;
        //private static string usersFilePath;
        private static string usersFilePath = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName, "data", "users.json");

        //private const string DataFilePath = "Data/users.json";
        private static string GetFilePath(string fileName)
        {
            // იღებს აპლიკაციის ძირეულ დირექტორიას (სადაც .exe ფაილია)
            string appDir = AppContext.BaseDirectory;
            // აერთიანებს გზას, რათა მიიღოს აბსოლუტური გზა
            return Path.Combine(appDir, "data", fileName);
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
          
            //usersFilePath = GetFilePath("users.json");
            users = JsonManager.LoadUsers(usersFilePath);

            // 1. ვალიდაცია და ავტორიზაცია
            User authenticatedUser = AuthenticateUser();
            if (authenticatedUser == null)
            {
                Console.WriteLine("Invalid card or PIN. Exiting.");
                return;
            }

            // 2. მენიუ და მოქმედებები
            ShowMenu(authenticatedUser);
        }




        private static User AuthenticateUser()
        {
            // ბარათის ნომრის ვალიდაცია LINQ-ის გამოყენებით
            Console.Write("Enter card number: ");
            //string cardNumber = Console.ReadLine();
            string cardNumber = Console.ReadLine().Trim();

            // users.FirstOrDefault(...): ეს არის LINQ (Language Integrated Query)-ის მეთოდი.
            // ის ეძებს პირველ ელემენტს (User ობიექტს) users სიაში, რომელიც აკმაყოფილებს ფრჩხილებში მოცემულ პირობას.
            // (u => u.CardDetails.CardNumber == cardNumber) : ეს არის Lambda ექსპრესია.
            // ის ამბობს: "თითოეული მომხმარებლისთვის (u) შეამოწმე, თუ მისი ბარათის დეტალებში არსებული ბარათის ნომერი
            // (u.CardDetails.CardNumber) ზუსტად ემთხვევა შეყვანილ ნომერს (cardNumber)"
            // თუ მომხმარებელი ნაპოვნია, ის ინახება userByCard ცვლადში

            //User userByCard = users.FirstOrDefault(u => u.CardDetails.CardNumber == cardNumber);
            User userByCard = users.FirstOrDefault(u => string.Equals(u.CardDetails.CardNumber, cardNumber));

            //var userByCard = users.FirstOrDefault(u => string.Equals(u?.CardDetails?.CardNumber?.Trim(), cardNumber, StringComparison.Ordinal));

            if (userByCard == null) return null; // თუ ვერ იპოვა მომხმარებელი, აბრუნებს null


            // ვადა და პინ კოდის ვალიდაცია
            Console.Write("Enter expiration date (MM/YY): ");
            string expirationDate = Console.ReadLine();
            if (userByCard.CardDetails.ExpirationDate != expirationDate) return null;

            Console.Write("Enter PIN code: ");
            string pinCode = Console.ReadLine();
            if (userByCard.PinCode != pinCode) return null;

            return userByCard;
        }



        private static void ShowMenu(User user)
        {
            while (true)
            {
                Console.WriteLine("\n--- Main Menu ---");
                Console.WriteLine("1. View Balance");
                Console.WriteLine("2. Withdraw Amount");
                Console.WriteLine("3. Deposit Amount");
                Console.WriteLine("4. View Last 5 Transactions");
                Console.WriteLine("5. Change PIN");
                Console.WriteLine("6. Currency Conversion");
                Console.WriteLine("7. Exit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        ViewBalance(user);
                        break;
                    case "2":
                        WithdrawAmount(user);
                        break;
                    case "3":
                        DepositAmount(user);
                        break;
                    case "4":
                        ViewLast5Transactions(user);
                        break;
                    case "5":
                        ChangePin(user);
                        break;
                    case "6":
                        CurrencyConversion(user);
                        break;
                    case "7":
                        Console.WriteLine("Thank you for using our service!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        // მოქმედებების მეთოდები
        private static void ViewBalance(User user)
        {
            Console.WriteLine($"Current Balance:");
            Console.WriteLine($"GEL: {user.BalanceGEL}");
            Console.WriteLine($"USD: {user.BalanceUSD}");
            Console.WriteLine($"EUR: {user.BalanceEUR}");

            // ტრანზაქციის დამატება
            user.TransactionHistory.Add(new Transaction
            {
                TransactionDate = DateTime.UtcNow,
                TransactionType = "Balance Inquiry",
                AmountGEL = 0,
                AmountUSD = 0,
                AmountEUR = 0
            });
            JsonManager.SaveUsers(users, usersFilePath);
        }

        


        private static void WithdrawAmount(User user)
        {
            Console.WriteLine("Choose currency to withdraw:");
            Console.WriteLine("1. GEL");
            Console.WriteLine("2. USD");
            Console.WriteLine("3. EUR");
            Console.Write("Your choice: ");
            string currencyChoice = Console.ReadLine();

            Console.Write("Enter amount to withdraw: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
            {
                string transactionType = "Withdrawal";
                switch (currencyChoice)
                {
                    case "1":
                        if (user.BalanceGEL >= amount)
                        {
                            user.BalanceGEL -= amount;
                            Console.WriteLine($"Successfully withdrew {amount} GEL. New balance: {user.BalanceGEL} GEL");
                            user.TransactionHistory.Add(new Transaction { TransactionDate = DateTime.UtcNow, TransactionType = transactionType, AmountGEL = amount, AmountUSD = 0, AmountEUR = 0 });
                        }
                        else
                        {
                            Console.WriteLine("Insufficient GEL balance.");
                            return;
                        }
                        break;
                    case "2":
                        if (user.BalanceUSD >= amount)
                        {
                            user.BalanceUSD -= amount;
                            Console.WriteLine($"Successfully withdrew {amount} USD. New balance: {user.BalanceUSD} USD");
                            user.TransactionHistory.Add(new Transaction { TransactionDate = DateTime.UtcNow, TransactionType = transactionType, AmountGEL = 0, AmountUSD = amount, AmountEUR = 0 });
                        }
                        else
                        {
                            Console.WriteLine("Insufficient USD balance.");
                            return;
                        }
                        break;
                    case "3":
                        if (user.BalanceEUR >= amount)
                        {
                            user.BalanceEUR -= amount;
                            Console.WriteLine($"Successfully withdrew {amount} EUR. New balance: {user.BalanceEUR} EUR");
                            user.TransactionHistory.Add(new Transaction { TransactionDate = DateTime.UtcNow, TransactionType = transactionType, AmountGEL = 0, AmountUSD = 0, AmountEUR = amount });
                        }
                        else
                        {
                            Console.WriteLine("Insufficient EUR balance.");
                            return;
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid currency choice.");
                        return;
                }
                JsonManager.SaveUsers(users, usersFilePath);
            }
            else
            {
                Console.WriteLine("Invalid amount.");
            }
        }


            // LINQ-ის გამოყენებით ბოლო 5 ტრანზაქციის ჩვენება
            private static void ViewLast5Transactions(User user)
        {
            Console.WriteLine("--- Last 5 Transactions ---");
            var lastFiveTransactions = user.TransactionHistory
                .OrderByDescending(t => t.TransactionDate)
                .Take(5)
                .ToList();

            if (!lastFiveTransactions.Any())
            {
                Console.WriteLine("No transactions found.");
                return;
            }

            foreach (var transaction in lastFiveTransactions)
            {
                Console.WriteLine($"Date: {transaction.TransactionDate}, Type: {transaction.TransactionType}, Amount (GEL): {transaction.AmountGEL:C}");
            }

            user.TransactionHistory.Add(new Transaction
            {
                TransactionDate = DateTime.UtcNow,
                TransactionType = "Transaction History Inquiry",
                AmountGEL = 0,
                AmountUSD = 0,
                AmountEUR = 0
            });
            JsonManager.SaveUsers(users, usersFilePath);
        }

        private static void DepositAmount(User user)
        {
            Console.WriteLine("Choose currency to deposit:");
            Console.WriteLine("1. GEL");
            Console.WriteLine("2. USD");
            Console.WriteLine("3. EUR");
            Console.Write("Your choice: ");
            string currencyChoice = Console.ReadLine();

            Console.Write("Enter amount to deposit: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
            {
                string transactionType = "Deposit";
                switch (currencyChoice)
                {
                    case "1":
                        user.BalanceGEL += amount;
                        Console.WriteLine($"Successfully deposited {amount:C} GEL. New balance: {user.BalanceGEL:C}");
                        user.TransactionHistory.Add(new Transaction { TransactionDate = DateTime.UtcNow, TransactionType = transactionType, AmountGEL = amount, AmountUSD = 0, AmountEUR = 0 });
                        break;
                    case "2":
                        user.BalanceUSD += amount;
                        Console.WriteLine($"Successfully deposited {amount:C} USD. New balance: {user.BalanceUSD:C}");
                        user.TransactionHistory.Add(new Transaction { TransactionDate = DateTime.UtcNow, TransactionType = transactionType, AmountGEL = 0, AmountUSD = amount, AmountEUR = 0 });
                        break;
                    case "3":
                        user.BalanceEUR += amount;
                        Console.WriteLine($"Successfully deposited {amount:C} EUR. New balance: {user.BalanceEUR:C}");
                        user.TransactionHistory.Add(new Transaction { TransactionDate = DateTime.UtcNow, TransactionType = transactionType, AmountGEL = 0, AmountUSD = 0, AmountEUR = amount });
                        break;
                    default:
                        Console.WriteLine("Invalid currency choice.");
                        return;
                }
                JsonManager.SaveUsers(users, usersFilePath);
            }
            else
            {
                Console.WriteLine("Invalid amount.");
            }
        }


        private static void ChangePin(User user)
        {
            Console.Write("Enter new PIN code (4 digits): ");
            string newPin = Console.ReadLine();

            if (newPin.Length == 4 && newPin.All(char.IsDigit))
            {
                user.PinCode = newPin;
                Console.WriteLine("PIN code changed successfully.");

                user.TransactionHistory.Add(new Transaction
                {
                    TransactionDate = DateTime.UtcNow,
                    TransactionType = "Change Pin",
                    AmountGEL = 0,
                    AmountUSD = 0,
                    AmountEUR = 0
                });
                JsonManager.SaveUsers(users, usersFilePath);
            }
            else
            {
                Console.WriteLine("Invalid PIN format. PIN must be 4 digits.");
            }
        }

        private static void CurrencyConversion(User user)
        {
            // For simplicity, we'll use hardcoded exchange rates
            const decimal GEL_TO_USD_RATE = 0.36m;
            const decimal USD_TO_GEL_RATE = 2.75m;
            const decimal GEL_TO_EUR_RATE = 0.33m;
            const decimal EUR_TO_GEL_RATE = 3.00m;
            const decimal USD_TO_EUR_RATE = 0.92m;
            const decimal EUR_TO_USD_RATE = 1.08m;

            Console.WriteLine("Choose conversion direction:");
            Console.WriteLine("1. GEL to USD");
            Console.WriteLine("2. USD to GEL");
            Console.WriteLine("3. GEL to EUR");
            Console.WriteLine("4. EUR to GEL");
            Console.WriteLine("5. USD to EUR");
            Console.WriteLine("6. EUR to USD");
            Console.Write("Your choice: ");

            string choice = Console.ReadLine();

            Console.Write("Enter amount to convert: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
            {
                decimal convertedAmount = 0;
                string transactionType = "Currency Conversion";
                bool success = true;

                switch (choice)
                {
                    case "1":
                        if (user.BalanceGEL >= amount) { convertedAmount = amount * GEL_TO_USD_RATE; user.BalanceGEL -= amount; user.BalanceUSD += convertedAmount; } else { Console.WriteLine("Insufficient GEL balance."); success = false; }
                        break;
                    case "2":
                        if (user.BalanceUSD >= amount) { convertedAmount = amount * USD_TO_GEL_RATE; user.BalanceUSD -= amount; user.BalanceGEL += convertedAmount; } else { Console.WriteLine("Insufficient USD balance."); success = false; }
                        break;
                    case "3":
                        if (user.BalanceGEL >= amount) { convertedAmount = amount * GEL_TO_EUR_RATE; user.BalanceGEL -= amount; user.BalanceEUR += convertedAmount; } else { Console.WriteLine("Insufficient GEL balance."); success = false; }
                        break;
                    case "4":
                        if (user.BalanceEUR >= amount) { convertedAmount = amount * EUR_TO_GEL_RATE; user.BalanceEUR -= amount; user.BalanceGEL += convertedAmount; } else { Console.WriteLine("Insufficient EUR balance."); success = false; }
                        break;
                    case "5":
                        if (user.BalanceUSD >= amount) { convertedAmount = amount * USD_TO_EUR_RATE; user.BalanceUSD -= amount; user.BalanceEUR += convertedAmount; } else { Console.WriteLine("Insufficient USD balance."); success = false; }
                        break;
                    case "6":
                        if (user.BalanceEUR >= amount) { convertedAmount = amount * EUR_TO_USD_RATE; user.BalanceEUR -= amount; user.BalanceUSD += convertedAmount; } else { Console.WriteLine("Insufficient EUR balance."); success = false; }
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        return;
                }

                if (success)
                {
                    Console.WriteLine($"Converted amount: {convertedAmount:C}");
                    user.TransactionHistory.Add(new Transaction
                    {
                        TransactionDate = DateTime.UtcNow,
                        TransactionType = transactionType,
                        AmountGEL = choice.Contains("GEL") ? amount : (choice.Contains("to GEL") ? convertedAmount : 0),
                        AmountUSD = choice.Contains("USD") ? amount : (choice.Contains("to USD") ? convertedAmount : 0),
                        AmountEUR = choice.Contains("EUR") ? amount : (choice.Contains("to EUR") ? convertedAmount : 0)
                    });
                    JsonManager.SaveUsers(users, usersFilePath);
                }
            }
            else
            {
                Console.WriteLine("Invalid amount.");
            }
        }
    }
}
