using Core.Application.DTOs;
using MediatR;
using Orders.Application.DTOs;

namespace Orders.Application.Handlers.Queries.GetOrders;

public class GetOrdersQuery : IRequest<BaseListDto<GetOrderDto>>
{
    public int? Region { get; init; }

    public DateTime? StartDateTime { get; init; }

    public DateTime? EndDateTime { get; init; }

    public int? NumberOfOrders { get; set; }

}