using System.Threading.Tasks;
using TetriEngine.Networking;

/// <summary>
/// TetriEngine client namespace
/// </summary>
namespace TetriEngine.Client
{
    /// <summary>
    /// Client class
    /// </summary>
    public static class Client
    {
        /// <summary>
        /// Create local lobby
        /// </summary>
        /// <param name="maxUsers">Maximal amount of users</param>
        /// <returns>Lobby</returns>
        public static ILobby CreateLocalLobby(uint maxUsers) => new LocalLobby(maxUsers);

        /// <summary>
        /// Create local lobby
        /// </summary>
        /// <returns>Lobby</returns>
        public static ILobby CreateLocalLobby() => CreateLocalLobby(uint.MaxValue);

        /// <summary>
        /// Join multiplayer lobby (asynchronous)
        /// </summary>
        /// <param name="host">Host</param>
        /// <param name="port">Port</param>
        /// <param name="username">Username</param>
        /// <param name="teamName">Team name</param>
        /// <returns></returns>
        public static Task<ILobby> JoinMultiplayerLobbyAsync(string host, ushort port, string username, string teamName)
        {
            Task<ILobby> ret = new Task<ILobby>(() =>
            {
                ILobby lobby = null;
                ClientConnection client_connection = Connector.ConnectClientAsync(host, port, username, teamName).GetAwaiter().GetResult();
                if (client_connection != null)
                {
                    lobby = new MultiplayerLobby(client_connection);
                }
                return lobby;
            });
            ret.Start();
            return ret;
        }
    }
}
