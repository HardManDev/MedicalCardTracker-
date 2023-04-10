// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using AutoMapper;
using MedicalCardTracker.Application.Models.ViewModels;
using MedicalCardTracker.Application.Queries.CardRequests.GetCardRequestCollection;
using MedicalCardTracker.Server.Application.Interfaces;
using MedicalCardTracker.Server.Application.Models.Enums;
using MedicalCardTracker.Server.Application.Queries.CardRequests.GetCardRequestCollection;
using Newtonsoft.Json;
using Xunit;

namespace MedicalCardTracker.Tests.Requests.Server.Queries.CardRequests;

public class GetCardRequestCollectionQueryHandlerTests : BaseTestsRequestHandler
{
    public GetCardRequestCollectionQueryHandlerTests(IApplicationDbContext dbContext, IMapper mapper)
        : base(dbContext, mapper)
    {
    }

    [Fact]
    public async Task GetCardRequestCollectionQueryHandler_Success()
    {
        var handler = new GetCardRequestCollectionQueryHandler(DbContext, Mapper);

        var result = await handler.Handle(
            new GetCardRequestCollectionQuery
            {
                Page = 0,
                Count = 3
            }, CancellationToken.None);

        Assert.IsType<CardRequestCollectionVm>(result);
        Assert.True(result is { TotalCount: 3, CardRequests.Count: 3 });
        Assert.Equal(JsonConvert.SerializeObject(result.CardRequests),
            JsonConvert.SerializeObject(DbContext.CardRequests
                .OrderBy(item => item.CreatedAt)
                .ToList()));
    }

    [Fact]
    public async Task GetCardRequestCollectionQueryHandler_SuccessWithNoParams()
    {
        var handler = new GetCardRequestCollectionQueryHandler(DbContext, Mapper);

        var result = await handler.Handle(
            new GetCardRequestCollectionQuery(), CancellationToken.None);

        Assert.IsType<CardRequestCollectionVm>(result);
        Assert.True(result is { TotalCount: 3, CardRequests.Count: 3 });
        Assert.Equal(JsonConvert.SerializeObject(result.CardRequests),
            JsonConvert.SerializeObject(DbContext.CardRequests
                .OrderBy(item => item.CreatedAt)
                .ToList()));
    }

    [Fact]
    public async Task GetCardRequestCollectionQueryHandler_SuccessWithCount()
    {
        var handler = new GetCardRequestCollectionQueryHandler(DbContext, Mapper);

        var result = await handler.Handle(
            new GetCardRequestCollectionQuery
            {
                Count = 2
            }, CancellationToken.None);

        Assert.IsType<CardRequestCollectionVm>(result);
        Assert.True(result is { TotalCount: 3, CardRequests.Count: 2 });
        Assert.Equal(JsonConvert.SerializeObject(result.CardRequests),
            JsonConvert.SerializeObject(DbContext.CardRequests
                .OrderBy(item => item.CreatedAt)
                .Take(2)
                .ToList()));
    }

    [Fact]
    public async Task GetCardRequestCollectionQueryHandler_SuccessWithPage()
    {
        var handler = new GetCardRequestCollectionQueryHandler(DbContext, Mapper);

        var result = await handler.Handle(
            new GetCardRequestCollectionQuery
            {
                Page = 1
            }, CancellationToken.None);

        Assert.IsType<CardRequestCollectionVm>(result);
        Assert.True(result is { TotalCount: 3, CardRequests.Count: 0 });
        Assert.Equal(JsonConvert.SerializeObject(result.CardRequests),
            JsonConvert.SerializeObject(DbContext.CardRequests
                .OrderBy(item => item.CreatedAt)
                .Skip(100)
                .ToList()));
    }

    [Fact]
    public async Task GetCardRequestCollectionQueryHandler_SuccessWithPageAndCount()
    {
        var handler = new GetCardRequestCollectionQueryHandler(DbContext, Mapper);

        var result = await handler.Handle(
            new GetCardRequestCollectionQuery
            {
                Page = 1,
                Count = 2
            }, CancellationToken.None);

        Assert.IsType<CardRequestCollectionVm>(result);
        Assert.True(result is { TotalCount: 3, CardRequests.Count: 1 });
        Assert.Equal(JsonConvert.SerializeObject(result.CardRequests),
            JsonConvert.SerializeObject(DbContext.CardRequests
                .OrderBy(item => item.CreatedAt)
                .Skip(2)
                .Take(2)
                .ToList()));
    }

    [Fact]
    public async Task GetCardRequestCollectionQueryHandler_SuccessWithTargetAddress()
    {
        var handler = new GetCardRequestCollectionQueryHandler(DbContext, Mapper);

        var result = await handler.Handle(
            new GetCardRequestCollectionQuery
            {
                TargetAddress = "cab. 200"
            }, CancellationToken.None);

        Assert.IsType<CardRequestCollectionVm>(result);
        // "CardRequests.Count: 1" because at this point in this test, one record from the database will already be deleted.
        Assert.True(result
            is { TotalCount: 3, CardRequests.Count: 1 }
            or { TotalCount: 3, CardRequests.Count: 2 });
        Assert.Equal(JsonConvert.SerializeObject(result.CardRequests),
            JsonConvert.SerializeObject(DbContext.CardRequests
                .Where(item =>
                    item.TargetAddress == "cab. 200")
                .OrderBy(item => item.CreatedAt)
                .ToList()));
    }

    [Fact]
    public async Task GetCardRequestCollectionQueryHandler_SuccessWithOrderBy()
    {
        var handler = new GetCardRequestCollectionQueryHandler(DbContext, Mapper);

        var result = await handler.Handle(
            new GetCardRequestCollectionQuery
            {
                OrderBy = OrderBy.Descending
            }, CancellationToken.None);

        Assert.IsType<CardRequestCollectionVm>(result);
        Assert.True(result is { TotalCount: 3 });
        Assert.Equal(JsonConvert.SerializeObject(result.CardRequests),
            JsonConvert.SerializeObject(DbContext.CardRequests
                .OrderByDescending(item => item.CreatedAt)
                .ToList()));
    }

    [Fact]
    public async Task GetCardRequestCollectionQueryHandler_SuccessWithAll()
    {
        var handler = new GetCardRequestCollectionQueryHandler(DbContext, Mapper);

        var result = await handler.Handle(
            new GetCardRequestCollectionQuery
            {
                Page = 1,
                Count = 1,
                OrderBy = OrderBy.Descending,
                TargetAddress = "cab. 200"
            }, CancellationToken.None);

        Assert.IsType<CardRequestCollectionVm>(result);
        Assert.True(result is { TotalCount: 3 });
        Assert.Equal(JsonConvert.SerializeObject(result.CardRequests),
            JsonConvert.SerializeObject(DbContext.CardRequests
                .Where(item =>
                    item.TargetAddress == "cab. 200")
                .OrderByDescending(item => item.CreatedAt)
                .Skip(1)
                .Take(1)
                .ToList()));
    }
}
