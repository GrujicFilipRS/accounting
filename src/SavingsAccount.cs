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
            if (!IsOwner(sessionId, address))
                throw new Exception("User isn't account owner");

            Account account = Account.GetAccountByAddress(address)!;
            if (!(account is SavingsAccount))
                throw new Exception("Attempted to apply interest to a non-savings account");

            SavingsAccount savingsAccount = (account as SavingsAccount)!;
            savingsAccount.ApplyInterest();
        }

        private const double INTEREST_PERCENTAGE = 50; // per minute, for testing purposes
        private DateTime lastInterestPayment;

        public SavingsAccount(User owner) : base(owner, "SAVINGS")
        {
            lastInterestPayment = DateTime.Now;
        }

        private void ApplyInterest()
        {
            TimeSpan timeElapsed = DateTime.Now - lastInterestPayment;
            double hoursPassed = timeElapsed.TotalMinutes;
            Console.WriteLine(hoursPassed);

            if (hoursPassed < 1)
                return;

            const double RATE = INTEREST_PERCENTAGE / 100.0;

            double multiplier = Math.Pow(1 + RATE, Math.Floor(hoursPassed));

            balance *= multiplier;

            lastInterestPayment = lastInterestPayment.AddHours(Math.Floor(hoursPassed));
        }
    }
}