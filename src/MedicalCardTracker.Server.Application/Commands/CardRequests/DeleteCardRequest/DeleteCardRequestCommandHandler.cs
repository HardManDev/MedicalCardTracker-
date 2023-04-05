// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using AutoMapper;
using MediatR;
using MedicalCardTracker.Application.Commands.CardRequests.DeleteCardRequest;
using MedicalCardTracker.Domain.Entities;
using MedicalCardTracker.Server.Application.Exceptions;
using MedicalCardTracker.Server.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedicalCardTracker.Server.Application.Commands.CardRequests.DeleteCardRequest;

public class DeleteCardRequestCommandHandler
    : BaseRequestHandler, IRequestHandler<DeleteCardRequestCommand>
{
    public DeleteCardRequestCommandHandler(IApplicationDbContext dbContext, IMapper mapper)
        : base(dbContext, mapper)
    {
    }

    public async Task Handle(DeleteCardRequestCommand request, CancellationToken cancellationToken)
    {
        var targetCardRequest =
            await DbContext.CardRequests
                .FirstOrDefaultAsync(item => item.Id == request.Id,
                    cancellationToken)
            ?? throw new EntityNotFoundException(nameof(CardRequest), request.Id);

        DbContext.CardRequests.Remove(targetCardRequest);
        await DbContext.SaveChangesAsync(cancellationToken);
    }
}
