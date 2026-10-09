// Copyright (C) 2026 <CHANG,SHIH-HSIN/Arc Studio>
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authorization.Casbin;

public class CasbinRequirement : IAuthorizationRequirement
{
    public string? RequiredRoleOrPolicy { get; }

    public CasbinRequirement(string? requiredRoleOrPolicy = null)
    {
        RequiredRoleOrPolicy = requiredRoleOrPolicy;
    }
}
