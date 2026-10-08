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
    [MapProperty("CreateUser.Name", "CreateUserName")]
    [MapProperty("UpdateUser.Name", "UpdateUserName")]
    public partial IQueryable<UserDto> UserToDto(IQueryable<User> user);

    [MapProperty(nameof(CreateUserCommand.UserId), nameof(UserSecretDto.Id))]
    public partial UserSecretDto UserToSecretDto(CreateUserCommand command);
}

