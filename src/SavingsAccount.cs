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

        public static void ApplyInterest(string sessionId, string address)
        {
            if (!IsOwner(address, sessionId))
                throw new Exception("User isn't account owner");

            Account account = Account.GetAccountByAddress(address)!;
            if (!(account is SavingsAccount))
                throw new Exception("Attempted to apply interest to a non-savings account");
            
            
        }

        private const double INTEREST_PERCENTAGE = 2.5; // per minute, for testing purposes
        private DateTime lastInterestPayment;
        public SavingsAccount(User owner) : base(owner, "SAVINGS")
        {
            lastInterestPayment = DateTime.Now;
        }
    }
}