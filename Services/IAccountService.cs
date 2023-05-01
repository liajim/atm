using AtmCoroBain.Models;

namespace AtmCoroBain.Services
{
    public interface IAccountService
    {
        // Account Services
        Task<List<Account>?> GetAccountsAsync();
        Task<Account?> GetAccountAsync(int id);
        Task<Account?> AddAccountAsync(Account account);
        Task<Account?> UpdateAccountAsync(Account account);
        Task<(bool, string)> DeleteAccountAsync(Account account);
    }
}