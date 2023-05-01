using AtmCoroBain.Models;

namespace AtmCoroBain.Services
{
    public interface ITransactionService
    {
        // Transaction Services
        Task<List<Transaction>?> GetTransactionsAsync();
        Task<Transaction?> GetTransactionAsync(int id);
        Task<Transaction?> AddDepositAsync(int AccountId, decimal amount);
        Task<Transaction?> AddWithdrawAsync(int AccountId, decimal amount);
        Task<Transaction?> AddTransferAsync(int FromAccountId, int ToAccountId, decimal amount);
        Task<Transaction?> UpdateTransactionAsync(int TransactionId, decimal amount);
        Task<(bool, string)> DeleteTransactionAsync(Transaction Transaction);
    }
}