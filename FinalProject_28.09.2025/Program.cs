using FinalProject_28._09._2025;
using System.Text.Json;

public static class Program
{
    static string filePath = "cards.json";
    static List<Card> cards = LoadCards();

    public static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("1. Register New Card");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Exit");
            Console.Write("Choose an option [1-3]: ");
            string choice = Console.ReadLine();

            if (choice == "1") // Register new card
            {
                Card card = new Card();
                Console.Clear();
                // Name
                while (true)
                {
                    Console.Write("Enter cardholder name (first and last): ");
                    try
                    {
                        card.Name = Console.ReadLine();
                        Console.WriteLine($"Cardholder Name Saved As: {card.Name}");
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{ex.Message}");
                    }
                }

                // Number
                while (true)
                {
                    Console.Write("Enter card Number: ");
                    try
                    {
                        card.Number = Console.ReadLine();
                        Console.WriteLine($"Card Number Saved As: {card.Number}");
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{ex.Message}");
                    }
                }

                // Expiry date
                while (true)
                {
                    Console.Write("Enter expiry date (MMYY, MM/YY, MM YY, MMYYYY): ");
                    try
                    {
                        card.Date = Console.ReadLine();
                        Console.WriteLine($"Card Expiry Date Saved As: {card.Date}");
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{ex.Message}");
                    }
                }

                // CVV
                while (true)
                {
                    Console.Write("Enter CVV (3 digits): ");
                    try
                    {
                        card.Cvv = Console.ReadLine();
                        Console.WriteLine($"Card CVV Saved As: ***");
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{ex.Message}");
                    }
                }

                // PIN
                while (true)
                {
                    Console.Write("Set a 4-digit PIN: ");
                    try
                    {
                        card.Pin = Console.ReadLine();
                        Console.WriteLine($"Card PIN Saved As: ****");
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{ex.Message}");
                    }
                }
                cards.Add(card);
                SaveCards();
                Console.WriteLine();
                Console.WriteLine("Card created and saved!");
                Console.WriteLine("Press any key to return to main menu...");
                Console.ReadKey();
            }
            else if (choice == "2") // Login
            {
                Card currentCard = null;
                Console.Clear();
                while (currentCard == null)
                {
                    Console.Write("Enter card number: ");
                    string inputNumber = Console.ReadLine().Replace(" ", "");

                    currentCard = cards.FirstOrDefault(c => c.Number.Replace(" ", "") == inputNumber);

                    if (currentCard == null)
                    {
                        Console.WriteLine("Card not found. Try again.");
                    }
                }

                while (true)
                {
                    Console.Write("Enter PIN: ");
                    string inputPin = Console.ReadLine();
                    if (currentCard.Pin == inputPin) break;
                    Console.WriteLine("Incorrect PIN. Try again.");
                }

                bool exit = false;
                while (!exit) // inner login menu
                {
                    Console.Clear();
                    Console.WriteLine($"Login successful! Welcome, {currentCard.Name}");
                    welcomeScreen();
                    Console.Write("Enter Your Answer [1 - 6]: ");
                    string answer = Console.ReadLine();
                    if (!int.TryParse(answer, out int ans))
                    {
                        Console.WriteLine("Error: Invalid Answer (not a number)");
                        continue;
                    }

                    switch (ans)
                    {
                        case 1: // Deposit
                            Console.Clear();
                            Console.WriteLine(new string('=', 50));
                            Console.WriteLine();
                            Console.WriteLine("Current Balance: " + currentCard.balanceGel + " GEL || " + currentCard.balanceUsd + " USD || " + currentCard.balanceEur + " EUR");
                            Console.WriteLine();
                            Console.WriteLine(new string('=', 50));
                            HandleDeposit(currentCard);
                            Console.WriteLine("Press any key to return to main menu...");
                            Console.ReadKey();
                            break;
                        case 2: // Withdraw
                            Console.Clear();
                            Console.WriteLine(new string('=', 50));
                            Console.WriteLine();
                            Console.WriteLine("Current Balance: " + currentCard.balanceGel + " GEL || " + currentCard.balanceUsd + " USD || " + currentCard.balanceEur + " EUR");
                            Console.WriteLine();
                            Console.WriteLine(new string('=', 50));
                            HandleWithdraw(currentCard);
                            Console.WriteLine("Press any key to return to main menu...");
                            Console.ReadKey();
                            break;
                        case 3: // Check Balance
                            Console.Clear();
                            Console.WriteLine("Current Balance");
                            Console.WriteLine(new string('=', 50));
                            Console.WriteLine($"GEL: {currentCard.BalanceGel():F2}");
                            Console.WriteLine($"USD: {currentCard.BalanceUsd():F2}");
                            Console.WriteLine($"EUR: {currentCard.BalanceEur():F2}");
                            Console.WriteLine(new string('=', 50));
                            Console.WriteLine("Press any key to return to menu");
                            Console.ReadKey();
                            break;
                        case 4: // Last 5 Transactions
                            Console.Clear();
                            var last5 = currentCard.Transactions.TakeLast(5);
                            Console.WriteLine("Last 5 Transactions:");
                            foreach (var t in last5) Console.WriteLine(t);
                            Console.WriteLine(new string('=', 50));
                            Console.WriteLine("Press any key to return to menu...");
                            Console.ReadKey();
                            break;
                        case 5: // Change PIN
                            Console.Clear();
                            while (true)
                            {
                                Console.Write("Enter current PIN: ");
                                string currentPinInput = Console.ReadLine();

                                if (currentPinInput == currentCard.Pin)
                                {
                                    break; // correct PIN entered, move on
                                }

                                Console.WriteLine("Incorrect current PIN. Try again.");
                            }
                            string newPin1, newPin2;
                            while (true)
                            {
                                Console.Write("Enter new 4-digit PIN: ");
                                newPin1 = Console.ReadLine();

                                if (newPin1.Length != 4 || !newPin1.All(char.IsDigit))
                                {
                                    Console.WriteLine("PIN must be exactly 4 digits.");
                                    continue;
                                }

                                Console.Write("Re-enter new PIN: ");
                                newPin2 = Console.ReadLine();

                                if (newPin1 != newPin2)
                                {
                                    Console.WriteLine("PINs do not match. Try again.");
                                    continue;
                                }

                                break;
                            }
                            currentCard.Pin = newPin1;
                            SaveCards();
                            Console.WriteLine("PIN changed successfully.");
                            break;
                        case 6: // Exit / Logout
                            Console.WriteLine("Logging out...");
                            exit = true; // leaves login menu
                            break;
                        case 7: //Delete the Card
                            Console.WriteLine("Deleting the Card");
                            cards.Remove(currentCard);
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Invalid Answer (must be 1-6)");
                            break;
                    }
                }
            }
            else if (choice == "3") // Exit whole program
            {
                Console.Clear ();
                Console.WriteLine("Goodbye!");
                break; // breaks outer loop = closes program
            }
            else
            {
                Console.WriteLine("Invalid choice. Please try again.");
            }
        }

    }

    static void HandleDeposit(Card card)
    {
        while (true) // loop until user chooses Back
        {
            Console.WriteLine("\nChoose The Account to Deposit Into:");
            Console.WriteLine("1. GEL");
            Console.WriteLine("2. USD ");
            Console.WriteLine("3. EUR ");
            Console.WriteLine("4. Back");
            Console.Write("Enter Your Option [1-4]: ");
            string choice = Console.ReadLine();

            if (choice == "4")
            {
                // go back to main menu
                Console.WriteLine("Returning to main menu...");
                break;
            }

            Console.Write("Enter The Amount You Want To Deposit: ");
            string input = Console.ReadLine();
            if (!double.TryParse(input, out double amount) || amount <= 0)
            {
                Console.WriteLine("Invalid amount. Please enter a positive number.");
                continue; // retry
            }

            try
            {
                switch (choice)
                {
                    case "1":
                        card.Deposit("GEL", amount);
                        break;
                    case "2":
                        card.Deposit("USD", amount);
                        break;
                    case "3":
                        card.Withdraw("EUR", amount);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please select 1-4.");
                        continue;
                }

                SaveCards();
                Console.WriteLine($"Successfully deposited {amount:F2} into your account.");
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Deposit failed: {ex.Message}");
            }
        }
    }


    static void HandleWithdraw(Card card)
{
    while (true) // keep repeating until user chooses "Back"
    {
        Console.WriteLine("\nChoose The Account to Withdraw From:");
        Console.WriteLine("1. GEL");
        Console.WriteLine("2. USD");
        Console.WriteLine("3. EUR");
        Console.WriteLine("4. Back");
        Console.Write("Enter Your Option [1-4]: ");
        string choice = Console.ReadLine();

        if (choice == "4") // back to main menu
            break;

        Console.Write("Enter the amount you want to withdraw: ");
        if (double.TryParse(Console.ReadLine(), out double amount) && amount > 0)
        {
            bool success = false;

            switch (choice)
            {
                case "1":
                    if (card.BalanceGel() >= amount)
                    {
                        card.Withdraw("GEL", amount);
                        success = true;
                    }
                    break;
                case "2":
                    if (card.BalanceUsd() >= amount)
                    {
                        card.Withdraw("USD", amount);
                        success = true;
                    }
                    break;
                case "3":
                    if (card.BalanceEur() >= amount)
                    {
                        card.Withdraw("EUR", amount);
                        success = true;
                    }
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    continue;
            }

            if (success)
            {
                SaveCards();
                Console.WriteLine("Withdrawal successful.");
                    break;
            }
            else
            {
                Console.WriteLine("Insufficient funds.");
            }
        }
        else
        {
            Console.WriteLine("Invalid amount.");
        }
    }
}


    static List<Card> LoadCards()
    {
        if (!File.Exists(filePath)) return new List<Card>();
        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<Card>>(json);
    }

    static void SaveCards()
    {
        string json = JsonSerializer.Serialize(cards, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    static void welcomeScreen()
    {
        Console.WriteLine();
        Console.WriteLine("=== Main Menu ===");
        Console.WriteLine("1. Deposit Money");
        Console.WriteLine("2. Withdraw Money");
        Console.WriteLine("3. Check Balance");
        Console.WriteLine("4. See The Last 5 Transactions");
        Console.WriteLine("5. Change PIN");
        Console.WriteLine("6. Log Out");
        Console.WriteLine("7. Delete/Cancel Your Card");
    }
}

