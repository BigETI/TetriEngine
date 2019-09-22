using System;
using TetriEngine.Networking.Connection;

/// <summary>
/// TetriEngine networking namespace
/// </summary>
namespace TetriEngine.Networking
{
    /// <summary>
    /// Connection interface
    /// </summary>
    public interface IConnection : IDisposable
    {
        /// <summary>
        /// Can receive
        /// </summary>
        bool CanReceive { get; }

        /// <summary>
        /// Can send
        /// </summary>
        bool CanSend { get; }

        /// <summary>
        /// Protocol
        /// </summary>
        EProtocol Protocol { get; }

        /// <summary>
        /// Is heartbeat supported
        /// </summary>
        bool IsHeartbeatSupported { get; }

        /// <summary>
        /// On user joined event
        /// </summary>
        event UserJoinedDelegate OnUserJoined;

        /// <summary>
        /// On user left event
        /// </summary>
        event UserLeftDelegate OnUserLeft;

        /// <summary>
        /// On user team name changed event
        /// </summary>
        event UserTeamNameChangedDelegate OnUserTeamNameChanged;

        /// <summary>
        /// On winlist received
        /// </summary>
        event WinlistReceivedDelegate OnWinlistReceived;

        /// <summary>
        /// On server chat message received event
        /// </summary>
        event ServerChatMessageReceivedDelegate OnServerChatMessageReceived;

        /// <summary>
        /// On user chat message received event
        /// </summary>
        event UserChatMessageReceivedDelegate OnUserChatMessageReceived;

        /// <summary>
        /// On server chat action received event
        /// </summary>
        event ServerChatActionReceivedDelegate OnServerChatActionReceived;

        /// <summary>
        /// On user chat action received event
        /// </summary>
        event UserChatActionReceivedDelegate OnUserChatActionReceived;

        /// <summary>
        /// On game chat message received event
        /// </summary>
        event GameChatReceivedDelegate OnGameChatReceived;

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
        /// On user level update event
        /// </summary>
        event UserLevelUpdateDelegate OnUserLevelUpdate;

        /// <summary>
        /// On classic mode add lines event
        /// </summary>
        event ClassicModeAddLinesDelegate OnClassicModeAddLines;

        /// <summary>
        /// On special used for all event
        /// </summary>
        event SpecialUsedForAllDelegate OnSpecialUsedForAll;

        /// <summary>
        /// On user special used for all event
        /// </summary>
        event UserSpecialUsedForAllDelegate OnUserSpecialUsedForAll;

        /// <summary>
        /// On server special used event
        /// </summary>
        event ServerSpecialUsedDelegate OnServerSpecialUsed;

        /// <summary>
        /// On user special used event
        /// </summary>
        event UserSpecialUsedDelegate OnUserSpecialUsed;

        /// <summary>
        /// On user lost event
        /// </summary>
        event UserLostDelegate OnUserLost;

        /// <summary>
        /// On user won event
        /// </summary>
        event UserWonDelegate OnUserWon;

        /// <summary>
        /// On pause game event
        /// </summary>
        event PauseGameDelegate OnPauseGame;

        /// <summary>
        /// On resume game event
        /// </summary>
        event ResumeGameDelegate OnResumeGame;

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
        event HeartBeatDelegate OnHeartBeat;

        /// <summary>
        /// On user full field update event
        /// </summary>
        event UserFullFieldUpdateDelegate OnUserFullFieldUpdate;

        /// <summary>
        /// On user partial field update event
        /// </summary>
        event UserPartialFieldUpdateDelegate OnUserPartialFieldUpdate;
    }
}
