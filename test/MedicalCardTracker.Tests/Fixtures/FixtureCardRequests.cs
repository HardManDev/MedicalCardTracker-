// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using MedicalCardTracker.Domain.Entities;
using MedicalCardTracker.Domain.Enums;

namespace MedicalCardTracker.Tests.Fixtures;

public static class FixtureCardRequests
{
    public static readonly CardRequest FixtureCardRequest = new()
    {
        Id = Guid.NewGuid(),
        CustomerName = "Miron Nikolaev",
        TargetAddress = "cab. 101",
        PatientFullName = "Efrosinia Makarova",
        PatientBirthDate = new DateOnly(1976, 4, 22),
        Description = "this is fixture card request",
        Status = CardRequestStatus.Created,
        Priority = CardRequestPriority.Urgently,
        CreatedAt = DateTimeOffset.Now,
        UpdatedAt = null
    };

    public static readonly CardRequest FixtureCardRequestForDelete = new()
    {
        Id = Guid.NewGuid(),
        CustomerName = "Miron Nikolaev",
        TargetAddress = "cab. 201",
        PatientFullName = "Nonna Popoa",
        PatientBirthDate = new DateOnly(1956, 11, 5),
        Description = "this is fixture card request for delete",
        Status = CardRequestStatus.Completed,
        Priority = CardRequestPriority.UnUrgently,
        CreatedAt = DateTimeOffset.Now,
        UpdatedAt = DateTimeOffset.Now.AddHours(12)
    };

    public static readonly CardRequest FixtureCardRequestForUpdate = new()
    {
        Id = Guid.NewGuid(),
        CustomerName = "Gavriila Nikolaeva",
        TargetAddress = "cab. 301",
        PatientFullName = "Yevgeniya Vinogradova",
        PatientBirthDate = new DateOnly(2005, 1, 14),
        Description = "this is fixture card request for update",
        Status = CardRequestStatus.Created,
        Priority = CardRequestPriority.Urgently,
        CreatedAt = DateTimeOffset.Now,
        UpdatedAt = null
    };
}
