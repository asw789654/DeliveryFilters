using Core.Application.Abstractions.Mappings;
using Orders.Domain;

namespace Orders.Application.DTOs;

public class GetOrderDto : IMapFrom<Order>
{
    public Guid OrderId { get; set; }

    public int RegionId { get; set; }

    public DateTime DeliveringDateTime { get; set; }

}