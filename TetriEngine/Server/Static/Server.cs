using System.Threading.Tasks;
using TetriEngine.Networking;

/// <summary>
/// TetriEngine server namespace
/// </summary>
namespace TetriEngine.Server
{
    /// <summary>
    /// Server class
    /// </summary>
    public static class Server
    {
        /// <summary>
        /// Create server lobby
        /// </summary>
        /// <param name="protocol">Protocol</param>
        /// <param name="port">Port</param>
        /// <param name="maxUsers">Maximal amount of users</param>
        /// <returns>Host lobby task</returns>
        public static Task<IHostLobby> CreateServerLobby(EProtocol protocol, ushort port, uint maxUsers)
        {
            Task<IHostLobby> ret = Task.FromResult<IHostLobby>(null);
            if (protocol != EProtocol.Unspecified)
            {
                ret = new Task<IHostLobby>(() =>
                {
                    ServerLobby server_lobby = null;
                    ushort p = ((port > 0) ? port : Connector.defaultPort);
                    uint max_users = ((maxUsers > 0) ? maxUsers : Connector.defaultMaxUsers);
                    ServerListener server_listener = Connector.CreateListenerAsync(protocol, p, max_users).GetAwaiter().GetResult();
                    if (server_listener != null)
                    {
                        server_lobby = new ServerLobby(server_listener, max_users);
                    }
                    return server_lobby;
                });
                ret.Start();
            }
            return ret;
        }

        /// <summary>
        /// Create server lobby
        /// </summary>
        /// <param name="protocol">Protocol</param>
        /// <param name="port">Port</param>
        /// <returns>Lobby task</returns>
        public static Task<IHostLobby> CreateServerLobby(EProtocol protocol, ushort port) => CreateServerLobby(protocol, port, 0U);

        /// <summary>
        /// Create server lobby
        /// </summary>
        /// <param name="protocol">Protocol</param>
        /// <returns>Lobby task</returns>
        public static Task<IHostLobby> CreateServerLobby(EProtocol protocol) => CreateServerLobby(protocol, 0, 0U);
    }
}
