
using Core;
using Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure;

public class ApplicationDbContext : IdentityDbContext<AppUser>
{
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<Daily> Dailies { get; set; }
    public DbSet<SubsidiaryJournal> SubsidiaryJournals { get; set; }
    public DbSet<Form> Forms { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<SubAccount> SubAccounts { get; set; }
    public DbSet<Fund> Funds { get; set; }
    public DbSet<Collage> Collages { get; set; }
    public DbSet<FormDetails> FormDetails { get; set; }
    public DbSet<UserAccount> UserAccounts { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserAccount>()
            .HasKey(us => new { us.UserId, us.AccountId });

        modelBuilder.Entity<UserAccount>()
            .HasOne(us => us.User)
            .WithMany()
            .HasForeignKey(us => us.UserId);

        modelBuilder.Entity<UserAccount>()
            .HasOne(us => us.Account)
            .WithMany()
            .HasForeignKey(us => us.AccountId);

        modelBuilder.Entity<SubsidiaryJournal>()
            .HasOne(x => x.FormDetails)
            .WithMany(x => x.SubsidiaryJournals)
            .OnDelete(DeleteBehavior.NoAction);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
    }
}

