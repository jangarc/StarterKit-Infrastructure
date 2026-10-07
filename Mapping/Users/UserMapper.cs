using Application.Features.Users.DTOs;
using Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Infrastructure.Mapping.Users;

[Mapper]
public partial class UserMapper
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [MapProperty("CreateUser.Name", "CreateUserName")]
    [MapProperty("UpdateUser.Name", "UpdateUserName")]
    public partial IQueryable<UserDto> UserToDto(IQueryable<User> user);
}

