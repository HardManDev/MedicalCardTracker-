// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using AutoMapper;
using MediatR;
using MedicalCardTracker.Application.Models.ViewModels;
using MedicalCardTracker.Application.Requests.Queries.CardRequests.GetCardRequest;
using MedicalCardTracker.Domain.Entities;
using MedicalCardTracker.Server.Application.Exceptions;
using MedicalCardTracker.Server.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedicalCardTracker.Server.Application.Queries.CardRequests.GetCardRequest;

public class GetCardRequestQueryHandler
    : BaseRequestHandler, IRequestHandler<GetCardRequestQuery, CardRequestVm>
{
    public GetCardRequestQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        : base(dbContext, mapper)
    {
    }

    public async Task<CardRequestVm> Handle(GetCardRequestQuery request, CancellationToken cancellationToken)
    {
        var targetCardRequest =
            await DbContext.CardRequests
                .FirstOrDefaultAsync(item => item.Id == request.Id,
                    cancellationToken)
            ?? throw new EntityNotFoundException(nameof(CardRequest), request.Id);

        return Mapper.Map<CardRequestVm>(targetCardRequest);
    }
}
