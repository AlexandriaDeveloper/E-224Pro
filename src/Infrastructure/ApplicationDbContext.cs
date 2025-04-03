
using Core;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure;

public class ApplicationDbContext : DbContext
{
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<Daily> Dailies { get; set; }
    public DbSet<SubsidiaryJournal> SubsidiaryJournals { get; set; }
    public DbSet<Form> Forms { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Fund> Funds { get; set; }
    public DbSet<Collage> Collages { get; set; }
    public DbSet<FormDetails> FormDetails { get; set; }

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {

    }
    override protected void OnModelCreating(ModelBuilder modelBuilder)
    {


        //modelBuilder.Entity<Account>().HasMany(x => x.ChildAccounts).WithOne(x => x.ParentAccount).HasForeignKey(x => x.ParentAccountId).OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<SubsidiaryJournal>()
        .HasOne(x => x.FormDetails)
        .WithMany(x => x.SubsidiaryJournals)
        .OnDelete(DeleteBehavior.NoAction);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
        //  optionsBuilder.UseLazyLoadingProxies();
    }
}

