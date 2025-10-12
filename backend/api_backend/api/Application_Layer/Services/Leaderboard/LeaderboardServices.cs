using WebApplication1.Application_Layer.DTO.User;
using WebApplication1.Domain.Repositories;
using WebApplication1.Domain.Services;

namespace WebApplication1.Application_Layer.Services.Leaderboard
{
    public class LeaderboardServices : ILeaderboardServices
    {
        private readonly ILeaderboardRepository _leaderboardRepository;

        public LeaderboardServices(ILeaderboardRepository leaderboardRepository)
        {
            _leaderboardRepository = leaderboardRepository;

        }
        public async Task<(List<LeaderboardDto> Leaderboard, int CurrentUserScore)> GetLeaderboardForUser(Guid userId)
        {
            var pointIncreaseCurrentUser = await _leaderboardRepository.RecentPointIncreaseCurrentUser(userId);
            var listOfLeaderboardTuples = await _leaderboardRepository.GetLeaderboardForOneUser(userId, pointIncreaseCurrentUser);

            var dtoList = listOfLeaderboardTuples
                .Select(tuple => new LeaderboardDto
                {
                    UserName = tuple.Item1,
                    PointIncreaseValue = tuple.Item2 // Consider renaming if this is total score
                })
                .ToList();

            return (dtoList, pointIncreaseCurrentUser);
        }

    }
}
