using Microsoft.EntityFrameworkCore;
using AtmCoroBain.Data;
using AtmCoroBain.Models;

namespace AtmCoroBain.Services
{
    public class ClientService : IClientService
    {
        private readonly AppDbContext _db;

        public ClientService(AppDbContext db)
        {
            _db = db;
        }


        public async Task<List<Client>?> GetClientsAsync()
        {
            try
            {
                return await _db.Clients.ToListAsync();
            }
            catch (Exception)
            {
                return null;
            }

        }

        public async Task<Client?> GetClientAsync(int id, bool includeAccounts)
        {
            try
            {
                if (includeAccounts) // accounts should be included
                {
                    return await _db.Clients.Include(b => b.Accounts).FirstOrDefaultAsync(i => i.Id == id);
                }

                // Accounts should be excluded
                return await _db.Clients.FindAsync(id);
            }
            catch (Exception)
            {
                return null;
            }

        }

        public async Task<Client?> AddClientAsync(Client client)
        {
            try
            {
                await _db.Clients.AddAsync(client);
                await _db.SaveChangesAsync();
                return await _db.Clients.FindAsync(client.Id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Client?> UpdateClientAsync(Client client)
        {
            try
            {
                _db.Entry(client).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return client;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<(bool, string)> DeleteClientAsync(Client client)
        {
            try
            {
                var dbClient = await _db.Clients.FindAsync(client.Id);

                if (dbClient == null)
                {
                    return (false, "Client could not be found");
                }

                _db.Clients.Remove(client);
                await _db.SaveChangesAsync();

                return (true, "Client got deleted.");
            }
            catch (Exception)
            {
                return (false, "Client could not be deleted");
            }
        }

    }
}