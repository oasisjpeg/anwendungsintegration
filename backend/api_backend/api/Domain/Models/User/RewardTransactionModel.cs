namespace WebApplication1.Domain.Models.User
{
    public class RewardTransactionModel
    {
        public required int TransactionId { get; set; }
        public required DateTime Created {  get; set; }
        public required int PointsGained { get; set; }
        // both not FK but used to recollect the information related to user andpoint histroy/origin of points
        public required Enum PointSourceType { get; set; }
        public required int PointSourceId { get; set; }
        // fk
        public required Guid UserId { get; set; }
        // navigation prop
        public required UserModel User { get; set; }
    }
}
