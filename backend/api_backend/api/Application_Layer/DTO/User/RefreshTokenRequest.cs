namespace WebApplication1.Application_Layer.DTO.User
{
    public class RefreshTokenRequest
    {
        public required string Token { get; set; }
        public required string RefreshToken { get; set; }
    }
}