using MediatR;
using Orders.Application.DTOs;

namespace Orders.Application.Handlers.Commands.CreateOrder;

public class CreateOrderCommand : IRequest<GetOrderDto>
{
    public int RegionId { get; set; }

    public double Weight { get; set; }

    public DateTime DeliveringDateTime { get; set; }
}