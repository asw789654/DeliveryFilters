using AutoMapper;
using Core.Application.Abstractions.Persistence.Repository.Writing;
using MediatR;
using Orders.Application.DTOs;
using Orders.Domain;

namespace Orders.Application.Handlers.Commands.CreateOrder;

internal class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, GetOrderDto>
{
    private readonly IBaseWriteRepository<Order> _orders;

    private readonly IMapper _mapper;

    public CreateOrderCommandHandler(
        IBaseWriteRepository<Order> orders,
        IMapper mapper)
    {
        _orders = orders;
        _mapper = mapper;
    }

    public async Task<GetOrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Order
        {
            OrderId = Guid.NewGuid(),

            RegionId = request.RegionId,

            Weight = request.Weight,

            DeliveringDateTime = request.DeliveringDateTime
        };

        order = await _orders.AddAsync(order, cancellationToken);

        return _mapper.Map<GetOrderDto>(order);
    }
}