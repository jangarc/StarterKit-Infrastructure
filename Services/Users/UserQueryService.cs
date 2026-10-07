using Application.Features.Users.DTOs;
using Application.Features.Users.Queries;
using Application.Interfaces;
using Application.Interfaces.Users;
using Domain.Specifications.Users;
using Infrastructure.Mapping.Users;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Infrastructure.Services.Users;

public class UserQueryService : IUserQueryService
{
    private readonly IApplicationDbContext _context;
    private readonly UserMapper _mapper;

    public UserQueryService(IApplicationDbContext context, UserMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<UserDto?> GetUserByIdAsync(GetUserQuery query, CancellationToken cancellationToken = default)
    {
        var spec = new UserByIdSpecification(query.UserId);

        //var userDto = await _context.Users
        //    .Where(spec.ToExpression())
        //    .Select(u => new UserDto(u.Id, u.Name, u.AliasName, u.Birthday,
        //        u.Account, u.Email,
        //        u.TenantId, u.Tenant.Name, u.CreateUserId, u.CreateUser.Name,
        //        u.UpdateUserId, u.UpdateUser.Name))
        //    .FirstOrDefaultAsync(cancellationToken);
        var userDto = await _mapper.UserToDto(_context.Users.Where(spec.ToExpression()))
            .FirstOrDefaultAsync(cancellationToken);

        return userDto;
    }

}
