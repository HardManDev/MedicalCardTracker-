// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using System.Text.Json.Serialization;
using AutoMapper;
using MedicalCardTracker.Application.Convertors;
using MedicalCardTracker.Application.Interfaces;
using MedicalCardTracker.Domain.Entities;
using MedicalCardTracker.Domain.Enums;

namespace MedicalCardTracker.Application.Models.ViewModels;

public class CardRequestVm : IMapWith<CardRequest>
{
    public Guid Id { get; set; }

    public string CustomerName { get; set; } = null!;
    public string TargetAddress { get; set; } = null!;

    public string PatientFullName { get; set; } = null!;

    [JsonConverter(typeof(DateOnlyConverter))]
    public DateOnly? PatientBirthDate { get; set; }

    public string? Description { get; set; }

    public CardRequestStatus Status { get; set; }
    public CardRequestPriority Priority { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public void Mapping(Profile profile)
        => profile.CreateMap<CardRequest, CardRequestVm>();
}
