namespace WebApplication1.Application_Layer.Services.UserAuth
{
    public interface IUserAuth
    {
        string HashPassword(string password);
        bool VerifyPassword(string hashedPassword, string providedPassword);
        Guid GetUserIdGuidFromClaims(string userIdString);
    }
}
