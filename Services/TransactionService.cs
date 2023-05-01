using Microsoft.EntityFrameworkCore;
using AtmCoroBain.Data;
using AtmCoroBain.Models;

namespace AtmCoroBain.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly AppDbContext _db;

        public TransactionService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Transaction>?> GetTransactionsAsync()
        {
            try
            {
                return await _db.Transactions.ToListAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Transaction?> GetTransactionAsync(int id)
        {
            try
            {
                return await _db.Transactions.FindAsync(id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Transaction?> AddDepositAsync(int AccountId, decimal amount)
        {
            try
            {
                Transaction newTransaction = new Transaction();
                newTransaction.TransactionType = TransactionType.Deposit;
                newTransaction.AccountId = AccountId;
                newTransaction.Amount = amount;
                await _db.Transactions.AddAsync(newTransaction);
                await _db.SaveChangesAsync();
                return await _db.Transactions.FindAsync(newTransaction.Id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Transaction?> AddWithdrawAsync(int AccountId, decimal amount)
        {
            try
            {
                Transaction newTransaction = new Transaction();
                newTransaction.TransactionType = TransactionType.Withdraw;
                newTransaction.AccountId = AccountId;
                newTransaction.Amount = -1 * amount;
                await _db.Transactions.AddAsync(newTransaction);
                await _db.SaveChangesAsync();
                return await _db.Transactions.FindAsync(newTransaction.Id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Transaction?> AddTransferAsync(int FromAccountId, int ToAccountId, decimal amount)
        {
            try
            {
                Transaction newWithDraw = new Transaction();
                newWithDraw.TransactionType = TransactionType.Transfer;
                newWithDraw.AccountId = FromAccountId;
                newWithDraw.Amount = -1 * amount;
                await _db.Transactions.AddAsync(newWithDraw);
                await _db.SaveChangesAsync();

                Transaction newTransaction = new Transaction();
                newTransaction.TransactionType = TransactionType.Transfer;
                newTransaction.AccountId = ToAccountId;
                newTransaction.WithDrawReferenceId = newWithDraw.Id;
                newTransaction.Amount = amount;
                await _db.Transactions.AddAsync(newTransaction);
                await _db.SaveChangesAsync();

                return await _db.Transactions.FindAsync(newTransaction.Id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Transaction?> UpdateTransactionAsync(int TransactionId, decimal amount)
        {
            try
            {
                var referencedTransaction = _db.Transactions.Where(b => b.WithDrawReferenceId == TransactionId);
                if (referencedTransaction.Any())
                {
                    return null;
                }
                Transaction transaction = _db.Transactions.Include(b => b.WithDrawReference).First(b => b.Id == TransactionId);
                if (transaction.TransactionType == TransactionType.Withdraw)
                    amount = -1 * amount;
                transaction.Amount = amount;
                if (transaction.WithDrawReference != null)
                {
                    Transaction withdraw = transaction.WithDrawReference;
                    withdraw.Amount = -1 * amount;
                    _db.Entry(withdraw).State = EntityState.Modified;
                }
                _db.Entry(transaction).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return transaction;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<(bool, string)> DeleteTransactionAsync(Transaction Transaction)
        {
            try
            {
                var dbTransaction = await _db.Transactions.FindAsync(Transaction.Id);
                if (dbTransaction == null)
                {
                    return (false, "Transaction could not be found.");
                }
                var referencedTransaction = _db.Transactions.Where(b => b.WithDrawReferenceId == Transaction.Id);
                if (referencedTransaction.Any())
                {
                    return (false, "Transaction cannot be removed, please remove the transfer transaction.");
                }
                int? WithdrawReference = dbTransaction.WithDrawReferenceId;                 
                _db.Transactions.Remove(Transaction);
                await _db.SaveChangesAsync();
                if (WithdrawReference != null)
                {
                    var WithdrawTransaction = await _db.Transactions.FindAsync(WithdrawReference);
                    if (WithdrawTransaction != null)
                        _db.Transactions.Remove(WithdrawTransaction);
                }
                await _db.SaveChangesAsync();
                return (true, "Transaction got deleted.");
            }
            catch (Exception)
            {
                return (false, "Transaction could not be deleted.");
            }
        }

    }
}
