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

            string addr = SavingsAccount.CreateAccount(sessionId);

            Account.Deposit(sessionId, addr, 1000.0);

            Console.WriteLine(Account.GetBalanceFormatted(sessionId, addr));
            Thread.Sleep(61000);

            SavingsAccount.ApplyInterest(sessionId, addr);

            Console.WriteLine(Account.GetBalanceFormatted(sessionId, addr));
        }
        
        public static void Setup()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
        }
    }
}