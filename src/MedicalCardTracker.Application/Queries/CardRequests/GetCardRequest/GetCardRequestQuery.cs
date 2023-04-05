// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using MediatR;
using MedicalCardTracker.Application.Models.ViewModels;

namespace MedicalCardTracker.Application.Queries.CardRequests.GetCardRequest;

public class GetCardRequestQuery : IRequest<CardRequestVm>
{
    public Guid Id { get; set; }
}
