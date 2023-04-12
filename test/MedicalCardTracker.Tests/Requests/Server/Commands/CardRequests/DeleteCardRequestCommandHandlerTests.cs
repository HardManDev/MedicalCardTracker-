// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using AutoMapper;
using MedicalCardTracker.Application.Requests.Commands.CardRequests.DeleteCardRequest;
using MedicalCardTracker.Server.Application.Commands.CardRequests.DeleteCardRequest;
using MedicalCardTracker.Server.Application.Exceptions;
using MedicalCardTracker.Server.Application.Interfaces;
using MedicalCardTracker.Tests.Fixtures;
using Xunit;

namespace MedicalCardTracker.Tests.Requests.Server.Commands.CardRequests;

public class DeleteCardRequestCommandHandlerTests : BaseTestsRequestHandler
{
    public DeleteCardRequestCommandHandlerTests(IApplicationDbContext dbContext, IMapper mapper)
        : base(dbContext, mapper)
    {
    }

    [Fact]
    public async Task DeleteCardRequestCommandHandler_Success()
    {
        var handler = new DeleteCardRequestCommandHandler(DbContext, Mapper);

        await handler.Handle(
            new DeleteCardRequestCommand
            {
                Id = FixtureCardRequests.FixtureCardRequestForDelete.Id
            }, CancellationToken.None);

        Assert.Null(DbContext.CardRequests.FirstOrDefault(
            item => item.Id == FixtureCardRequests.FixtureCardRequestForDelete.Id));
    }

    [Fact]
    public async Task DeleteCardRequestCommandHandler_FailOnWrongId()
    {
        var handler = new DeleteCardRequestCommandHandler(DbContext, Mapper);

        await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
        {
            await handler.Handle(
                new DeleteCardRequestCommand
                {
                    Id = Guid.Empty
                }, CancellationToken.None);
        });
    }
}
