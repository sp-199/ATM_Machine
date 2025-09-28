using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FinalProject_28._09._2025
{
    public class Card
    {
        private string name;
        public string Name { 
            set 
            {
                if (value == null) throw new ArgumentNullException("name cannot be empty!");
                if (!value.Contains(' ')) throw new FormatException("Please Enter full name and surname!");
                foreach (char c in value)
                {
                    if (!char.IsLetter(c) && !char.IsWhiteSpace(c))
                        throw new ArgumentException("Cardholder name must contain only letters and spaces.");
                }
                name = value.ToUpper();
            }
            get { return name; }
        }
        private string number;
        public string Number
        {
            get { return number; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Card number cannot be empty.");

                string digitsOnly = value.Replace(" ", "");

                if (digitsOnly.Length != 16)
                    throw new ArgumentException("Card number must be exactly 16 digits.");

                foreach (char c in digitsOnly)
                {
                    if (!char.IsDigit(c))
                        throw new ArgumentException("Card number must contain only digits.");
                }

                number = string.Join(" ",
                    digitsOnly.Substring(0, 4),
                    digitsOnly.Substring(4, 4),
                    digitsOnly.Substring(8, 4),
                    digitsOnly.Substring(12, 4));
            }
        }
        private string date;
        public string Date
        {
            get { return date; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Expiry date cannot be empty.");

                // Keep only digits
                string digitsOnly = new string(value.Where(char.IsDigit).ToArray());

                int month, year;

                if (digitsOnly.Length == 4) // MMYY
                {
                    month = int.Parse(digitsOnly.Substring(0, 2));
                    year = 2000 + int.Parse(digitsOnly.Substring(2, 2));
                }
                else if (digitsOnly.Length == 6) // MMYYYY
                {
                    month = int.Parse(digitsOnly.Substring(0, 2));
                    year = int.Parse(digitsOnly.Substring(2, 4));
                }
                else
                {
                    throw new FormatException("Expiry date must be MMYY, MM YY, or MMYYYY.");
                }

                if (month < 1 || month > 12)
                    throw new FormatException("Expiry month must be between 01 and 12.");

                if (year > 2100)
                    throw new ArgumentException("Expiry year cannot be greater than 2100.");

                DateTime expiry = new DateTime(year, month, 1).AddMonths(1).AddDays(-1); // last day of the month
                if (expiry < DateTime.Now)
                    throw new ArgumentException("Card is expired.");

                date = month.ToString("D2") + "/" + (year % 100).ToString("D2"); // store as MM/YY
            }
        }
        private string cvv;
        public string Cvv
        {
            get { return cvv; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("CVV cannot be empty.");

                if (!value.All(char.IsDigit))
                    throw new FormatException("CVV must contain only digits.");

                if (value.Length != 3)
                    throw new ArgumentException("CVV must be exactly 3 digits.");

                cvv = value;
            }
        }
        private string pin;
        public string Pin
        {
            get { return pin; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("PIN cannot be empty.");

                if (!value.All(char.IsDigit))
                    throw new FormatException("PIN must contain only digits.");

                if (value.Length != 4)
                    throw new ArgumentException("PIN must be exactly 4 digits.");

                pin = value;
            }
        }
        public double balanceGel { get; set; }
        public double balanceUsd { get; set; }
        public double balanceEur { get; set; }


        public double BalanceGel() { return balanceGel; }
        public double BalanceUsd() { return balanceUsd; }
        public double BalanceEur() { return balanceEur; }
        public List<string> Transactions { get; set; } = new List<string>();

        public void Deposit(string currency, double amount)
        {
            if (amount<=0)
            {
                Console.WriteLine("You must deposit the amount greater than 0");
            }
            else
            {
                if (currency == "GEL") balanceGel += amount;
                else if (currency == "USD") balanceUsd += amount;
                else if (currency == "EUR") balanceEur += amount;

                Transactions.Add($"Deposited {amount} {currency} at {DateTime.Now}");
            }
        }

        public void Withdraw(string currency, double amount)
        {
            bool success = false;
            if ( amount <= 0)
            {
                Console.WriteLine("You must withdraw the amount greater than 0");
            }
            else 
            {
                if (currency == "GEL" && balanceGel >= amount) { balanceGel -= amount; success = true; }
                else if (currency == "USD" && balanceUsd >= amount) { balanceUsd -= amount; success = true; }
                else if (currency == "EUR" && balanceEur >= amount) { balanceEur -= amount; success = true; }

                if (success) Transactions.Add($"Withdrew {amount} {currency} at {DateTime.Now}");
                else Transactions.Add($"Failed Withdrawal {amount} {currency} at {DateTime.Now}");
            }
        }
        // Save card to JSON file
        public void SaveCard(string filename)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(this, options);
            File.WriteAllText(filename, json);
        }

        // Load card from JSON file
        public static Card LoadCard(string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException("Card file not found.");
            string json = File.ReadAllText(filename);
            return JsonSerializer.Deserialize<Card>(json);
        }
    }
}
