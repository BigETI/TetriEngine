using System.Net;
using TetriEngine.Networking;
using TetriEngine.Networking.Connection.Server;

/// <summary>
/// TetriEngine server namespace
/// </summary>
namespace TetriEngine.Server
{
    /// <summary>
    /// Server connection interface
    /// </summary>
    public interface IServerConnection : IConnection
    {
        /// <summary>
        /// IP address
        /// </summary>
        IPAddress IPAddress { get; }

        /// <summary>
        /// On client log in
        /// </summary>
        event ClientLogInDelegate OnClientLogIn;

        /// <summary>
        /// On client information received
        /// </summary>
        event ClientInformationReceivedDelegate OnClientInformationReceived;

        /// <summary>
        /// On start game
        /// </summary>
        event StartGameDelegate OnStartGame;

        /// <summary>
        /// On stop game
        /// </summary>
        event StopGameDelegate OnStopGame;
    }
}
