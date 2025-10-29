namespace Accounting
{
    class Application
    {
        public static void Run()
        {
            Setup();

            string username = "ficfiric";
            string password = "test123";
            string fullName = "Filip Grujic";

            string sessionId = User.Register(username, password, fullName);

            string addrChecking = CheckingAccount.CreateAccount(sessionId);

            Account.Deposit(sessionId, addrChecking, 1000.0);

            string addrSavings = SavingsAccount.CreateAccount(sessionId);

            Account.Deposit(sessionId, addrSavings, 1000.0);
            SavingsAccount.WithdrawToChecking(sessionId, addrSavings, 500.0);
            Console.WriteLine(Account.GetBalanceFormatted(sessionId, addrChecking));
            Console.WriteLine(Account.GetBalanceFormatted(sessionId, addrSavings));
        }
        
        public static void Setup()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
        }
    }
}