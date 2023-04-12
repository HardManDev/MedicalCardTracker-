// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using MediatR;
using MedicalCardTracker.Application.Models.ViewModels;
using MedicalCardTracker.Server.Application.Models.Enums;

namespace MedicalCardTracker.Application.Requests.Queries.CardRequests.GetCardRequestCollection;

public class GetCardRequestCollectionQuery : IRequest<CardRequestCollectionVm>
{
    public uint Page { get; set; } = 0;
    public uint Count { get; set; } = 100;

    public OrderBy OrderBy { get; set; } = OrderBy.Ascending;
    public string? TargetAddress { get; set; }
}
