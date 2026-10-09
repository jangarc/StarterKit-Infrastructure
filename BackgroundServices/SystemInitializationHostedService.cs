// Copyright (C) 2026 <CHANG,SHIH-HSIN/Arc Studio>
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY
using Infrastructure.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Paramore.Brighter;

namespace Infrastructure.BackgroundServices;

public class SystemInitializationHostedService : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory; // 💡 關鍵：用來安全隔離生命週期的工廠
    private readonly ILogger<SystemInitializationHostedService> _logger;

    public SystemInitializationHostedService(
        IServiceScopeFactory scopeFactory,
        ILogger<SystemInitializationHostedService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("--- 系統主機開機完成，準備執行系統初始化 Command ---");

        // 🛡️ 核心防線：手動切出一個 Scoped 範圍，模仿一次真實的 HTTP 請求
        using var scope = _scopeFactory.CreateScope();

        try
        {
            // 🎯 從這個獨立的安全範疇中，撈出 Brighter 的 Command 發送器
            var commandProcessor = scope.ServiceProvider.GetRequiredService<IAmACommandProcessor>();

            // 準備你的開機參數
            var warmUpCommand = new WarmUpCommand();

            // 🔥 發送 Command！Brighter 會在記憶體中自動尋找對應的 Handler 並安全執行
            await commandProcessor.SendAsync(warmUpCommand, cancellationToken: cancellationToken);

            _logger.LogInformation("--- 系統初始化 Command 執行成功！資料庫基礎數據已對齊 ---");
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "!!! 系統開機初始化 Command 發生嚴重崩潰 !!!");
            // 根據企業資安警報規範，可以選擇在此處阻止主機繼續啟動 (throw;)
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        // 關機時的清理邏輯，這裡不需要做任何事
        return Task.CompletedTask;
    }
}
