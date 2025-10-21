namespace WebApplication1.Application_Layer.DTO.User;

public class LeaderboardResponseDto
{
    public required List<LeaderboardDto> Leaderboard { get; set; }
    public int CurrentUserScore { get; set; }
}