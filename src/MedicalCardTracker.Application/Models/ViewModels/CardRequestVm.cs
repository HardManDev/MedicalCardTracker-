// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using AutoMapper;
using MedicalCardTracker.Application.Interfaces;
using MedicalCardTracker.Domain.Entities;

namespace MedicalCardTracker.Application.Models.ViewModels;

public class CardRequestVm : CardRequest, IMapWith<CardRequest>
{
    public void Mapping(Profile profile)
        => profile.CreateMap<CardRequest, CardRequestVm>();
}
