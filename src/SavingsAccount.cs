namespace Accounting;

class SavingsAccount : Account
{
    public static string CreateAccount(string sessionId)
    {
        User? user = User.VerifySession(sessionId);
        if (user is null)
            throw new Exception("Session invalid");
        
        if (accounts.Any(acc =>
            IsOwner(sessionId, acc.address) &&
            IsType(acc.address, "CHECKING")))
            throw new Exception("User already has savings account");

        SavingsAccount account = new SavingsAccount(user);

        return account.address;
    }

    public static void ApplyInterest(string sessionId, string address)
    {
        if (!IsOwner(sessionId, address))
            throw new Exception("User isn't account owner");

        Account account = GetAccountByAddress(address)!;
        if (!(account is SavingsAccount savingsAccount))
            throw new Exception("Attempted to apply interest to a non-savings account");

        savingsAccount.ApplyInterest();
    }

    public static void WithdrawToChecking(string sessionId, string address, double amount)
    {
        const double MIN_WITHDRAW_TO_CHECKING = 5.0;

        if (!IsOwner(sessionId, address))
            throw new Exception("User isn't account owner");

        Account account = GetAccountByAddress(address)!;
        if (!(account is SavingsAccount savingsAccount))
            throw new Exception("Attempted to withdraw to checkings from a non-savings account");

        if (amount < MIN_WITHDRAW_TO_CHECKING)
            throw new Exception($"The minimum amount to withdraw to checking is {MIN_WITHDRAW_TO_CHECKING}. You submitted {amount}");

        if (amount > savingsAccount.balance)
            throw new Exception($"The amount to withdraw {CurrencyFormatter.Format(amount, currency)} is greater than savings account balance");

        CheckingAccount? checkingAccount = accounts.SingleOrDefault(acc => acc.IsType("CHECKING") && IsOwner(sessionId, acc.address)) as CheckingAccount;

        if (checkingAccount is null)
            throw new Exception("User doesn't have checkings account");

        string checkingAddr = checkingAccount.address;

        Deposit(sessionId, checkingAddr, amount);
        Withdraw(sessionId, address, amount);
    }

    public static void DepositFromChecking(string sessionId, string address, double amount)
    {
        const double MIN_DEPOSIT_FROM_CHECKING = 5.0;

        if (!IsOwner(sessionId, address))
            throw new Exception("User isn't account owner");

        Account account = GetAccountByAddress(address)!;
        if (!(account is SavingsAccount savingsAccount))
            throw new Exception("Attempted to deposit from checkings to a non-savings account");

        if (amount < MIN_DEPOSIT_FROM_CHECKING)
            throw new Exception($"The minimum amount to withdraw to checking is {MIN_DEPOSIT_FROM_CHECKING}. You submitted {amount}");

        CheckingAccount? checkingAccount = accounts.SingleOrDefault(acc => acc.IsType("CHECKING") && IsOwner(sessionId, acc.address)) as CheckingAccount;

        if (checkingAccount is null)
            throw new Exception("User doesn't have checkings account");

        if (amount > GetBalance(sessionId, address))
            throw new Exception($"The amount to withdraw {CurrencyFormatter.Format(amount, currency)} is greater than checking account balance");

        string checkingAddr = checkingAccount.address;

        Withdraw(sessionId, checkingAddr, amount);
        Deposit(sessionId, address, amount);
    }

    public static string? GetAccountBySession(string session)
    {
        SavingsAccount? account = accounts.FirstOrDefault(
            acc => IsOwner(session, acc.address)
            && IsType(acc.address, "SAVINGS")
        ) as SavingsAccount;

        if (account is null) return null;
        return account.address;
    }

    public static bool CheckIfUserHasAccount(string session)
    {
        return accounts.FirstOrDefault(
            acc => IsOwner(session, acc.address)
            && IsType(acc.address, "SAVINGS")
        ) is not null;
    }

    private const double INTEREST_PERCENTAGE = 5; // per minute, for testing purposes
    private DateTime lastInterestPayment;

    public SavingsAccount(User owner) : base(owner, "SAVINGS")
    {
        lastInterestPayment = DateTime.Now;
        DatabaseHandler.SaveAccount(id, address, balance, owner.id, "SAVINGS", lastInterestPayment);
    }

    public SavingsAccount(int id, string address, double balance, int ownerId, DateTime lastInterestPayment)
    : base(id, address, balance, ownerId, "SAVINGS")
    {
        this.lastInterestPayment = lastInterestPayment;
    }

    private void ApplyInterest()
    {
        TimeSpan timeElapsed = DateTime.Now - lastInterestPayment;
        double minutesPassed = timeElapsed.TotalMinutes;

        if (minutesPassed < 1)
            return;

        const double RATE = INTEREST_PERCENTAGE / 100.0;

        double multiplier = Math.Pow(1 + RATE, Math.Floor(minutesPassed));

        balance *= multiplier;

        lastInterestPayment = lastInterestPayment.AddHours(Math.Floor(minutesPassed));

        DatabaseHandler.InterestApplication(id, balance, lastInterestPayment);
    }
}