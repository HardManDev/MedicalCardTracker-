// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using AutoMapper;
using MedicalCardTracker.Domain.Entities;
using MedicalCardTracker.Server.Application.Interfaces;
using MedicalCardTracker.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace MedicalCardTracker.Tests.Requests;

public abstract class BaseTestsRequestHandler
{
    protected BaseTestsRequestHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        Mapper = mapper;
        DbContext = dbContext;

        ((DbContext)DbContext).Database.EnsureCreated();
        DbContext.CardRequests.AddRange(new List<CardRequest>
        {
            FixtureCardRequests.FixtureCardRequest,
            FixtureCardRequests.FixtureCardRequestForDelete,
            FixtureCardRequests.FixtureCardRequestForUpdate
        });
        ((DbContext)DbContext).SaveChanges();
    }

    protected IApplicationDbContext DbContext { get; }
    protected IMapper Mapper { get; }
}
