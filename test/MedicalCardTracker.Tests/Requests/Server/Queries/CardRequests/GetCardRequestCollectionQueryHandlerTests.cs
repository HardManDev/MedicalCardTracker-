// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using AutoMapper;
using MedicalCardTracker.Application.Models.ViewModels;
using MedicalCardTracker.Application.Queries.CardRequests.GetCardRequestCollection;
using MedicalCardTracker.Server.Application.Interfaces;
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
            JsonConvert.SerializeObject(DbContext.CardRequests.ToList()));
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
            JsonConvert.SerializeObject(DbContext.CardRequests.ToList()));
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
            JsonConvert.SerializeObject(DbContext.CardRequests.Take(2).ToList()));
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
            JsonConvert.SerializeObject(DbContext.CardRequests.Skip(100).ToList()));
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
            JsonConvert.SerializeObject(DbContext.CardRequests.Skip(2).Take(2).ToList()));
    }
}
