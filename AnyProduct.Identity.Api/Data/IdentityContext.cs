using AnyProduct.Identity.Api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AnyProduct.Identity.Api.Data;

public class IdentityContext : IdentityDbContext<User>
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {

        base.OnModelCreating(builder);

        builder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR"
            },
             new IdentityRole
             {
                 Name = "User",
                 NormalizedName = "USER"
             }
            );

        builder.Entity<User>()
            .HasMany<RefreshToken>()
            .WithOne().HasForeignKey(x => x.UserId);
    }
}
