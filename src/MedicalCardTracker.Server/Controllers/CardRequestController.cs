// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using MedicalCardTracker.Application.Commands.CardRequests.CreateCardRequest;
using MedicalCardTracker.Application.Commands.CardRequests.DeleteCardRequest;
using MedicalCardTracker.Application.Commands.CardRequests.UpdateCardRequest;
using MedicalCardTracker.Application.Models.ViewModels;
using MedicalCardTracker.Application.Queries.CardRequests.GetCardRequest;
using MedicalCardTracker.Application.Queries.CardRequests.GetCardRequestCollection;
using MedicalCardTracker.Server.Application.Exceptions;
using MedicalCardTracker.Server.Hubs;
using MedicalCardTracker.Server.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace MedicalCardTracker.Server.Controllers;

public class CardRequestController : BaseController
{
    public CardRequestController(IHubContext<NotificationHub> notificationHubContext)
        : base(notificationHubContext)
    {
    }

    [HttpGet]
    public async Task<ActionResult<CardRequestVm>> Get(
        [FromQuery] GetCardRequestQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            return await Mediator.Send(request, cancellationToken);
        }
        catch (Exception e)
        {
            if (e.GetType() == typeof(EntityNotFoundException)) return NotFound();
        }

        return BadRequest();
    }

    [HttpGet]
    public async Task<ActionResult<CardRequestCollectionVm>> GetCollection(
        [FromQuery] GetCardRequestCollectionQuery request,
        CancellationToken cancellationToken
    ) => await Mediator.Send(request, cancellationToken);

    [HttpPost]
    public async Task<ActionResult<CardRequestVm>> Create(
        [FromBody] CreateCardRequestCommand request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(request, cancellationToken);

        await NotificationHubContext.Clients.All
            .SendAsync("OnCardRequestCreated", "Server", result, cancellationToken);

        return result;
    }

    [HttpPatch]
    public async Task<ActionResult<CardRequestVm>> Update(
        [FromQuery] Guid id,
        [FromBody] UpdateCardRequestCommandDto requestCommandDto,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await Mediator.Send(
                new UpdateCardRequestCommand
                {
                    Id = id,
                    CustomerName = requestCommandDto.CustomerName,
                    TargetAddress = requestCommandDto.TargetAddress,
                    PatientFullName = requestCommandDto.PatientFullName,
                    PatientBirthDate = requestCommandDto.PatientBirthDate,
                    Description = requestCommandDto.Description,
                    Status = requestCommandDto.Status,
                    Priority = requestCommandDto.Priority
                }, CancellationToken.None);

            await NotificationHubContext.Clients.All
                .SendAsync("OnCardRequestUpdated", "Server", result, cancellationToken);

            return result;
        }
        catch (Exception e)
        {
            if (e.GetType() == typeof(EntityNotFoundException)) return NotFound();
        }

        return BadRequest();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(
        [FromQuery] Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            await Mediator.Send(
                new DeleteCardRequestCommand
                {
                    Id = id
                }, CancellationToken.None);

            await NotificationHubContext.Clients.All
                .SendAsync("OnCardRequestDeleted", "Server", id, cancellationToken);

            return Ok();
        }
        catch (Exception e)
        {
            if (e.GetType() == typeof(EntityNotFoundException)) return NotFound();
        }

        return BadRequest();
    }
}
