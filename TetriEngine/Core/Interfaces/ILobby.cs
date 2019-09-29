using System;
using System.Collections.Generic;
using TetriEngine.Networking;
using TetriEngine.Networking.Lobby;

/// <summary>
/// TetriEngine namespace
/// </summary>
namespace TetriEngine
{
    /// <summary>
    /// Lobby interface
    /// </summary>
    public interface ILobby : IDisposable
    {
        /// <summary>
        /// Is game in progress
        /// </summary>
        bool IsGameInProgress { get; }

        /// <summary>
        /// Is paused
        /// </summary>
        bool IsGamePaused { get; }

        /// <summary>
        /// Maximal amount of players
        /// </summary>
        uint MaxUsers { get; }

        /// <summary>
        /// User
        /// </summary>
        IUser User { get; }

        /// <summary>
        /// Chat messages and actions
        /// </summary>
        IReadOnlyList<ChatMessageAction> ChatMessagesActions { get; }

        /// <summary>
        /// Game manager
        /// </summary>
        IGameManager GameManager { get; }

        /// <summary>
        /// Users
        /// </summary>
        IReadOnlyPool<IUser> Users { get; }

        /// <summary>
        /// Winlist
        /// </summary>
        Winlist Winlist { get; }

        /// <summary>
        /// Close lobby
        /// </summary>
        void Close();

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
        /// On new game started event
        /// </summary>
        event NewGameStartedDelegate OnNewGameStarted;

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
        /// On heart beat event
        /// </summary>
        event HeartBeatDelegate OnHeartBeat;

        /// <summary>
        /// On user field update event
        /// </summary>
        event UserFieldUpdateDelegate OnUserFieldUpdate;
    }
}
