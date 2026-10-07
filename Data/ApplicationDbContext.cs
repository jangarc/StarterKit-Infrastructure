using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
using Domain.Entities;
using Casbin.Persist.Adapter.EFCore;

namespace Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    /// <inheritdoc/>
    public DbSet<Tenant> Tenants => Set<Tenant>();

    /// <inheritdoc/>
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new DefaultPersistPolicyEntityTypeConfiguration<int>("casbin_rule"));

        // 🛠️ PostgreSQL 專屬全域設定：強迫 Guid 映射為 pgsql 的 uuid 型別
        modelBuilder.HasPostgresExtension("uuid-ossp");

        // 載入剛才討論的 Order 與 OrderItem 的 Fluent API 聚合配置
        modelBuilder.Entity<User>(builder =>
        {
            builder.ToTable("users");
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id)
                .HasColumnType("uuid")
                .ValueGeneratedNever();

            builder.Property(u => u.Name)
               .IsRequired()
               .HasMaxLength(100);

            builder.HasIndex(u => u.Name)
               .IsUnique();

            builder.HasOne(u => u.CreateUser)
               .WithMany()
               .HasForeignKey(u => u.CreateUserId)
               .OnDelete(DeleteBehavior.ClientNoAction);

            builder.HasOne(u => u.UpdateUser)
                .WithMany()
                .HasForeignKey(u => u.UpdateUserId)
                .OnDelete(DeleteBehavior.ClientNoAction);

            builder.HasOne(u => u.Tenant)
                .WithMany()
                .HasForeignKey(u => u.TenantId)
                .OnDelete(DeleteBehavior.ClientNoAction);
        });

        modelBuilder.Entity<Tenant>(builder =>
        {
            builder.ToTable("tenants");
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).HasColumnType("uuid");

            builder.HasOne(u => u.CreateUser)
               .WithMany()
               .HasForeignKey(u => u.CreateUserId)
               .OnDelete(DeleteBehavior.ClientNoAction);

            builder.HasOne(u => u.UpdateUser)
                .WithMany()
                .HasForeignKey(u => u.UpdateUserId)
                .OnDelete(DeleteBehavior.ClientNoAction);
        });
    }

    /// <inheritdoc/>
    private IQueryable<T> CreateSqlQuery<T>(string sql, object[]? parameters = null)
    {
        var query = Database.SqlQueryRaw<T>(sql, parameters ?? []);
        return query;
    }

    /// <inheritdoc/>
    public async Task<List<T>> SqlQueryListAsync<T>(string sql, object[]? parameters = null, CancellationToken cancellationToken = default)
    {
        List<T> result = await CreateSqlQuery<T>(sql, parameters).ToListAsync(cancellationToken);
        return result;
    }

    /// <inheritdoc/>
    public async Task<T?> SqlQueryFirstOrDefaultAsync<T>(string sql, object[]? parameters = null, CancellationToken cancellationToken = default)
    {
        T? result = await CreateSqlQuery<T>(sql, parameters).FirstOrDefaultAsync(cancellationToken);

        return result;
    }

    /// <inheritdoc/>
    public async Task ExecuteSqlAsync(string sql, object[]? parameters = null, CancellationToken cancellationToken = default)
    {
        await Database.ExecuteSqlRawAsync(sql, parameters ?? [], cancellationToken);
    }
}
