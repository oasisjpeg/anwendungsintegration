using WebApplication1.Application_Layer.DTO.User;

public class LeaderboardResponseDto
{
    public List<LeaderboardDto> Leaderboard { get; set; }
    public int CurrentUserScore { get; set; }
}