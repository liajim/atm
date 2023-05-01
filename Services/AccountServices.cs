using Microsoft.EntityFrameworkCore;
using AtmCoroBain.Data;
using AtmCoroBain.Models;

namespace AtmCoroBain.Services
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _db;

        public AccountService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Account>?> GetAccountsAsync()
        {
            try
            {
                return await _db.Accounts.ToListAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Account?> GetAccountAsync(int id)
        {
            try
            {
                var query = (from p in _db.Accounts select p);
                query = (from p in query
                         select new Account
                         {
                             Id = p.Id,
                             ClientId = p.ClientId,
                             Balance = p.Transactions == null ? 0 : p.Transactions.Sum(x => x.Amount),
                             Client = p.Client,
                             Transactions = p.Transactions,
                         });
                return await query.Where(m => m.Id == id).FirstAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Account?> AddAccountAsync(Account Account)
        {
            try
            {
                await _db.Accounts.AddAsync(Account);
                await _db.SaveChangesAsync();
                return await _db.Accounts.FindAsync(Account.Id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Account?> UpdateAccountAsync(Account Account)
        {
            try
            {
                _db.Entry(Account).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return Account;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<(bool, string)> DeleteAccountAsync(Account Account)
        {
            try
            {
                var dbAccount = await _db.Accounts.FindAsync(Account.Id);
                if (dbAccount == null)
                {
                    return (false, "Account could not be found.");
                }
                _db.Accounts.Remove(Account);
                await _db.SaveChangesAsync();
                return (true, "Account got deleted.");
            }
            catch (Exception)
            {
                return (false, "Account couldnot be deleted");
            }
        }

    }
}
