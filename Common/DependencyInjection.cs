// Copyright (C) 2026 <CHANG,SHIH-HSIN/Arc Studio>
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection;

public static class InfrastructureHostExtensions
{
    // 🎯 核心技巧：擴充 IHostBuilder 介面，成功讓 Infrastructure 擁有控制主機的能力！
    public static IHostBuilder UseInfrastructureHost(this IHostBuilder hostBuilder)
    {
        return hostBuilder.UseDefaultServiceProvider((context, options) =>
        {
            // 🚀 在底層完美關閉 Scope 與開機驗證，WebApi 層完全不需關心技術細節
            options.ValidateScopes = false;
            options.ValidateOnBuild = false;
        });
    }
}
