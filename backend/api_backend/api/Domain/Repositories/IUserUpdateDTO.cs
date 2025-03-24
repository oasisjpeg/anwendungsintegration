namespace WebApplication1.Domain.Repositories
{
    public interface IUserUpdateDto
    {
        string? Name { get; set; }
        string? Email { get; set; }
        string CurrentPasswordHash { get; set; }
        string? NewPasswordHash { get; set; }

    }
}
