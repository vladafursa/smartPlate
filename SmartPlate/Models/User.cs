namespace SmartPlate.Models;

//class representing system user
public class User
{
    //private constructor to prevent direct instantiation with "new"
    private User(Guid id, string userName, string passwordHash, string email, string role)
    {
        Id = id;
        UserName = userName;
        PasswordHash = passwordHash;
        Email = email;
        Role = role;
    }

    // // Unique identifier
    public Guid Id { get; set; }

    //Publicly readable, but only settable inside this class.
    public string UserName { get; private set; }

    public string PasswordHash { get; private set; }

    public string Email { get; private set; }

    // Role property with default user role
    public string Role { get; private set; } = "User";

    // method for creating a new user instance.
    public static User Create(Guid id, string userName, string passwordHash, string email, string role = "User")
    {
        return new User(id, userName, passwordHash, email, role);
    }
}
