namespace Accounting;

class CheckingAccount : Account
{
    public static string CreateAccount(string sessionId)
    {
        User? user = User.VerifySession(sessionId);
        if (user is null)
            throw new Exception("Session invalid");

        if (accounts.Any(acc =>
            IsOwner(sessionId, acc.address) &&
            IsType(acc.address, "CHECKING")))
            throw new Exception("User already has checking account");

        CheckingAccount account = new CheckingAccount(user);

        return account.address;
    }

    public CheckingAccount(User owner) : base(owner, "CHECKING")
    {
        DatabaseHandler.SaveAccount(id, address, balance, owner.id, "CHECKING", null);
    }
    
    public CheckingAccount(int id, string address, double balance, int ownerId)
    : base(id, address, balance, ownerId, "CHECKING") {}

    public static void Transfer(
        string sessionId,
        string addressFrom,
        string addressTo,
        double amount)
    {
        const double MIN_TRANSFER = 5.0;

        if (!IsOwner(sessionId, addressFrom))
            throw new Exception("Unauthorized");

        if (!IsType(addressFrom, "CHECKING"))
            throw new Exception("Account from not checkings account");

        if (!IsType(addressTo, "CHECKING"))
            throw new Exception("Account to not checkings account");

        if (GetAccountByAddress(addressTo) is null)
            throw new Exception("Account that was tried to transfer to doesn't exist");

        string formattedAmount = CurrencyFormatter.Format(amount, currency);
        if (amount < MIN_TRANSFER)
        {
            string formattedMin = CurrencyFormatter.Format(MIN_TRANSFER, currency);
            throw new Exception($"Transfered amount {formattedAmount} less than the minimum amount of {formattedMin}");
        }

        double balance = GetBalance(sessionId, addressFrom);
        if (amount > balance)
            throw new Exception($"Transfered amount {formattedAmount} bigger than balance");

        DepositTo(addressTo, amount);
        Withdraw(sessionId, addressFrom, amount);
    }
}