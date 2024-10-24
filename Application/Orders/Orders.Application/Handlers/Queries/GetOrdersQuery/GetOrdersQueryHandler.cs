using AutoMapper;
using Core.Application.Abstractions.Persistence.Repository.Read;
using Core.Application.DTOs;
using MediatR;
using Orders.Application.DTOs;
using Orders.Domain;

namespace Orders.Application.Handlers.Queries.GetOrders;

public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, BaseListDto<GetOrderDto>>
{
    private readonly IBaseReadRepository<Order> _orders;

    private readonly IMapper _mapper;

    public GetOrdersQueryHandler(
        IBaseReadRepository<Order> orders,
        IMapper mapper)
    {
        _orders = orders;
        _mapper = mapper;
    }

    public async Task<BaseListDto<GetOrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var query = _orders.AsQueryable();

        query = query.OrderBy(e => e.DeliveringDateTime);

        if (request.StartDateTime.HasValue)
        {
            query = query.Where(x => x.DeliveringDateTime > request.StartDateTime);
        }

        if (request.EndDateTime.HasValue)
        {
            query = query.Where(x => x.DeliveringDateTime > request.EndDateTime);
        }

        if (request.NumberOfOrders.HasValue)
        {
            query = query.Take(request.NumberOfOrders.Value);
        }

        var firstItem = query.FirstOrDefault();

        if (request.Region.HasValue)
        {
            query = query.Where(x => x.RegionId == request.Region.Value);
        }
        // Ќе уверен нужно ли фильтр 30 минут от первого заказа ,если есть фильтр по времени, при его ненужности достаточно закоментить строку ниже
        query = query.Where(e => e.DeliveringDateTime <= firstItem.DeliveringDateTime.AddMinutes(30) && e.DeliveringDateTime >= firstItem.DeliveringDateTime);

        var items = await _orders.AsAsyncRead().ToArrayAsync(query, cancellationToken);

        var totalCount = await _orders.AsAsyncRead().CountAsync(query, cancellationToken);

        using (StreamWriter writer = new StreamWriter("_deliveryOrder.txt", false))
        {
            foreach (var item in items)
            {
                await writer.WriteLineAsync(new string('-', 20));
                await writer.WriteLineAsync($"OrderId - {item.OrderId.ToString()}");
                await writer.WriteLineAsync($"Weight - {Convert.ToString(item.Weight)}");
                await writer.WriteLineAsync($"DeliveringDateTime - {Convert.ToString(item.DeliveringDateTime)}");
            }
        }

        return new BaseListDto<GetOrderDto>
        {
            TotalCount = totalCount,
            Items = _mapper.Map<GetOrderDto[]>(items)
        };
    }
}