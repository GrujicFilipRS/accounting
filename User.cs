namespace Accounting
{
    class User
    {
        private static int workFactor = 8; // Number of iterations of the BCrypt algorithm
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password, workFactor);
        }

        public static List<User> users = new List<User>();

        private int id;
        private string username, hashedPassword;
        public string fullName;

        public User(string username, string password, string fullName)
        {
            id = users.Count + 1;
            this.username = username;
            this.fullName = fullName;

            // Hashing the password, because it will be stored in the db later
            hashedPassword = HashPassword(password);

            users.Add(this);
        }

        public void PrintUser()
        {
            Console.WriteLine($"ID: {id}");
            Console.WriteLine($"Username: {username}");
            Console.WriteLine($"Hashed pwd: {hashedPassword}");
            Console.WriteLine($"Full name: {fullName}");
        }
    }
}