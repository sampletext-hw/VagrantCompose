namespace Models.Internals;

public class OrderPostProcessItem
{
    public OrderPostProcessItem(long creatorId, long orderId)
    {
        CreatorId = creatorId;
        OrderId = orderId;
    }

    public long CreatorId { get; set; }

    public long OrderId { get; set; }
}