namespace Accounting;

class User
{
    private static List<User> users = new List<User>();

    private static string Hash(string password)
    {
        const int WORK_FACTOR = 8; // Number of iterations of the BCrypt algorithm
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password, WORK_FACTOR);
    }

    private static bool InvalidParams(string username, string password, string fullName)
    {
        const int USERNAME_LOWER_BOUND = 5, USERNAME_UPPER_BOUND = 15;
        const int PASSWORD_LOWER_BOUND = 5, PASSWORD_UPPER_BOUND = 25;
        const int FULLNAME_LOWER_BOUND = 10, FULLNAME_UPPER_BOUND = 45;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            return true;

        if (username.Length < USERNAME_LOWER_BOUND || username.Length > USERNAME_UPPER_BOUND)
            return true;

        if (password.Length < PASSWORD_LOWER_BOUND || password.Length > PASSWORD_UPPER_BOUND)
            return true;

        if (fullName.Length < FULLNAME_LOWER_BOUND || fullName.Length > FULLNAME_UPPER_BOUND)
            return true;

        return false;
    }

    public static string Register(string username, string password, string fullName)
    {
        if (InvalidParams(username, password, fullName))
            throw new Exception("Invalid registration parameters");

        if (users.Any(user => user.username == username))
            throw new Exception("User already exists with that username");

        User newUser = new User(username, password, fullName);

        return newUser.sessionId;
    }

    public static string Login(string username, string password)
    {
        IEnumerable<User> possibleUsers = users.Where(user => user.username == username && user.ComparePassword(password));
        if (possibleUsers.Count() == 0)
            throw new Exception("Invalid credentials");

        User foundUser = possibleUsers.First();

        return foundUser.sessionId;
    }

    public static User? VerifySession(string sessionId)
    {
        IEnumerable<User> possibleUsers = users.Where(user => user.sessionId == sessionId);
        if (possibleUsers.Count() == 0)
            return null;

        User foundUser = possibleUsers.First();
        return foundUser;
    }

    private int id;
    private string sessionId; // Used for handling actions taken by user
    private string username, hashedPassword;
    private string fullName;

    public User(string username, string password, string fullName)
    {
        id = users.Count + 1;
        sessionId = Hash(id.ToString());
        this.username = username;
        this.fullName = fullName;

        // Hashing the password, because it will be stored in the db later
        hashedPassword = Hash(password);

        users.Add(this);
    }

    private bool ComparePassword(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
    }
}