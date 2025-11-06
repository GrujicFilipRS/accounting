using System.Text;
using dotenv.net;

namespace Accounting;

class Application
{
    private static string? session;
    private static string? checkingAddr;
    private static string? savingsAddr;

    public static void Run()
    {
        Console.WriteLine("Welcome to the Accounting app");
        Console.WriteLine("Please enter your credentials");
        session = GetSession();
        checkingAddr = CheckingAccount.GetAccountBySession(session);
        savingsAddr = SavingsAccount.GetAccountBySession(session);

        while (DoAction()) ;
    }

    public static void Setup()
    {
        Console.OutputEncoding = Encoding.UTF8;
        DotEnv.Load();
        DatabaseHandler.LoadConnectionParams();
        if (!DatabaseHandler.VerifyConnection())
        {
            Console.WriteLine("Exiting, due to database connection not being made");
            Environment.Exit(1);
        }
    }

    public static void LoadData()
    {
        User.LoadUsers();
        Account.LoadAccounts();
    }

    private static string GetSession()
    {
        while (true)
        {
            string verificationType = "";
            string username = "";
            string password = "";
            string? fullName = "";
            Console.WriteLine("Login (L) or Register (R)");
            Console.Write("> ");
            verificationType = Console.ReadLine()!.ToUpper();

            if (verificationType != "L" && verificationType != "R")
            {
                Console.WriteLine($"Unknown verification type: {verificationType}");
                continue;
            }

            Console.WriteLine("Please enter username");
            Console.Write("> ");
            username = Console.ReadLine()!;

            Console.WriteLine("Please enter password");
            Console.Write("> ");
            password = GetPassword();

            if (verificationType == "R")
            {
                Console.WriteLine("Please enter your full name");
                Console.Write("> ");
                fullName = Console.ReadLine()!;

                return User.Register(username, password, fullName);
            }

            return User.Login(username, password);
        }
    }

    private static string GetPassword()
    {
        StringBuilder input = new();
        while (true)
        {
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Enter) break;
            if (key.Key == ConsoleKey.Backspace && input.Length > 0) input.Remove(input.Length - 1, 1);
            else if (key.Key != ConsoleKey.Backspace) input.Append(key.KeyChar);
        }
        Console.WriteLine();
        return input.ToString();
    }

    private static bool DoAction()
    {
        Console.WriteLine("\nPossible options:");
        Console.WriteLine("  1. Check balance of accounts");
        Console.WriteLine("  2. Create an account");
        Console.WriteLine("  3. Transfer from checking to savings");
        Console.WriteLine("  4. Transfer from savings to checking");
        Console.WriteLine("  5. Transfer to other accounts");
        Console.WriteLine("  6. Check balance of accounts");
        Console.WriteLine("  7. Redeem interest");
        Console.WriteLine("  8. Exit");

        Console.Write("\n> ");

        string actionMode = Console.ReadLine()!;
        switch (actionMode)
        {
            case "1":
                Console.WriteLine("Checking (C) or Savings (S) account?");
                string? accountSelected = Console.ReadLine();
                if (accountSelected != "C" && accountSelected != "S")
                {
                    Console.WriteLine($"Unknown account type: {accountSelected}");
                    return true;
                }

                if ((string.IsNullOrEmpty(checkingAddr) && accountSelected == "C")
                    || (string.IsNullOrEmpty(savingsAddr) && accountSelected == "S"))
                {
                    Console.WriteLine("You don't have that type of account");
                    return true;
                }

                string addrSelected = accountSelected == "C" ? checkingAddr! : savingsAddr!;
                Console.WriteLine(Account.GetBalanceFormatted(session!, addrSelected));
                break;

            case "2":
                Console.WriteLine("Choose account type");
                Console.Write("Checking (C) or Savings (S) account?\n >");

                accountSelected = Console.ReadLine();

                if (accountSelected == "C") checkingAddr = CheckingAccount.CreateAccount(session!);
                if (accountSelected == "S") checkingAddr = SavingsAccount.CreateAccount(session!);
                Console.WriteLine($"Unknown account type: {accountSelected}");
                return true;

            case "3":
                Console.Write("Choose amount (EUR)\n >");
                double amount = double.Parse(Console.ReadLine()!);

                SavingsAccount.DepositFromChecking(session!, savingsAddr!, amount);
                return true;

            case "4":
                Console.Write("Choose amount (EUR)\n >");
                amount = double.Parse(Console.ReadLine()!);

                SavingsAccount.WithdrawToChecking(session!, savingsAddr!, amount);
                return true;

            case "5":
                Console.Write("Enter address\n >");
                string addr = Console.ReadLine()!;

                Console.Write("Choose amount (EUR)\n >");
                amount = double.Parse(Console.ReadLine()!);

                CheckingAccount.Transfer(session!, checkingAddr!, addr, amount);
                return true;

            case "6":
                Console.WriteLine("Checking (C) or Savings (S) account?");
                accountSelected = Console.ReadLine();
                if (accountSelected == "C")
                    Console.WriteLine(Account.GetBalanceFormatted(session!, checkingAddr!));
                else if (accountSelected == "S")
                    Console.WriteLine(Account.GetBalanceFormatted(session!, savingsAddr!));
                else
                    Console.WriteLine($"Unknown account type {accountSelected}");

                return true;

            case "7":
                SavingsAccount.ApplyInterest(session!, savingsAddr!);
                Console.WriteLine("Interest redeemed");
                return true;

            case "8":
                return false;

            default:
                Console.WriteLine($"Unknown action {actionMode}");
                return true;
        }

        return true;
    }
}