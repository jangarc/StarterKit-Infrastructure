// Copyright (C) 2026 <CHANG,SHIH-HSIN/Arc Studio>
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY
using Application.Interfaces.Security;

namespace Infrastructure.Services.Security;
public class PasswordHasher : IPasswordHasher
{
    // 💡 工作因子預設為 11（代表迭代 2^11 次），既能有效防禦顯卡（GPU）暴力破解，又能保持後端高反應速度
    private const int WorkFactor = 11;

    public string HashPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentNullException(nameof(password));

        // 🚀 核心黑魔法：自動生成唯一的隨機鹽巴，並產出不可逆的雜湊值
        // 產出格式類似：$2a$11$G7Z... (內含演算法版本、工作因子、鹽巴、密碼內核)
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password, WorkFactor);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hashedPassword))
            return false;

        // 🚀 自動從 hashedPassword 中拆解出當時的鹽巴與迭代次數，進行安全比對（內建防止計時攻擊）
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
    }
}