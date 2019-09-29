using TetriEngine.Networking.Lobby.Client;

/// <summary>
/// TetriEngine client namespace
/// </summary>
namespace TetriEngine.Client
{
    /// <summary>
    /// Client lobby interface
    /// </summary>
    public interface IClientLobby : IHostLobby
    {
        /// <summary>
        /// On client joined
        /// </summary>
        event ClientJoinedDelegate OnClientJoined;

        /// <summary>
        /// On winlist received
        /// </summary>
        event WinlistReceivedDelegate OnWinlistReceived;

        /// <summary>
        /// On server chat message received
        /// </summary>
        event ServerChatMessageReceivedDelegate OnServerChatMessageReceived;

        /// <summary>
        /// On server chat action received event
        /// </summary>
        event ServerChatActionReceivedDelegate OnServerChatActionReceived;

        /// <summary>
        /// On game is already in progress event
        /// </summary>
        event GameIsAlreadyInProgressDelegate OnGameIsAlreadyInProgress;

        /// <summary>
        /// On request client information event
        /// </summary>
        event RequestClientInformationDelegate OnRequestClientInformation;

        /// <summary>
        /// Send chat message
        /// </summary>
        /// <param name="message">Message</param>
        void SendChatMessage(string message);

        /// <summary>
        /// Send chat action
        /// </summary>
        /// <param name="action">Action</param>
        void SendChatAction(string action);

        /// <summary>
        /// Send game chat message
        /// </summary>
        /// <param name="message">Message</param>
        void SendGameChatMessage(string message);
    }
}
