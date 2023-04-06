// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using AutoMapper;
using MediatR;
using MedicalCardTracker.Application.Models;
using MedicalCardTracker.Application.Models.ViewModels;
using MedicalCardTracker.Application.Queries.CardRequests.GetCardRequestCollection;
using MedicalCardTracker.Domain.Entities;
using MedicalCardTracker.Server.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedicalCardTracker.Server.Application.Queries.CardRequests.GetCardRequestCollection;

public class GetCardRequestCollectionQueryHandler
    : BaseRequestHandler, IRequestHandler<GetCardRequestCollectionQuery, CardRequestCollectionVm>
{
    public GetCardRequestCollectionQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        : base(dbContext, mapper)
    {
    }

    public async Task<CardRequestCollectionVm> Handle(GetCardRequestCollectionQuery request,
        CancellationToken cancellationToken)
    {
        var targetCardRequestCollection =
            await DbContext.CardRequests
                .Skip((int)(request.Page * request.Count))
                .Take((int)request.Count)
                .ToListAsync(cancellationToken);

        return Mapper.Map<CardRequestCollectionVm>(
            new VmCollection<CardRequest>
            {
                TotalCount = (uint)DbContext.CardRequests.Count(),
                Collection = targetCardRequestCollection
            });
    }
}
