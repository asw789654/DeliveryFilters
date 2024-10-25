using AutoFixture;
using AutoMapper;
using Core.Application.Abstractions.Persistence.Repository.Read;
using Core.Application.DTOs;
using Core.Tests;
using Core.Tests.Fixtures;
using MediatR;
using Moq;
using Orders.Domain;
using Orders.Application.DTOs;
using Orders.Application.Handlers.Queries.GetOrders;
using System.Linq.Expressions;
using Xunit.Abstractions;

namespace Orders.UnitTests.Tests.Queries.GetOrder;

public class GetOrdersQueryHandlerTests : RequestHandlerTestBase<GetOrdersQuery, BaseListDto<GetOrderDto>>
{
    private readonly Mock<IBaseReadRepository<Order>> _ordersMok = new();

    private readonly IMapper _mapper;

    public GetOrdersQueryHandlerTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
        _mapper = new AutoMapperFixture(typeof(GetOrdersQuery).Assembly).Mapper;
    }

    protected override IRequestHandler<GetOrdersQuery, BaseListDto<GetOrderDto>> CommandHandler =>
        new GetOrdersQueryHandler(_ordersMok.Object, _mapper);

    [Fact]
    public async Task Should_BeValid_When_GetOrders()
    {

        var query = new GetOrdersQuery();

        var orders = TestFixture.Build<Order>().CreateMany(10).ToArray();
        var count = orders.Length;

        _ordersMok.Setup(
            p => p.AsAsyncRead().ToArrayAsync(It.IsAny<Expression<Func<Order, bool>>>(), default)
        ).ReturnsAsync(orders);

        _ordersMok.Setup(
            p => p.AsAsyncRead().CountAsync(It.IsAny<Expression<Func<Order, bool>>>(), default)
        ).ReturnsAsync(count);

        // act and assert
        await AssertNotThrow(query);
    }
}