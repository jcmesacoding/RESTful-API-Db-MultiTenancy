using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Domain.Entities;
using System.Security.Claims;

namespace Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IHttpContextAccessor httpContextAccessor
    ) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var tenantId = _httpContextAccessor.HttpContext?
            .User?
            .FindFirst("TenantId")?
            .Value;

        if (!string.IsNullOrEmpty(tenantId))
        {
            modelBuilder.Entity<Product>()
                .HasQueryFilter(p => p.TenantId == tenantId);
        }

        base.OnModelCreating(modelBuilder);
    }
}