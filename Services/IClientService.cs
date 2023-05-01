using AtmCoroBain.Models;

namespace AtmCoroBain.Services
{
    public interface IClientService
    {
        // Client Services
        Task<List<Client>?> GetClientsAsync();
        Task<Client?> GetClientAsync(int id, bool includedAccounts);
        Task<Client?> AddClientAsync(Client client);
        Task<Client?> UpdateClientAsync(Client client);
        Task<(bool, string)> DeleteClientAsync(Client client);
    }
}