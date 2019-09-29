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
        /// On user team name changed event
        /// </summary>
        event UserTeamNameChangedDelegate OnUserTeamNameChanged;

        /// <summary>
        /// On user chat message received event
        /// </summary>
        event UserChatMessageReceivedDelegate OnUserChatMessageReceived;

        /// <summary>
        /// On user chat action received event
        /// </summary>
        event UserChatActionReceivedDelegate OnUserChatActionReceived;

        /// <summary>
        /// On game chat message received event
        /// </summary>
        event GameChatMessageReceivedDelegate OnGameChatMessageReceived;

        /// <summary>
        /// On user level update event
        /// </summary>
        event UserLevelUpdateDelegate OnUserLevelUpdate;

        /// <summary>
        /// On classic mode add lines event
        /// </summary>
        event ClassicModeAddLinesDelegate OnClassicModeAddLines;

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
        /// On pause game event
        /// </summary>
        event PauseGameDelegate OnPauseGame;

        /// <summary>
        /// On resume game event
        /// </summary>
        event ResumeGameDelegate OnResumeGame;

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
