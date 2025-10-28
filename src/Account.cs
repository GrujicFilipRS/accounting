using System.Security.Cryptography;
using System.Text;

namespace Accounting
{
    class Account
    {
        protected static List<Account> accounts = new List<Account>();

        protected static string HexHash(int id)
        {
            const int ADDRESS_LENGTH = 24;
            string dataToEncode = id.ToString();

            using (SHA256 hash = SHA256.Create())
            {
                byte[] bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(dataToEncode));

                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString().Substring(0, ADDRESS_LENGTH);
            }
        }

        protected static Account? GetAccountByAddress(string address)
        {
            return accounts.SingleOrDefault(acc => acc.address == address);
        }

        public static string GetBalanceFormatted(string sessionId, string address)
        {
            Account? acc = GetAccountByAddress(address);
            if (acc is null)
                throw new Exception("Account with such address doesn't exist");

            return acc.FormatBalance(sessionId);
        }

        public static void Deposit(string sessionId, string address, double amount)
        {
            Account? acc = GetAccountByAddress(address);
            if (acc is null)
                throw new Exception("Account with such address doesn't exist");

            acc.Deposit(sessionId, amount);
        }
        
        public static bool IsOwner(string sessionId, string address)
        {
            User? user = User.VerifySession(sessionId);

            Account? account = Account.GetAccountByAddress(address);
            if (account is null)
                throw new Exception("Account nonexistent");

            return user == account.owner;
        }

        protected int id;
        protected string address;
        protected double balance;
        protected const string currency = "EUR";
        protected User owner;
        protected string accountType;

        public Account(User owner, string accountType)
        {
            id = accounts.Count() + 1;
            address = HexHash(id);
            balance = 0.0;
            this.owner = owner;
            this.accountType = accountType;

            accounts.Add(this);
        }

        private string FormatBalance(string sessionId)
        {
            User? user = User.VerifySession(sessionId);
            if (user != owner)
                throw new Exception("No authorization");

            return CurrencyFormatter.Format(balance, currency);
        }

        private void Deposit(string sessionId, double amount)
        {
            const double MIN_DEPOSIT = 5.0;

            User? user = User.VerifySession(sessionId);
            if (user != owner)
                throw new Exception("No authorization");

            if (amount < MIN_DEPOSIT)
                throw new Exception("Invalid deposit amount");

            balance += amount;
        }
    }
}