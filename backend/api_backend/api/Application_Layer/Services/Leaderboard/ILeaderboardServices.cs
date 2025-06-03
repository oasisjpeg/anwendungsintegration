using WebApplication1.Application_Layer.DTO.User;

namespace WebApplication1.Application_Layer.Services.Leaderboard
{
    public interface ILeaderboardServices
    {
        public Task<(List<LeaderboardDto> Leaderboard, int CurrentUserScore)> GetLeaderboardForUser(Guid userId);
        // ListOfOtherUserTuples == (string UserName, int usersPointScore)
    }
}
