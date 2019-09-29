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

        /// <summary>
        /// On user joined event
        /// </summary>
        event UserJoinedDelegate OnUserJoined;

        /// <summary>
        /// On user left event
        /// </summary>
        event UserLeftDelegate OnUserLeft;

        /// <summary>
        /// On winlist received
        /// </summary>
        event WinlistReceivedDelegate OnWinlistReceived;

        /// <summary>
        /// On server chat message received event
        /// </summary>
        event ServerChatMessageReceivedDelegate OnServerChatMessageReceived;

        /// <summary>
        /// On server chat action received event
        /// </summary>
        event ServerChatActionReceivedDelegate OnServerChatActionReceived;

        /// <summary>
        /// On new game started event
        /// </summary>
        event NewGameStartedDelegate OnNewGameStarted;

        /// <summary>
        /// On game is already in progress event
        /// </summary>
        event GameIsAlreadyInProgressDelegate OnGameIsAlreadyInProgress;

        /// <summary>
        /// On request client information event
        /// </summary>
        event RequestClientInformationDelegate OnRequestClientInformation;

        /// <summary>
        /// On special used for all event
        /// </summary>
        event ServerSpecialUsedForAllDelegate OnServerSpecialUsedForAll;

        /// <summary>
        /// On user won event
        /// </summary>
        event UserWonDelegate OnUserWon;

        /// <summary>
        /// On end game event
        /// </summary>
        event EndGameDelegate OnEndGame;

        /// <summary>
        /// On connection denied event
        /// </summary>
        event ConnectionDeniedDelegate OnConnectionDenied;

        /// <summary>
        /// On heart beat event
        /// </summary>
        event HeartBeatReceivedDelegate OnHeartBeatReceived;
    }
}
