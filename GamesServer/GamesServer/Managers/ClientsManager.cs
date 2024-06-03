using GamesServer.Models;

namespace GamesServer.Managers
{
    public interface IClientsManager
    {
        public void UserConnected (int userId, string displayname, string connectionId);
        public void UserDisconnected (string connectionId);
        public WSClient GetClient(int userId);
        public WSClient GetClientByConnectionId(string connectionId);
        public List<WSClient> GetConnectedClients();
    }
    public class ClientsManager : IClientsManager
    {
        ILogger<IClientsManager> _logger;
        private List<WSClient> Clients;

        public ClientsManager(ILogger<ClientsManager> logger)
        {
            _logger = logger;
            Clients = new List<WSClient>();
        }

        public WSClient GetClient(int userId)
        {
            WSClient client = Clients.FirstOrDefault(c => c.UserId == userId);
            if (client == null)
            {
                _logger.LogWarning($"(GetClient): Couldn't find userId={userId}");
                return null;
            }
            return client;
        }

        public WSClient GetClientByConnectionId(string connectionId)
        {
            WSClient client = Clients.FirstOrDefault(c => c.ConnectionId == connectionId);
            if (client == null)
            {
                _logger.LogWarning($"(GetClient): Couldn't find connectionId={connectionId}");
                return null;
            }
            return client;
        }

        public List<WSClient> GetConnectedClients()
        {
            return Clients;
        }

        public void UserConnected(int userId, string displayname, string connectionId)
        {
            WSClient client = Clients.FirstOrDefault(c => c.UserId == userId);
            if (client != null)
            {
                client.ConnectionId = connectionId;
                _logger.LogInformation($"userId: {userId} reconnected with connectionId: {connectionId}");
                return;
            }
            _logger.LogInformation($"userId: {userId} connected with connectionId: {connectionId}");
            Clients.Add(new WSClient { ConnectionId = connectionId, UserId = userId, DisplayName = displayname });
        }

        public void UserDisconnected(string connectionId)
        {
            {
                WSClient client = Clients.FirstOrDefault(c => c.ConnectionId == connectionId);
                if (client == null)
                {
                    _logger.LogError($"(UserDisconnected): connectionId: {connectionId} was not found");
                    return;
                }
                Clients.Remove(client);
            }
        }
    }
}
