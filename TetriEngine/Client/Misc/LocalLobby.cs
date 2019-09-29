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
    /// <summary>
    /// Local lobby class
    /// </summary>
    internal class LocalLobby : IHostLobby
    {
        /// <summary>
        /// User
        /// </summary>
        private User user;

        /// <summary>
        /// User
        /// </summary>
        private Pool<IUser> users;

        /// <summary>
        /// Chat messages and actions
        /// </summary>
        private readonly List<ChatMessageAction> chatMessagesActions = new List<ChatMessageAction>();

        /// <summary>
        /// Game manager
        /// </summary>
        private GameManager gameManager;

        /// <summary>
        /// Is game in progress
        /// </summary>
        public bool IsGameInProgress => (gameManager != null);

        /// <summary>
        /// Is game paused
        /// </summary>
        public bool IsGamePaused => ((gameManager == null) ? true : gameManager.IsPaused);

        /// <summary>
        /// Maximal amount of players
        /// </summary>
        public uint MaxUsers => users.MaxEntries;

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
        public IGameManager GameManager => gameManager;

        /// <summary>
        /// Users
        /// </summary>
        public IReadOnlyPool<IUser> Users => users;

        /// <summary>
        /// Winlist
        /// </summary>
        public Winlist Winlist { get; private set; }

        /// <summary>
        /// On client joined
        /// </summary>
        public event ClientJoinedDelegate OnClientJoined;

        /// <summary>
        /// On user joined
        /// </summary>
        public event UserJoinedDelegate OnUserJoined;

        /// <summary>
        /// On user left
        /// </summary>
        public event UserLeftDelegate OnUserLeft;

        /// <summary>
        /// On user team name changed
        /// </summary>
        public event UserTeamNameChangedDelegate OnUserTeamNameChanged;

        /// <summary>
        /// Winlist received event
        /// </summary>
        public event WinlistReceivedDelegate OnWinlistReceived;

        /// <summary>
        /// On server chat message received
        /// </summary>
        public event ServerChatMessageReceivedDelegate OnServerChatMessageReceived;

        /// <summary>
        /// On user chat message received
        /// </summary>
        public event UserChatMessageReceivedDelegate OnUserChatMessageReceived;

        /// <summary>
        /// On server chat action received
        /// </summary>
        public event ServerChatActionReceivedDelegate OnServerChatActionReceived;

        /// <summary>
        /// On user chat action received
        /// </summary>
        public event UserChatActionReceivedDelegate OnUserChatActionReceived;

        /// <summary>
        /// On game chat received
        /// </summary>
        public event GameChatMessageReceivedDelegate OnGameChatMessageReceived;

        /// <summary>
        /// On new game started
        /// </summary>
        public event NewGameStartedDelegate OnNewGameStarted;

        /// <summary>
        /// On game is already in progress
        /// </summary>
        public event GameIsAlreadyInProgressDelegate OnGameIsAlreadyInProgress;

        /// <summary>
        /// On request client information
        /// </summary>
        public event RequestClientInformationDelegate OnRequestClientInformation;

        /// <summary>
        /// On user level update
        /// </summary>
        public event UserLevelUpdateDelegate OnUserLevelUpdate;

        /// <summary>
        /// On classic mode add lines
        /// </summary>
        public event ClassicModeAddLinesDelegate OnClassicModeAddLines;

        /// <summary>
        /// On special used for all
        /// </summary>
        public event SpecialUsedForAllDelegate OnSpecialUsedForAll;

        /// <summary>
        /// On user special used for all
        /// </summary>
        public event UserSpecialUsedForAllDelegate OnUserSpecialUsedForAll;

        /// <summary>
        /// On server special used
        /// </summary>
        public event ServerSpecialUsedDelegate OnServerSpecialUsed;

        /// <summary>
        /// On user special used
        /// </summary>
        public event UserSpecialUsedDelegate OnUserSpecialUsed;

        /// <summary>
        /// On user lost
        /// </summary>
        public event UserLostDelegate OnUserLost;

        /// <summary>
        /// On user won
        /// </summary>
        public event UserWonDelegate OnUserWon;

        /// <summary>
        /// On pause game
        /// </summary>
        public event PauseGameDelegate OnPauseGame;

        /// <summary>
        /// On resume game
        /// </summary>
        public event ResumeGameDelegate OnResumeGame;

        /// <summary>
        /// On end game
        /// </summary>
        public event EndGameDelegate OnEndGame;

        /// <summary>
        /// On connection denied
        /// </summary>
        public event ConnectionDeniedDelegate OnConnectionDenied;

        /// <summary>
        /// On heart beat
        /// </summary>
        public event HeartBeatDelegate OnHeartBeat;

        /// <summary>
        /// On user field update
        /// </summary>
        public event UserFieldUpdateDelegate OnUserFieldUpdate;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="maxUsers">Maximal amount of users</param>
        public LocalLobby(uint maxUsers)
        {
            users = new Pool<IUser>(0, maxUsers);
        }

        /// <summary>
        /// Send chat action
        /// </summary>
        /// <param name="action">Action</param>
        public void SendChatAction(string action)
        {
            if ((action != null) && (User != null))
            {
                string trimmed_action = action.Trim();
                if (trimmed_action.Length > 0)
                {
                    chatMessagesActions.Add(new ChatMessageAction(User, trimmed_action, true));
                    OnUserChatActionReceived?.Invoke(User, trimmed_action);
                }
            }
        }

        /// <summary>
        /// Send chat message
        /// </summary>
        /// <param name="message">Message</param>
        public void SendChatMessage(string message)
        {
            if ((message != null) && (User != null))
            {
                string trimmed_action = message.Trim();
                if (trimmed_action.Length > 0)
                {
                    chatMessagesActions.Add(new ChatMessageAction(User, message, false));
                    OnUserChatMessageReceived?.Invoke(User, trimmed_action);
                }
            }
        }

        /// <summary>
        /// Send game chat message
        /// </summary>
        /// <param name="message">Message</param>
        public void SendGameChatMessage(string message)
        {
            if ((message != null) && (gameManager != null))
            {
                string trimmed_message = message.Trim();
                if (trimmed_message.Length > 0)
                {
                    if (gameManager.AddGameChatMessage(user, trimmed_message))
                    {
                        OnGameChatMessageReceived?.Invoke(trimmed_message);
                    }
                }
            }
        }

        /// <summary>
        /// Close
        /// </summary>
        public void Close()
        {
            // ...
            if (gameManager != null)
            {
                gameManager = null;
                OnEndGame?.Invoke();
            }
        }

        /// <summary>
        /// Start game
        /// </summary>
        /// <param name="startingHeight">Starting height</param>
        /// <param name="startingLevel">Starting level</param>
        /// <param name="linesPerLevel">Lines per level</param>
        /// <param name="levelIncrement">Level increment</param>
        /// <param name="linesPerSpecial">Lines per special</param>
        /// <param name="specialsAdded">Specials added</param>
        /// <param name="specialCapacity">Special capacity</param>
        /// <param name="blockFrequencies">Block frequencies</param>
        /// <param name="specialFrequencies">Special frequencies</param>
        /// <param name="displayAverageLevel">Display average level</param>
        /// <param name="classicMode">Classic mode</param>
        /// <returns>Game manager</returns>
        public IGameManager StartGame(uint startingHeight, uint startingLevel, uint linesPerLevel, uint levelIncrement, uint linesPerSpecial, uint specialsAdded, uint specialCapacity, IReadOnlyList<EBlock> blockFrequencies, IReadOnlyList<ESpecial> specialFrequencies, bool displayAverageLevel, bool classicMode)
        {
            if (gameManager == null)
            {
                gameManager = new GameManager(user, new GameOptions(startingHeight, startingLevel, linesPerLevel, levelIncrement, linesPerSpecial, specialsAdded, specialCapacity, blockFrequencies, specialFrequencies, displayAverageLevel, classicMode));
                OnNewGameStarted?.Invoke(gameManager);
            }
            return gameManager;
        }

        // TODO
        // Set apropriate values
        
        /// <summary>
        /// Start game
        /// </summary>
        /// <returns>Game manager</returns>
        public IGameManager StartGame() => StartGame(0U, 1U, 1U, 1U, 1U, 1U, 8U, Array.Empty<EBlock>(), Array.Empty<ESpecial>(), false, false);

        /// <summary>
        /// Stop game
        /// </summary>
        public bool StopGame()
        {
            bool ret = (gameManager != null);
            if (ret)
            {
                gameManager = null;
                OnEndGame?.Invoke();
            }
            return ret;
        }

        /// <summary>
        /// Add bot user
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>User</returns>
        public IUser AddBotUser(string username)
        {
            IUser ret = null;
            if (username != null)
            {
                string trimmed_username = username.Trim();
                if (trimmed_username.Length > 0)
                {
                    int user_id = users.NextAvailableID;
                    if (user_id != users.InvalidID)
                    {
                        ret = new BotUser(user_id, username);
                        users.Add(ret);
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Dispose();
        /// </summary>
        public void Dispose()
        {
            Close();
        }
    }
}
