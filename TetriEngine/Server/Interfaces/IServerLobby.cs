using TetriEngine.Networking.Lobby.Server;

/// <summary>
/// TetriEngine server namespace
/// </summary>
namespace TetriEngine.Server
{
    /// <summary>
    /// Server lobby interface
    /// </summary>
    public interface IServerLobby : IHostLobby
    {
        /// <summary>
        /// Whitelist
        /// </summary>
        PatternCollection Whitelist { get; }

        /// <summary>
        /// Bans
        /// </summary>
        PatternCollection Bans { get; }

        /// <summary>
        /// On client log in event
        /// </summary>
        event ClientLogInDelegate OnClientLogIn;

        /// <summary>
        /// On client information received event
        /// </summary>
        event ClientInformationReceivedDelegate OnClientInformationReceived;

        /// <summary>
        /// On winlist sent event
        /// </summary>
        event WinlistSentDelegate OnWinlistSent;

        /// <summary>
        /// On server chat message sent event
        /// </summary>
        event ServerChatMessageSentDelegate OnServerChatMessageSent;

        /// <summary>
        /// On server chat action sent event
        /// </summary>
        event ServerChatActionSentDelegate OnServerChatActionSent;

        /// <summary>
        /// On game is already in progress sent event
        /// </summary>
        event GameIsAlreadyInProgressSentDelegate OnGameIsAlreadyInProgressSent;

        /// <summary>
        /// On client information sent event
        /// </summary>
        event RequestClientInformationSentDelegate OnClientInformationSent;

        /// <summary>
        /// On connection denied sent event
        /// </summary>
        event ConnectionDeniedSentDelegate OnConnectionDenialSent;

        /// <summary>
        /// On start game requested event
        /// </summary>
        event StartGameRequestedDelegate OnStartGameRequested;

        /// <summary>
        /// On stop game requested event
        /// </summary>
        event StopGameRequestedDelegate OnStopGameRequested;

        /// <summary>
        /// On user kicked event
        /// </summary>
        event UserKickedDelegate OnUserKicked;

        /// <summary>
        /// On user banned event
        /// </summary>
        event UserBannedDelegate OnUserBanned;

        /// <summary>
        /// On user timed out event
        /// </summary>
        event UserTimedOutDelegate OnUserTimedOut;
    }
}
