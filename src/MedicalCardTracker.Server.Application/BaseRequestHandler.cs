﻿// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using AutoMapper;
using MedicalCardTracker.Server.Application.Interfaces;

namespace MedicalCardTracker.Server.Application;

public abstract class BaseRequestHandler
{
    protected BaseRequestHandler(IApplicationDbContext dbContext, IMapper mapper)
        => (DbContext, Mapper) = (dbContext, mapper);

    protected IApplicationDbContext DbContext { get; }
    protected IMapper Mapper { get; }
}
