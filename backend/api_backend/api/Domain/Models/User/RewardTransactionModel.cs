namespace WebApplication1.Domain.Models.User
{
    public class RewardTransactionModel
    {
        public required int id { get; set; }
        public required DateTime Created {  get; set; }
        public required int PointsGained { get; set; }
        // both not FK but used to recollect the information related to user andpoint histroy/origin of points
        public required PointSourceType PointSourceType { get; set; }
        // public required int PointSourceId { get; set; } --> I dont see this as relevant anymore (author: Jacob)
        // fk
        public required Guid UserId { get; set; }
        // navigation prop
        // public required UserModel User { get; set; } --> removed because of initialization issues when creating new RewardTransactionModel instances
    }
}
