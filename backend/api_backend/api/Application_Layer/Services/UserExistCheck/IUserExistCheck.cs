namespace WebApplication1.Application_Layer.Services.UserExistCheck
{
    public interface IUserExistCheck
    {
        Task<bool> UserExistsAsync(string email);
    }
}
