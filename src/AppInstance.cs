namespace Accounting
{
    class Application
    {
        public static void Run()
        {
            string username = "ficfiric";
            string password = "test123";
            string fullName = "Filip Grujic";

            string sessionId = User.Register(username, password, fullName);

            string addr = CheckingAccount.CreateAccount(sessionId);
            Account.Deposit(sessionId, addr, 1000.0);
            Console.WriteLine(Account.GetBalanceFormatted(sessionId, addr));
        }
    }
}