using Microsoft.EntityFrameworkCore;
using AtmCoroBain.Models;

namespace AtmCoroBain.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Auto increment ids
            builder.Entity<Account>()
                .Property(f => f.Id)
                .ValueGeneratedOnAdd();
            
            builder.Entity<Client>()
                .Property(f => f.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Transaction>()
                .Property(f => f.Id)
                .ValueGeneratedOnAdd();
            
            builder.Entity<Transaction>()
                .Property(o => o.Amount)
                .HasColumnType("decimal(18,2)");
            
            builder.Entity<Account>()
                .Property(e => e.Balance)
                .UsePropertyAccessMode(PropertyAccessMode.Property);

            // Define relationship between accounts and clients
            builder.Entity<Account>()
                .HasOne(x => x.Client)
                .WithMany(x => x.Accounts);

            // Define relationship between transactions and accounts
            builder.Entity<Transaction>()
                .HasOne(x => x.Account)
                .WithMany(x => x.Transactions);

            // Define default Transaction date
            builder.Entity<Transaction>()
                .Property(b => b.TransactionDate )
                .HasDefaultValueSql("getdate()");

            // Seed database with accounts, clients and transactions for demo
            new DbInitializer(builder).Seed();
        }
    }
}
