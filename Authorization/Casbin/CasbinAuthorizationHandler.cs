// Copyright (C) 2026 <CHANG,SHIH-HSIN/Arc Studio>
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY
using Casbin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Infrastructure.Authorization.Casbin;

public class CasbinAuthorizationHandler : AuthorizationHandler<CasbinRequirement>
{
    private readonly IEnforcer _enforcer;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CasbinAuthorizationHandler(IEnforcer enforcer, IHttpContextAccessor httpContextAccessor)
    {
        _enforcer = enforcer;
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CasbinRequirement requirement)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null) return;

        // 🛡️ 1. 領取 IdentityServer 經由 JWT 解析出來的 Claims
        // IdentityServer 預設的識別碼通常是 ClaimTypes.NameIdentifier 或 "sub"
        var sub = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                  ?? context.User.FindFirst("sub")?.Value
                  ?? "anonymous";

        // 如果你的 Casbin 規則是以「角色 (Role)」作為主體，則改撈 Role Claim
        var userRole = context.User.FindFirst(ClaimTypes.Role)?.Value ?? "guest";

        // ==========================================
        // 模態一：情境 [Authorize("role code")] -> 走特定 Policy 判定
        // ==========================================
        if (!string.IsNullOrEmpty(requirement.RequiredRoleOrPolicy))
        {
            // 詢問 Casbin：該用戶(或角色) 是否擁有這個特定權限代碼(act)？
            // 這裡將 obj 設為 "policy_named"，act 設為指定的 role code
            if (await _enforcer.EnforceAsync(userRole, "policy_named", requirement.RequiredRoleOrPolicy))
            {
                context.Succeed(requirement);
            }
            return;
        }

        // ==========================================
        // 模態二：情境 [Authorize] -> 走全域自動路由對應與 HTTP 方法判定
        // ==========================================
        var obj = httpContext.Request.Path.Value ?? ""; // 例如: /api/values
        var act = httpContext.Request.Method;          // 例如: GET, POST

        // 詢問 Casbin：這個角色，能不能對這個 API 路由，執行這個 HTTP 動詞？
        if (await _enforcer.EnforceAsync(userRole, obj, act))
        {
            context.Succeed(requirement);
        }
    }
}
