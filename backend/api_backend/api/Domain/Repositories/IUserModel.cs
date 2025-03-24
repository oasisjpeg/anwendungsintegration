namespace WebApplication1.Domain.Repositories
{
    public interface IUserModel
    {
        string Id { get; set; }
        string Name { get; set; }
        string Email { get; set; }
        string PasswordHash { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
        string HashPassword(string password);
        bool VerifyPassword(string password);
    }
}
