namespace Accounting
{
    class SavingsAccount : Account
    {
        public static string CreateAccount(string sessionId)
        {
            User? user = User.VerifySession(sessionId);
            if (user is null)
                throw new Exception("Session invalid");

            SavingsAccount account = new SavingsAccount(user);

            return account.address;
        }

        public SavingsAccount(User owner) : base(owner, "SAVINGS") { }
    }
}