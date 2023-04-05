// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using AutoMapper;
using MedicalCardTracker.Application.Interfaces;

namespace MedicalCardTracker.Application.Models;

public class VmCollection<TCollection> : IMapWith<ICollection<TCollection>> where TCollection : class
{
    public uint TotalCount { get; set; }
    public ICollection<TCollection> CardRequests { get; set; } = null!;

    public void Mapping(Profile profile)
        => profile.CreateMap<ICollection<TCollection>, VmCollection<TCollection>>()
            .ForMember(dest => dest.TotalCount,
                opt =>
                    opt.MapFrom(src => src.Count))
            .ForMember(dest => dest.CardRequests,
                opt =>
                    opt.MapFrom(src => src));
}
