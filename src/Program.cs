namespace Accounting
{
    class Entrypoint
    {
        public static void Main(string[] args)
        {
            string username = "ficfiric";
            string password = "test123";
            string fullName = "Filip Grujic";

            string sessionId = User.Register(username, password, fullName);

            string addr = Account.CreateAccount(sessionId);
            Account.Deposit(sessionId, addr, 1000.0);
            Console.WriteLine(Account.GetBalanceFormatted(sessionId, addr));
        }
    }
}