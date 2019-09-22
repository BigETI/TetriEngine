using System;
using System.Collections.Generic;
using TetriEngine.Networking;
using TetriEngine.Networking.Lobby;
using TetriEngine.Networking.Lobby.Client;

/// <summary>
/// TetriEngine client namespace
/// </summary>
namespace TetriEngine.Client
{
    // TODO

    /// <summary>
    /// Lobby class
    /// </summary>
    internal class Lobby //: ILobby
    {
        /// <summary>
        /// User
        /// </summary>
        private User user = new User(1, "Player");

        /// <summary>
        /// Chat messages and actions
        /// </summary>
        private readonly List<ChatMessageAction> chatMessagesActions = new List<ChatMessageAction>();

        /// <summary>
        /// Is game in progress
        /// </summary>
        public bool IsGameInProgress { get; private set; }

        /// <summary>
        /// Is game paused
        /// </summary>
        public bool IsGamePaused { get; private set; }

        /// <summary>
        /// Maximal amount of players
        /// </summary>
        public uint MaxUsers => 1U;

        /// <summary>
        /// User
        /// </summary>
        public IUser User => user;

        /// <summary>
        /// Chat messages and actions
        /// </summary>
        public IReadOnlyList<ChatMessageAction> ChatMessagesActions => chatMessagesActions;

        /// <summary>
        /// Level
        /// </summary>
        public ILevel Level { get; private set; }

        /// <summary>
        /// Winlist
        /// </summary>
        public IWinlist Winlist { get; private set; }

        /// <summary>
        /// On client joined
        /// </summary>
        public event ClientJoinedDelegate OnClientJoined;

        ///// <summary>
        ///// On user joined
        ///// </summary>
        //public event UserJoinedDelegate OnUserJoined;

        ///// <summary>
        ///// On user left
        ///// </summary>
        //public event UserLeftDelegate OnUserLeft;

        ///// <summary>
        ///// On user team name changed
        ///// </summary>
        //public event UserTeamNameChangedDelegate OnUserTeamNameChanged;

        ///// <summary>
        ///// Winlist received event
        ///// </summary>
        //public event WinlistReceivedDelegate OnWinlistReceived;

        ///// <summary>
        ///// On server chat message received
        ///// </summary>
        //public event ServerChatMessageReceivedDelegate OnServerChatMessageReceived;

        ///// <summary>
        ///// On user chat message received
        ///// </summary>
        //public event UserChatMessageReceivedDelegate OnUserChatMessageReceived;

        ///// <summary>
        ///// On server chat action received
        ///// </summary>
        //public event ServerChatActionReceivedDelegate OnServerChatActionReceived;

        ///// <summary>
        ///// On user chat action received
        ///// </summary>
        //public event UserChatActionReceivedDelegate OnUserChatActionReceived;

        ///// <summary>
        ///// On game chat received
        ///// </summary>
        //public event GameChatReceivedDelegate OnGameChatReceived;

        ///// <summary>
        ///// On new game started
        ///// </summary>
        //public event NewGameStartedDelegate OnNewGameStarted;

        ///// <summary>
        ///// On game is already in progress
        ///// </summary>
        //public event GameIsAlreadyInProgressDelegate OnGameIsAlreadyInProgress;

        ///// <summary>
        ///// On request client information
        ///// </summary>
        //public event RequestClientInformationDelegate OnRequestClientInformation;

        ///// <summary>
        ///// On user level update
        ///// </summary>
        //public event UserLevelUpdateDelegate OnUserLevelUpdate;

        ///// <summary>
        ///// On classic mode add lines
        ///// </summary>
        //public event ClassicModeAddLinesDelegate OnClassicModeAddLines;

        ///// <summary>
        ///// On special used for all
        ///// </summary>
        //public event SpecialUsedForAllDelegate OnSpecialUsedForAll;

        ///// <summary>
        ///// On user special used for all
        ///// </summary>
        //public event UserSpecialUsedForAllDelegate OnUserSpecialUsedForAll;

        ///// <summary>
        ///// On server special used
        ///// </summary>
        //public event ServerSpecialUsedDelegate OnServerSpecialUsed;

        ///// <summary>
        ///// On user special used
        ///// </summary>
        //public event UserSpecialUsedDelegate OnUserSpecialUsed;

        ///// <summary>
        ///// On user lost
        ///// </summary>
        //public event UserLostDelegate OnUserLost;

        ///// <summary>
        ///// On user won
        ///// </summary>
        //public event UserWonDelegate OnUserWon;

        /// <summary>
        /// On pause game
        /// </summary>
        public event PauseGameDelegate OnPauseGame;

        /// <summary>
        /// On resume game
        /// </summary>
        public event ResumeGameDelegate OnResumeGame;

        ///// <summary>
        ///// On end game
        ///// </summary>
        //public event EndGameDelegate OnEndGame;

        ///// <summary>
        ///// On connection denied
        ///// </summary>
        //public event ConnectionDeniedDelegate OnConnectionDenied;

        ///// <summary>
        ///// On heart beat
        ///// </summary>
        //public event HeartBeatDelegate OnHeartBeat;

        ///// <summary>
        ///// On user field update
        ///// </summary>
        //public event UserFieldUpdateDelegate OnUserFieldUpdate;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Lobby()
        {
            OnClientJoined?.Invoke(User);
        }

        /// <summary>
        /// Pause game
        /// </summary>
        public void PauseGame()
        {
            if (IsGameInProgress && !IsGamePaused)
            {
                IsGamePaused = true;
                OnPauseGame?.Invoke();
            }
        }

        /// <summary>
        /// Resume game
        /// </summary>
        public void ResumeGame()
        {
            if (IsGameInProgress && IsGamePaused)
            {
                IsGamePaused = false;
                OnResumeGame?.Invoke();
            }
        }

        /// <summary>
        /// Send chat action
        /// </summary>
        /// <param name="action"></param>
        public void SendChatAction(string action)
        {
            if ((action != null) && (User != null))
            {
                string trimmed_action = action.Trim();
                if (trimmed_action.Length > 0)
                {
                    chatMessagesActions.Add(new ChatMessageAction(User, action, true));
                }
            }
        }

        public void SendChatMessage(string message)
        {
            // TODO
            throw new NotImplementedException();
        }

        public void SendGameChatMessage(string message)
        {
            // TODO
            throw new NotImplementedException();
        }

        public void StartGame()
        {
            // TODO
            throw new NotImplementedException();
        }

        public void StopGame()
        {
            // TODO
            throw new NotImplementedException();
        }

        public void Close()
        {
            // ...
        }

        public void Dispose()
        {
            Close();
        }
    }
}
