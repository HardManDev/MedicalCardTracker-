// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using AutoMapper;
using MedicalCardTracker.Application.Commands.CardRequests.CreateCardRequest;
using MedicalCardTracker.Application.Models.ViewModels;
using MedicalCardTracker.Domain.Enums;
using MedicalCardTracker.Server.Application.Commands.CardRequests.CreateCardRequest;
using MedicalCardTracker.Server.Application.Interfaces;
using Xunit;

namespace MedicalCardTracker.Tests.Requests.Server.Commands.CardRequests;

public class CreateCardRequestCommandHandlerTests : BaseTestsRequestHandler
{
    public CreateCardRequestCommandHandlerTests(IApplicationDbContext dbContext, IMapper mapper)
        : base(dbContext, mapper)
    {
    }

    [Fact]
    public async Task CreateCardRequestCommandHandler_Success()
    {
        const string testCustomerName = "Shavirin S. V.";
        const string testTargetAddress = "cab. 404";
        const string testPatientFullName = "Zhivopiscev Diomid Mitrofanievich";
        var testPatientBirthDate = new DateOnly(2000, 12, 30);
        const string testDescription = "this is test card request";
        const CardRequestPriority testPriority = CardRequestPriority.Urgently;

        var handler = new CreateCardRequestCommandHandler(DbContext, Mapper);

        var result = await handler.Handle(
            new CreateCardRequestCommand
            {
                CustomerName = testCustomerName,
                TargetAddress = testTargetAddress,
                PatientFullName = testPatientFullName,
                PatientBirthDate = testPatientBirthDate,
                Description = testDescription,
                Priority = testPriority
            }, CancellationToken.None);

        Assert.IsType<CardRequestVm>(result);
        Assert.NotNull(DbContext.CardRequests.FirstOrDefault(
            item =>
                item.Id != Guid.Empty &&
                item.CustomerName == testCustomerName &&
                item.TargetAddress == testTargetAddress &&
                item.PatientFullName == testPatientFullName &&
                item.PatientBirthDate == testPatientBirthDate &&
                item.Description == testDescription &&
                item.Priority == testPriority &&
                item.CreatedAt.Date == DateTime.Now.Date &&
                item.UpdatedAt == null));
    }
}
