using AutoFixture;
using Core.Tests;
using Core.Tests.Attributes;
using FluentValidation;
using Orders.Application.Handlers.Queries.GetOrders;
using Xunit.Abstractions;

namespace Orders.UnitTests.Tests.Queries.GetOrder;

public class GetOrdersQueryValidatorTests : ValidatorTestBase<GetOrdersQuery>
{
    public GetOrdersQueryValidatorTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
    }

    protected override IValidator<GetOrdersQuery> TestValidator =>
        TestFixture.Create<GetOrdersQueryValidator>();

    [Fact]
    public void Should_BeValid_When_RequestIsValid()
    {
        // arrange
        var query = new GetOrdersQuery
        {
            Region = 1,
            NumberOfOrders = 5,
            StartDateTime = new DateTime(2024, 10, 24, 12, 26, 02),
            EndDateTime = new DateTime(2024, 10, 25, 12, 26, 02)

        };

        // act & assert
        AssertValid(query);
    }

    [Theory]
    [FixtureInlineAutoData(1)]
    [FixtureInlineAutoData(2)]
    [FixtureInlineAutoData(3)]
    public void Should_BeValid_When_RegionIsValid(int region)
    {
        // arrange
        var query = new GetOrdersQuery
        {
            Region = region,
        };

        // act & assert
        AssertValid(query);
    }

    [Theory]
    [FixtureInlineAutoData(0)]
    [FixtureInlineAutoData(-1)]
    [FixtureInlineAutoData(-4)]
    public void Should_NotBeValid_When_IncorrectRegion(int region)
    {
        // arrange
        var query = new GetOrdersQuery
        {
            Region = region,
        };

        // act & assert
        AssertNotValid(query);
    }

    [Theory]
    [FixtureInlineAutoData(-5)]
    [FixtureInlineAutoData(-1)]
    public void Should_NotBeValid_When_IncorrectNumberOfOrders(int numberOfOrders)
    {
        // arrange
        var query = new GetOrdersQuery
        {
            NumberOfOrders = numberOfOrders,
        };

        // act & assert
        AssertNotValid(query);
    }
}