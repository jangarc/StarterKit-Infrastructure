// Copyright (C) 2026 <CHANG,SHIH-HSIN/Arc Studio>
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY
using Application.Features.Users.DTOs;
using Application.Interfaces;
using Application.Interfaces.Security;
using Application.Interfaces.Users;
using Domain.Entities;

namespace Infrastructure.Services.Users;

public class UserCommandService: IUserCommandService
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public UserCommandService(IApplicationDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    /// <inheritdoc/>
    public async Task CreateUserAsync(UserSecretDto dto)
    {
        if (dto.TenantId == null)
            throw new ArgumentNullException(nameof(dto.TenantId));

        if (dto.CreatedUserId == null)
            throw new ArgumentNullException(nameof(dto.CreatedUserId));

        var user = new User(dto.TenantId.Value,
            dto.Name, dto.AliasName, dto.Account, dto.Email, dto.Birthday,
            _passwordHasher.HashPassword(dto.Password),
            dto.CreatedUserId.Value);

        if(dto.Id.HasValue)
            user.Id = dto.Id.Value;

        _context.Users.Add(user);
    }
}
