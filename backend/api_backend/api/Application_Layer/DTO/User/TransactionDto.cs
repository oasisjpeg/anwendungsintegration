namespace WebApplication1.Application_Layer.DTO.User
{
    public class TransactionDto
    {
        public required int TransactionId { get; set; }
        public required DateTime Created { get; set; }
        public required int PointsGained { get; set; }
        public required Enum PointSourceType { get; set; }
        public required int PointSourceId { get; set; }
        public required Guid UserId { get; set; } // Fk
    }
}
