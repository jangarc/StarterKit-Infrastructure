using Microsoft.Extensions.Hosting; // 💡 引入主機介面

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
