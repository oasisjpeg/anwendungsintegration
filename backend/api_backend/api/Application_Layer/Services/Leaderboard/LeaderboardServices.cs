using WebApplication1.Application_Layer.DTO.User;
using WebApplication1.Domain.Services;

namespace WebApplication1.Application_Layer.Services.Leaderboard
{
    public class LeaderboardServices : ILeaderboardServices
    {
        private ILeaderboardRepository _leaderboardRepository;
        public LeaderboardServices(ILeaderboardRepository leaderboardRepository)
        {
            _leaderboardRepository = leaderboardRepository;
        }
        public async Task<(List<LeaderboardDto> Leaderboard, int CurrentUserScore)> GetLeaderboardForUser(Guid userId)
        {
            var PointIncreaseCurrentUser = await _leaderboardRepository.RecentPointIncreaseCurrentUser(userId);
            var ListOfLeaderboardTuples = await _leaderboardRepository.GetLeaderboardForOneUser(userId, PointIncreaseCurrentUser);

            var dtoList = ListOfLeaderboardTuples
                .Select(tuple => new LeaderboardDto
                {
                    UserName = tuple.Item1,
                    PointIncreaseValue = tuple.Item2
                })
                .ToList();

            return (dtoList, PointIncreaseCurrentUser);
        }
    }
}
