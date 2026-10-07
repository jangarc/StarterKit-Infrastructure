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
