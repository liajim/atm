using Microsoft.EntityFrameworkCore;
using AtmCoroBain.Models;

namespace AtmCoroBain.Data
{
    public class DbInitializer
    {
        private readonly ModelBuilder _builder;

        public DbInitializer(ModelBuilder builder)
        {
            _builder = builder;
        }

        public void Seed()
        {
            _builder.Entity<Client>(a =>
            {
                a.HasData(new Client{Id=1, Name = "Client 1"});
                a.HasData(new Client{Id=2, Name = "Client 2"});
            });

            _builder.Entity<Account>(b =>
            {
                b.HasData(new Account{Id = 1, ClientId = 1});
                b.HasData(new Account{Id = 2, ClientId = 1});
                b.HasData(new Account{Id = 3, ClientId = 2});
                b.HasData(new Account{Id = 4, ClientId = 2});
            });

            _builder.Entity<Transaction>(b =>
            {
                b.HasData(new Transaction{Id = 1, AccountId = 1, Amount = 100, TransactionType = TransactionType.Deposit});
                b.HasData(new Transaction{Id = 2, AccountId = 1, Amount = -50, TransactionType = TransactionType.Withdraw});
                b.HasData(new Transaction{Id = 3, AccountId = 3, Amount = -100, TransactionType = TransactionType.Withdraw});
                b.HasData(new Transaction{Id = 4, AccountId = 1, Amount = 100, TransactionType = TransactionType.Transfer, WithDrawReferenceId=3});
                

                b.HasData(new Transaction{Id = 5, AccountId = 2, Amount = 300, TransactionType = TransactionType.Deposit});
                b.HasData(new Transaction{Id = 6, AccountId = 2, Amount = -150, TransactionType = TransactionType.Withdraw});
                b.HasData(new Transaction{Id = 7, AccountId = 4, Amount = -200, TransactionType = TransactionType.Withdraw});
                b.HasData(new Transaction{Id = 8, AccountId = 2, Amount = 200, TransactionType = TransactionType.Transfer, WithDrawReferenceId=7});

                b.HasData(new Transaction{Id = 9, AccountId = 3, Amount = 500, TransactionType = TransactionType.Deposit});
                b.HasData(new Transaction{Id = 10, AccountId = 3, Amount = -450, TransactionType = TransactionType.Withdraw});
                b.HasData(new Transaction{Id = 11, AccountId = 2, Amount = -100, TransactionType = TransactionType.Withdraw});
                b.HasData(new Transaction{Id = 12, AccountId = 3, Amount = 100, TransactionType = TransactionType.Transfer, WithDrawReferenceId=11});

                b.HasData(new Transaction{Id = 13, AccountId = 4, Amount = 300, TransactionType = TransactionType.Deposit});
                b.HasData(new Transaction{Id = 14, AccountId = 4, Amount = -100, TransactionType = TransactionType.Withdraw});                
                b.HasData(new Transaction{Id = 15, AccountId = 1, Amount = -100, TransactionType = TransactionType.Withdraw});
                b.HasData(new Transaction{Id = 16, AccountId = 4, Amount = 100, TransactionType = TransactionType.Transfer, WithDrawReferenceId=15});
            });
        }
    }
}