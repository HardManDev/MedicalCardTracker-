// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using AutoMapper;
using MedicalCardTracker.Application.Commands.CardRequests.UpdateCardRequest;
using MedicalCardTracker.Application.Models.ViewModels;
using MedicalCardTracker.Domain.Enums;
using MedicalCardTracker.Server.Application.Commands.CardRequests.UpdateCardRequest;
using MedicalCardTracker.Server.Application.Exceptions;
using MedicalCardTracker.Server.Application.Interfaces;
using MedicalCardTracker.Tests.Fixtures;
using Xunit;

namespace MedicalCardTracker.Tests.Requests.Server.Commands.CardRequests;

public class UpdateCardRequestCommandHandlerTests : BaseTestsRequestHandler
{
    public UpdateCardRequestCommandHandlerTests(IApplicationDbContext dbContext, IMapper mapper)
        : base(dbContext, mapper)
    {
    }

    [Fact]
    public async Task UpdateCardRequestCommandHandler_Success()
    {
        const string testCustomerName = "Yaromir Semenov";
        const string testTargetAddress = "cab. 404";
        const string testPatientFullName = "Koloda Tarasov";
        var testPatientBirthDate = new DateOnly(2000, 12, 30);
        const string testDescription = "this is test card request";
        const CardRequestPriority testPriority = CardRequestPriority.Urgently;

        var handler = new UpdateCardRequestCommandHandler(DbContext, Mapper);

        var result = await handler.Handle(
            new UpdateCardRequestCommand
            {
                Id = FixtureCardRequests.FixtureCardRequestForUpdate.Id,
                CustomerName = testCustomerName,
                TargetAddress = testTargetAddress,
                PatientFullName = testPatientFullName,
                PatientBirthDate = testPatientBirthDate,
                Description = testDescription,
                Status = CardRequestStatus.Completed,
                Priority = testPriority
            }, CancellationToken.None);

        Assert.IsType<CardRequestVm>(result);
        Assert.NotNull(DbContext.CardRequests.FirstOrDefault(
            item => item.Id == FixtureCardRequests.FixtureCardRequestForUpdate.Id &&
                    item.CustomerName == testCustomerName &&
                    item.TargetAddress == testTargetAddress &&
                    item.PatientFullName == testPatientFullName &&
                    item.PatientBirthDate == testPatientBirthDate &&
                    item.Description == testDescription &&
                    item.Priority == testPriority &&
                    item.CreatedAt.Date == DateTime.Now.Date &&
                    item.UpdatedAt != null && item.UpdatedAt.Value.Date == DateTime.Now.Date));
    }

    [Fact]
    public async Task UpdateCardRequestCommandHandler_FailOnWrongId()
    {
        var handler = new UpdateCardRequestCommandHandler(DbContext, Mapper);

        await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
        {
            await handler.Handle(
                new UpdateCardRequestCommand
                {
                    Id = Guid.Empty,
                    Priority = CardRequestPriority.UnUrgently
                }, CancellationToken.None);
        });
    }
}
