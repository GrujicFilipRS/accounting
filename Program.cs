namespace Accounting
{
    class Entrypoint
    {
        public static void Main(string[] args)
        {
            string username = "ficfiric";
            string password = "test123";
            string fullName = "Filip Grujic";

            User user = new User(username, password, fullName);
            user.PrintUser();
        }
    }
}