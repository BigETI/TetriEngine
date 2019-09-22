using System;
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
        /// <returns>Lobby task</returns>
        public static Task<ILobby> CreateServerLobby(EProtocol protocol, ushort port, uint maxUsers)
        {
            //Task<ILobby> ret = Task.FromResult<ILobby>(null);
            //if (protocol != EProtocol.Unspecified)
            //{
            //    ret = new Task<ILobby>(() =>
            //    {
            //        ILobby lobby = null;
            //        ServerListener server_listener = Connector.CreateListenerAsync(protocol, port, maxUsers).GetAwaiter().GetResult();
            //        if (server_listener != null)
            //        {
            //            lobby = new ServerLobby(server_listener, maxUsers);
            //        }
            //        return lobby;
            //    });
            //    ret.Start();
            //}
            //return ret;

            // TODO
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create server lobby
        /// </summary>
        /// <param name="protocol">Protocol</param>
        /// <param name="port">Port</param>
        /// <returns>Lobby task</returns>
        public static Task<ILobby> CreateServerLobby(EProtocol protocol, ushort port) => CreateServerLobby(protocol, port, 0U);

        /// <summary>
        /// Create server lobby
        /// </summary>
        /// <param name="protocol">Protocol</param>
        /// <returns>Lobby task</returns>
        public static Task<ILobby> CreateServerLobby(EProtocol protocol) => CreateServerLobby(protocol, 0, 0U);
    }
}
