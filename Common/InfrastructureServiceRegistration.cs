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
using Application.Features.Users.Queries;
using Application.Interfaces;
using Application.Interfaces.Security;
using Application.Interfaces.Users;
using Casbin;
using Casbin.Persist.Adapter.EFCore;
using Casbin.Persist.Adapter.EFCore.Extensions;
using Infrastructure.Authorization.Casbin;
using Infrastructure.Data;
using Infrastructure.Mapping.Users;
using Infrastructure.Services.Security;
using Infrastructure.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Paramore.Brighter.Extensions.DependencyInjection;
using Paramore.Darker.AspNetCore;

namespace Infrastructure.Common;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        // EF Core
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                connectionString,
                npgsqlOptions => npgsqlOptions.MigrationsAssembly(nameof(Infrastructure))
                // 🛠️ 關鍵：指定將資料庫遷移腳本（Migrations）生成在基礎設施層，不污染 API 層
            ));

        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());


        // 增加密碼驗證處理
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        // JWT授權
        services.AddSingleton<ITokenService, TokenService>();

        // Paramore 相關
        services.AddBrighter(options =>
        {
            // 這裡可以配置你的 Brighter 原生選項（例如：設定 PolicyRegistry 重試政策）
        })
            .AutoFromAssemblies([typeof(CreateUserCommandHandler).Assembly]);

        services.AddDarker(options =>
        {
            // 💡 告訴 Darker：請使用網頁請求的 Scope 來建構 Handler
            options.HandlerLifetime = ServiceLifetime.Scoped;
        })
                .AddHandlersFromAssemblies([typeof(GetUserQueryHandler).Assembly]);

        // Casbin 
        services.AddEFCoreAdapter<int>();
        services.AddDbContext<CasbinDbContext<int>>(options =>
            options.UseNpgsql(
                connectionString,
                npgsqlOptions => npgsqlOptions.MigrationsAssembly(nameof(Infrastructure))
                // 🛠️ 關鍵：指定將資料庫遷移腳本（Migrations）生成在基礎設施層，不污染 API 層
            ));
        services.AddSingleton<IEnforcer>(sp =>
            {
                //var options = new DbContextOptionsBuilder<CasbinDbContext<int>>()
                //    .UseNpgsql(connectionString)
                //    .Options;
                //var context = new CasbinDbContext<int>(options);

                //// 💡 這裡使用的是 EFCoreAdapter (記得傳入你的 DbContext 類型)
                //var adapter = new EFCoreAdapter<int>(context);

                //var enforcer = new Enforcer("config/rbac_with_multiple_roles.conf", adapter);
                //enforcer.LoadPolicy();

                var enforcer = new Enforcer("config/rbac_with_multiple_roles.conf", "config/policy.csv");

                return enforcer;
            });
        services.AddSingleton<IAuthorizationPolicyProvider, CasbinPolicyProvider>();
        services.AddTransient<IAuthorizationHandler, CasbinAuthorizationHandler>();
        services.AddAuthorizationCore(options =>
        {
            // 配置預設 [Authorize] 的全域動態路由路由規則
            var defaultPolicy = new AuthorizationPolicyBuilder();
            defaultPolicy.RequireAuthenticatedUser();
            defaultPolicy.AddRequirements(new CasbinRequirement()); // 傳入 null，走全域路由動態 Enforce
            options.DefaultPolicy = defaultPolicy.Build();
        });

        services.AddScoped<IUserQueryService, UserQueryService>();
        services.AddScoped<IUserCommandService, UserCommandService >();
        services.AddSingleton<IUserMapper, UserMapper>();

        return services;
    }
}
