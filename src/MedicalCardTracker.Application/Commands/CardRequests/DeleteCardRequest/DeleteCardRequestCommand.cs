// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using MediatR;

namespace MedicalCardTracker.Application.Commands.CardRequests.DeleteCardRequest;

public class DeleteCardRequestCommand : IRequest
{
    public Guid Id { get; set; }
}
