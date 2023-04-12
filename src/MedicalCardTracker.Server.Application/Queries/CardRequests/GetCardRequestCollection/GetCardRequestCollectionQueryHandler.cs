// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using AutoMapper;
using MediatR;
using MedicalCardTracker.Application.Models;
using MedicalCardTracker.Application.Models.ViewModels;
using MedicalCardTracker.Application.Requests.Queries.CardRequests.GetCardRequestCollection;
using MedicalCardTracker.Domain.Entities;
using MedicalCardTracker.Server.Application.Interfaces;
using MedicalCardTracker.Server.Application.Models.Enums;
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
        var query = DbContext.CardRequests.AsQueryable();

        query = request.OrderBy == OrderBy.Ascending
            ? query.OrderBy(item => item.CreatedAt)
            : query.OrderByDescending(item => item.CreatedAt);

        if (request.TargetAddress != null)
            query = query.Where(item => item.TargetAddress == request.TargetAddress);

        var totalCount = await DbContext.CardRequests.CountAsync(cancellationToken);
        var targetCardRequestCollection = await query
            .Skip((int)(request.Page * request.Count))
            .Take((int)request.Count)
            .ToListAsync(cancellationToken);

        return Mapper.Map<CardRequestCollectionVm>(
            new VmCollection<CardRequest>
            {
                TotalCount = (uint)totalCount,
                Collection = targetCardRequestCollection
            });
    }
}
