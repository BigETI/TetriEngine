using TetriEngine.Networking;
using TetriEngine.Networking.Connection.Client;

/// <summary>
/// TetriEngine client namespace
/// </summary>
namespace TetriEngine.Client
{
    /// <summary>
    /// Client connection interface
    /// </summary>
    public interface IClientConnection : IConnection
    {
        /// <summary>
        /// Is connected
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Username
        /// </summary>
        string Username { get; }

        /// <summary>
        /// Team name
        /// </summary>
        string TeamName { get; }

        /// <summary>
        /// On client joined event
        /// </summary>
        event ClientJoinedDelegate OnClientJoined;
    }
}
