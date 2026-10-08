using Application.Features.Users.DTOs;
using Application.Features.Users.Queries;
using Application.Interfaces;
using Application.Interfaces.Users;
using Domain.Entities;
using Domain.Specifications;
using Domain.Specifications.Users;
using Infrastructure.Mapping.Users;
using Microsoft.EntityFrameworkCore;

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
    public async Task<UserDto?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var spec = new UserByIdSpecification(userId);

        var userDto = await _mapper.UserToDto(_context.Users.Where(spec.ToExpression()))
            .FirstOrDefaultAsync(cancellationToken);

        return userDto;
    }

    /// <inheritdoc/>
    public async Task<List<UserDto>> QueryUserAsync(SearchUsersQuery query, CancellationToken cancellationToken = default)
    {
        ISpecification<User> spec = new UserTenantSpecification(query.TenantId);

        if (!string.IsNullOrWhiteSpace(query.Keyword))
            spec = spec.And(new UserByKeywordSpecification(query.Keyword));

        if (query.Birthday.HasValue)
            spec = spec.And(new UserByBithdayRangeSpecification(query.Birthday));
        else if (query.StartBirthdayRange.HasValue || query.EndBirthdayRange.HasValue)
            spec = spec.And(new UserByBithdayRangeSpecification(query.StartBirthdayRange, query.EndBirthdayRange));

        var userDtoList = await _mapper.UserToDto(_context.Users.Where(spec.ToExpression()))
            .ToListAsync(cancellationToken);

        return userDtoList;
    }

}
