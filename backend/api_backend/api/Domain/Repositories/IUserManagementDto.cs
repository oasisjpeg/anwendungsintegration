namespace WebApplication1.Domain.Repositories
{
    public interface IUserManagementDto
    {
        public string? Id { get; set; }
        string CurrentPasswordHash { get; set; }

    }
}
