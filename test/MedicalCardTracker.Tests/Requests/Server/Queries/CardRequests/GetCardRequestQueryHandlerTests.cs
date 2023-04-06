// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using AutoMapper;
using MedicalCardTracker.Application.Models.ViewModels;
using MedicalCardTracker.Application.Queries.CardRequests.GetCardRequest;
using MedicalCardTracker.Server.Application.Exceptions;
using MedicalCardTracker.Server.Application.Interfaces;
using MedicalCardTracker.Server.Application.Queries.CardRequests.GetCardRequest;
using MedicalCardTracker.Tests.Fixtures;
using Newtonsoft.Json;
using Xunit;

namespace MedicalCardTracker.Tests.Requests.Server.Queries.CardRequests;

public class GetCardRequestQueryHandlerTests : BaseTestsRequestHandler
{
    public GetCardRequestQueryHandlerTests(IApplicationDbContext dbContext, IMapper mapper)
        : base(dbContext, mapper)
    {
    }

    [Fact]
    public async Task GetCardRequestQueryHandler_Success()
    {
        var handler = new GetCardRequestQueryHandler(DbContext, Mapper);

        var result = await handler.Handle(
            new GetCardRequestQuery
            {
                Id = FixtureCardRequests.FixtureCardRequest.Id
            }, CancellationToken.None);

        Assert.IsType<CardRequestVm>(result);
        Assert.Equal(JsonConvert.SerializeObject(result),
            JsonConvert.SerializeObject(Mapper.Map<CardRequestVm>(DbContext.CardRequests.FirstOrDefault(
                item => item.Id == FixtureCardRequests.FixtureCardRequest.Id))));
    }

    [Fact]
    public async Task GetCardRequestQueryHandler_FailOnWrongId()
    {
        var handler = new GetCardRequestQueryHandler(DbContext, Mapper);

        await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
        {
            await handler.Handle(
                new GetCardRequestQuery
                {
                    Id = Guid.Empty
                }, CancellationToken.None);
        });
    }
}
