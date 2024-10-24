using Core.Application.Abstractions;
using Core.Application.DTOs;
using MediatR; 
using Microsoft.AspNetCore.Mvc;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Orders.Application.DTOs;
using Orders.Application.Handlers.Commands.CreateOrder;
using Orders.Application.Handlers.Queries.GetOrders;
using System.Net;

namespace Orders.Api.Apis;

/// <summary>
/// Orders Api.
/// </summary>

public class OrdersApi : IApi
{
    const string Tag = "Orders";

    private string _apiUrl = default!;

    /// <summary>
    /// Register orders apis.
    /// </summary>
    /// <param name="app">App.</param>
    /// <param name="baseApiUrl">Base url for apis.</param>
    public void Register(WebApplication app, string baseApiUrl)
    {
        _apiUrl = $"{baseApiUrl}/{Tag}";

        #region Queries

        app.MapGet($"{_apiUrl}/{{id}}", GetOrders)
            .WithTags(Tag)
            .WithOpenApi()
            .WithSummary("Get orders")
            .Produces<GetOrderDto[]>();

        #endregion

        #region Command

        app.MapPost(_apiUrl, PostOrder)
            .WithTags(Tag)
            .Produces<GetOrderDto>((int)HttpStatusCode.Created)
            .WithOpenApi()
            .WithSummary("Create order");

        #endregion
    }

    private static async Task<GetOrderDto[]> GetOrders([FromServices] IMediator mediator, [AsParameters] GetOrdersQuery query, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(query, cancellationToken);

        return result.Items;
    }

    private async Task<IResult> PostOrder([FromServices] IMediator mediator, [FromBody] CreateOrderCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Results.Created($"{_apiUrl}/{{}}", result);
    }
}