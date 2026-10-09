// Copyright (C) 2026 <CHANG,SHIH-HSIN/Arc Studio>
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY
using Application.Features.Users.Commands;
using Application.Features.Users.DTOs;
using Application.Interfaces.Users;
using Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Infrastructure.Mapping.Users;

[Mapper]
public partial class UserMapper : IUserMapper
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [MapProperty("CreatedId", "CreatedUserId")]
    [MapProperty("CreatedUser.Name", "CreatedUserName")]
    [MapProperty("LastModifiedId", "LastModifiedUserId")]
    [MapProperty("LastModifiedUser.Name", "UpdatedUserName")]
    [MapProperty("DeletedId", "DeletedIdUserId")]
    [MapProperty("DeletedUser.Name", "DeletedUserName")]
    public partial IQueryable<UserDto> UserToDto(IQueryable<User> user);

    [MapProperty(nameof(CreateUserCommand.UserId), nameof(UserSecretDto.Id))]
    public partial UserSecretDto UserToSecretDto(CreateUserCommand command);
}

