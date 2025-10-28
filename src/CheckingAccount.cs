namespace Accounting
{
    class CheckingAccount : Account
    {
        public static string CreateAccount(string sessionId)
        {
            User? user = User.VerifySession(sessionId);
            if (user is null)
                throw new Exception("Session invalid");

            CheckingAccount account = new CheckingAccount(user);

            return account.address;
        }

        public CheckingAccount(User owner) : base(owner, "CHECKING") { }
    }
}