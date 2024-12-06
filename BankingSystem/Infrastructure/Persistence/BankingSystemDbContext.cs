using BankingSystem.Domain.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BankingSystem.Infrastructure.Persistence
{
    public class BankingSystemDbContext : DbContext
    {
        public BankingSystemDbContext(DbContextOptions<BankingSystemDbContext> options) : base(options)
        {
        }
        public DbSet<Account> Account { get; set; }
        public DbSet<Transaction> Transaction { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
         
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            builder.Entity<Account>()
                .HasIndex(a => a.AccountNumber)
                .IsUnique();
         
            builder.Entity<Transaction>()
            .HasOne(t => t.Account)          // A transaction is related to one account
            .WithMany(a => a.Transactions)   // An account can have many transactions
            .HasForeignKey(t => t.AccountId) // The foreign key in the Transaction table
            .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(builder);
        }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}
