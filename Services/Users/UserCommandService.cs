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

        if (dto.CreateUserId == null)
            throw new ArgumentNullException(nameof(dto.CreateUserId));

        var user = new User(dto.TenantId.Value,
            dto.Name, dto.AliasName, dto.Account, dto.Email, dto.Birthday,
            _passwordHasher.HashPassword(dto.Password),
            dto.CreateUserId.Value);

        if(dto.Id.HasValue)
            user.Id = dto.Id.Value;

        _context.Users.Add(user);
    }
}
