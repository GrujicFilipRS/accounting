namespace Accounting
{
    abstract class Account
    {
        private static List<Account> accounts = new List<Account>();

        private static string HexHash(int id) {
            // Write a hash function that gives a hexadecimal value
            return id.ToString();
        }

        private int accountID;
        private string accountAddress;
        private double balance;
        private User owner;

        // public Account(User owner)
        // {
        //     accountID = accounts.Count() + 1;
        //     // accountAddress = 
        // }
    }
}