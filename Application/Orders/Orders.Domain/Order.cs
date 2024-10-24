namespace Orders.Domain;

public class Order
{
    public Guid OrderId { get; set; }

    public int RegionId { get; set; }

    public double Weight { get; set; }

    public DateTime DeliveringDateTime { get; set; }

}