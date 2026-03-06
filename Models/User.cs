namespace SalesApi.Models;

public class User
{
    public int Id { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public UserRole Role { get; private set; }
    public bool IsActive { get; private set; } = true;
    public ICollection<Product> Product { get; private set; } 

    public User(string email, string passwordHash, UserRole role)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email inválido");
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Senha inválido");

        Email = email;
        PasswordHash = passwordHash;
        Role = role;

    }

    public void Update(string email, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email inválido");
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Senha inválido");

        Email = email;
        PasswordHash = passwordHash;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }

}

public enum UserRole
{
    Admin,
    User
}