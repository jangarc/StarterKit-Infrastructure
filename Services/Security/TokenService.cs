// Copyright (C) 2026 <CHANG,SHIH-HSIN/Arc Studio>
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces.Security;

namespace Infrastructure.Services.Security;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateJwtToken(string account, string userName, string role)
    {
        // 1. 從 appsettings.json 讀取安全設定（請確保 Key 至少有 32 個字元，即 256 bits）
        var secretKey = _configuration["JwtSettings:SecretKey"]
            ?? throw new InvalidOperationException("JWT Secret Key 未設定");
        var issuer = _configuration["JwtSettings:Issuer"];
        var audience = _configuration["JwtSettings:Audience"];
        var expiryMinutes = double.Parse(_configuration["JwtSettings:ExpiryMinutes"] ?? "60");

        // 2. 打包 Claims（使用者的隨身行李證件）
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, account),
            new Claim(JwtRegisteredClaimNames.Name, userName),
            new Claim(ClaimTypes.Role, role), // 💡 剛好對齊你之前寫的 Casbin 授權角色！
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Token 唯一識別碼（防重放攻擊）
        };

        // 3. 準備加密金鑰
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // 4. 產製 JWT 核心物件
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiryMinutes), // ⚠️ 務必使用 UTC 時間避免時區地雷
            signingCredentials: creds
        );

        // 5. 將物件序列化為最終的 Token 字串（Headers.Payload.Signature）
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public bool IsTokenExpired(string tokenString)
    {
        var handler = new JwtSecurityTokenHandler();

        if (!handler.CanReadToken(tokenString))
            throw new ArgumentException("無效的 JWT 格式");

        var jwtToken = handler.ReadJwtToken(tokenString);

        // ⚠️ 核心雷區：ValidTo 是 UTC 時間，所以必須與 DateTime.UtcNow 比對！
        return jwtToken.ValidTo < DateTime.UtcNow;
    }

    public bool ValidateTokenLifetime(string tokenString)
    {
        var secretKey = _configuration["JwtSettings:SecretKey"]
            ?? throw new InvalidOperationException("JWT Secret Key 未設定");
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(secretKey);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            // 🎯 核心：啟動生命週期驗證
            ValidateLifetime = true,
            // 💡 關鍵設定：微軟預設有 5 分鐘的「時鐘偏移容忍值 (ClockSkew)」
            // 為了防範時間差攻擊，在正式環境強烈建議將其歸零 (精準比對)
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            // 如果 Token 沒過期且簽章正確，這行會綠燈通車
            handler.ValidateToken(tokenString, validationParameters, out SecurityToken validatedToken);
            return false; // 代表「沒有過期」
        }
        catch (SecurityTokenExpiredException)
        {
            // 🛑 精准捕獲：Token 已經明確過期！
            return true;
        }
        catch (Exception)
        {
            // 其他錯誤（如簽章被竄改、格式錯誤等）
            throw new SecurityTokenException("Token 驗證失敗或已被串改");
        }
    }
}

