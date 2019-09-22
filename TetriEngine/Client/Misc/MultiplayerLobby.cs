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
    /// Multiplayer lobby class
    /// </summary>
    internal class MultiplayerLobby : ILobby
    {
        /// <summary>
        /// Server user
        /// </summary>
        private static readonly IUser serverUser = new User(0, "Server");

        /// <summary>
        /// Users
        /// </summary>
        private readonly Pool<User> users;

        /// <summary>
        /// Winlist
        /// </summary>
        public Winlist winlist;

        /// <summary>
        /// Chat messages
        /// </summary>
        private readonly List<ChatMessageAction> chatMessagesActions = new List<ChatMessageAction>();

        /// <summary>
        /// Client connection
        /// </summary>
        private ClientConnection clientConnection;

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
        public uint MaxUsers => uint.MaxValue;

        /// <summary>
        /// User
        /// </summary>
        public IUser User { get; private set; }

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
        public IWinlist Winlist => winlist;

        /// <summary>
        /// On client joined event
        /// </summary>
        public event ClientJoinedDelegate OnClientJoined;

        /// <summary>
        /// On user joined event
        /// </summary>
        public event UserJoinedDelegate OnUserJoined;

        /// <summary>
        /// On user left event
        /// </summary>
        public event UserLeftDelegate OnUserLeft;

        /// <summary>
        /// On user team name changed event
        /// </summary>
        public event UserTeamNameChangedDelegate OnUserTeamNameChanged;

        /// <summary>
        /// On winlist received
        /// </summary>
        public event WinlistReceivedDelegate OnWinlistReceived;

        /// <summary>
        /// On server chat message received event
        /// </summary>
        public event ServerChatMessageReceivedDelegate OnServerChatMessageReceived;

        /// <summary>
        /// On user chat message received event
        /// </summary>
        public event UserChatMessageReceivedDelegate OnUserChatMessageReceived;

        /// <summary>
        /// On server chat action received event
        /// </summary>
        public event ServerChatActionReceivedDelegate OnServerChatActionReceived;

        /// <summary>
        /// On user chat action received event
        /// </summary>
        public event UserChatActionReceivedDelegate OnUserChatActionReceived;

        /// <summary>
        /// On game chat message received event
        /// </summary>
        public event GameChatReceivedDelegate OnGameChatReceived;

        /// <summary>
        /// On new game started event
        /// </summary>
        public event NewGameStartedDelegate OnNewGameStarted;

        /// <summary>
        /// On game is already in progress event
        /// </summary>
        public event GameIsAlreadyInProgressDelegate OnGameIsAlreadyInProgress;

        /// <summary>
        /// On request client information event
        /// </summary>
        public event RequestClientInformationDelegate OnRequestClientInformation;

        /// <summary>
        /// On user level update event
        /// </summary>
        public event UserLevelUpdateDelegate OnUserLevelUpdate;

        /// <summary>
        /// On classic mode add lines event
        /// </summary>
        public event ClassicModeAddLinesDelegate OnClassicModeAddLines;

        /// <summary>
        /// On special used for all event
        /// </summary>
        public event SpecialUsedForAllDelegate OnSpecialUsedForAll;

        /// <summary>
        /// On user special used for all event
        /// </summary>
        public event UserSpecialUsedForAllDelegate OnUserSpecialUsedForAll;

        /// <summary>
        /// On server special used event
        /// </summary>
        public event ServerSpecialUsedDelegate OnServerSpecialUsed;

        /// <summary>
        /// On user special used event
        /// </summary>
        public event UserSpecialUsedDelegate OnUserSpecialUsed;

        /// <summary>
        /// On user lost event
        /// </summary>
        public event UserLostDelegate OnUserLost;

        /// <summary>
        /// On user won event
        /// </summary>
        public event UserWonDelegate OnUserWon;

        /// <summary>
        /// On pause game event
        /// </summary>
        public event PauseGameDelegate OnPauseGame;

        /// <summary>
        /// On resume game event
        /// </summary>
        public event ResumeGameDelegate OnResumeGame;

        /// <summary>
        /// On end game event
        /// </summary>
        public event EndGameDelegate OnEndGame;

        /// <summary>
        /// On connection denied event
        /// </summary>
        public event ConnectionDeniedDelegate OnConnectionDenied;

        /// <summary>
        /// On heart beat event
        /// </summary>
        public event HeartBeatDelegate OnHeartBeat;

        /// <summary>
        /// On user field update event
        /// </summary>
        public event UserFieldUpdateDelegate OnUserFieldUpdate;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="clientConnection">Client connection</param>
        internal MultiplayerLobby(ClientConnection clientConnection)
        {
            this.clientConnection = clientConnection;
            users = new Pool<User>(0, MaxUsers);
            clientConnection.OnClientJoined += ClientJoinedEvent;
            clientConnection.OnUserJoined += UserJoinedEvent;
            clientConnection.OnUserLeft += UserLeftEvent;
            clientConnection.OnUserTeamNameChanged += UserTeamNameChangedEvent;
            clientConnection.OnWinlistReceived += WinlistReceivedEvent;
            clientConnection.OnServerChatMessageReceived += ServerChatMessageReceivedEvent;
            clientConnection.OnUserChatMessageReceived += UserChatMessageReceivedEvent;
            clientConnection.OnServerChatActionReceived += ServerChatActionReceivedEvent;
            clientConnection.OnUserChatActionReceived += UserChatActionReceivedEvent;
            clientConnection.OnGameChatReceived += GameChatReceivedEvent;
            clientConnection.OnNewGameStarted += NewGameStartedEvent;
            clientConnection.OnGameIsAlreadyInProgress += GameIsAlreadyInProgressEvent;
            clientConnection.OnRequestClientInformation += RequestClientInformationEvent;
            clientConnection.OnUserLevelUpdate += UserLevelUpdateEvent;
            clientConnection.OnClassicModeAddLines += ClassicModeAddLinesEvent;
            clientConnection.OnSpecialUsedForAll += SpecialUsedForAllEvent;
            clientConnection.OnUserSpecialUsedForAll += UserSpecialUsedForAllEvent;
            clientConnection.OnServerSpecialUsed += ServerSpecialUsedEvent;
            clientConnection.OnUserSpecialUsed += UserSpecialUsedEvent;
            clientConnection.OnUserLost += UserLostEvent;
            clientConnection.OnUserWon += UserWonEvent;
            clientConnection.OnPauseGame += PauseGameEvent;
            clientConnection.OnResumeGame += ResumeGameEvent;
            clientConnection.OnEndGame += EndGameEvent;
            clientConnection.OnConnectionDenied += ConnectionDeniedEvent;
            clientConnection.OnHeartBeat += HeartBeatEvent;
            clientConnection.OnUserFullFieldUpdate += UserFullFieldUpdateEvent;
            clientConnection.OnUserPartialFieldUpdate += UserPartialFieldUpdateEvent;
        }

        /// <summary>
        /// Client joined event
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <param name="username">Username</param>
        private void ClientJoinedEvent(int userID, string username)
        {
            if (User == null)
            {
                User user = new User(userID, username);
                User = user;
                if (users.Insert(user, userID))
                {
                    OnClientJoined?.Invoke(User);
                }
            }
        }

        /// <summary>
        /// User joined event
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <param name="username">Username</param>
        private void UserJoinedEvent(int userID, string username)
        {
            if (User != null)
            {
                if (User.ID != userID)
                {
                    User user = new User(userID, username);
                    if (users.Insert(user, userID))
                    {
                        OnUserJoined?.Invoke(user);
                    }
                }
            }
        }

        /// <summary>
        /// User left event
        /// </summary>
        /// <param name="userID">User ID</param>
        private void UserLeftEvent(int userID)
        {
            if (User != null)
            {
                if ((User.ID != userID) && (users.IsIDValid(userID)))
                {
                    IUser user = users[userID];
                    if (users.Remove(userID))
                    {
                        OnUserLeft?.Invoke(user);
                    }
                }
            }
        }

        /// <summary>
        /// User team name changed event
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <param name="newTeamName">New team name</param>
        private void UserTeamNameChangedEvent(int userID, string newTeamName)
        {
            if (users.IsIDValid(userID))
            {
                User user = users[userID];
                string old_team_name = user.TeamName;
                user.TeamName = newTeamName;
                OnUserTeamNameChanged?.Invoke(user, old_team_name, newTeamName);
            }
        }

        /// <summary>
        /// Winlist received event
        /// </summary>
        /// <param name="winlist">Winlist</param>
        private void WinlistReceivedEvent(WinlistEntry[] winlist)
        {
            this.winlist = new Winlist();
            foreach (WinlistEntry winlist_entry in winlist)
            {
                if (winlist_entry.IsTeam)
                {
                    this.winlist.AppendTeam(new Team(winlist_entry.Name, winlist_entry.Score));
                }
                else
                {
                    foreach (User user in users)
                    {
                        if (user.Name == winlist_entry.Name)
                        {
                            user.Score = winlist_entry.Score;
                            this.winlist.AppendUser(user);
                        }
                    }
                }
            }
            OnWinlistReceived?.Invoke(Winlist);
        }

        /// <summary>
        /// Server chat message received event
        /// </summary>
        /// <param name="message">Message</param>
        private void ServerChatMessageReceivedEvent(string message)
        {
            chatMessagesActions.Add(new ChatMessageAction(serverUser, message, false));
            OnServerChatMessageReceived?.Invoke(message);
        }

        /// <summary>
        /// User chat message received
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <param name="message">Message</param>
        private void UserChatMessageReceivedEvent(int userID, string message)
        {
            if (users.IsIDValid(userID))
            {
                IUser user = users[userID];
                chatMessagesActions.Add(new ChatMessageAction(user, message, false));
                OnUserChatMessageReceived?.Invoke(user, message);
            }
        }

        /// <summary>
        /// Server chat action received event
        /// </summary>
        /// <param name="action">Action</param>
        private void ServerChatActionReceivedEvent(string action)
        {
            chatMessagesActions.Add(new ChatMessageAction(serverUser, action, true));
            OnServerChatActionReceived?.Invoke(action);
        }

        /// <summary>
        /// User chat action received event
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <param name="action">Action</param>
        private void UserChatActionReceivedEvent(int userID, string action)
        {
            if (users.IsIDValid(userID))
            {
                IUser user = users[userID];
                chatMessagesActions.Add(new ChatMessageAction(user, action, true));
                OnUserChatActionReceived?.Invoke(user, action);
            }
        }

        /// <summary>
        /// Game chat received event
        /// </summary>
        /// <param name="message">Message</param>
        private void GameChatReceivedEvent(string message)
        {
            chatMessagesActions.Add(new ChatMessageAction(serverUser, message, true));
            OnGameChatReceived?.Invoke(message);
        }

        /// <summary>
        /// New game started event
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
        /// <param name="displayAverageLevels">Display average levels</param>
        /// <param name="classicMode">Classic mode</param>
        private void NewGameStartedEvent(uint startingHeight, uint startingLevel, uint linesPerLevel, uint levelIncrement, uint linesPerSpecial, uint specialsAdded, uint specialCapacity, IReadOnlyList<EBlock> blockFrequencies, IReadOnlyList<ESpecial> specialFrequencies, bool displayAverageLevels, bool classicMode)
        {
            if (Level != null)
            {
                OnEndGame?.Invoke();
            }
            Level = new Level(startingHeight, startingLevel, linesPerLevel, levelIncrement, linesPerSpecial, specialsAdded, specialCapacity, blockFrequencies, specialFrequencies, displayAverageLevels, classicMode);
            IsGameInProgress = true;
            OnNewGameStarted?.Invoke(Level);
        }

        /// <summary>
        /// Game is already in progress event
        /// </summary>
        private void GameIsAlreadyInProgressEvent()
        {
            IsGameInProgress = true;
            OnGameIsAlreadyInProgress?.Invoke();
        }

        /// <summary>
        /// Request client information event
        /// </summary>
        private void RequestClientInformationEvent() => OnRequestClientInformation?.Invoke();

        /// <summary>
        /// User level update event
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <param name="newLevel">New level</param>
        private void UserLevelUpdateEvent(int userID, uint newLevel)
        {
            if (users.IsIDValid(userID))
            {
                User user = users[userID];
                uint old_level = user.Level;
                user.Level = newLevel;
                OnUserLevelUpdate?.Invoke(user, old_level, newLevel);
            }
        }

        /// <summary>
        /// Classic mode add lines event
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <param name="numLines">Number of lines</param>
        private void ClassicModeAddLinesEvent(int userID, uint numLines)
        {
            if (users.IsIDValid(userID))
            {
                OnClassicModeAddLines?.Invoke(users[userID], numLines);
            }
        }

        /// <summary>
        /// Special used for all event
        /// </summary>
        /// <param name="special">Special</param>
        private void SpecialUsedForAllEvent(ESpecial special) => OnSpecialUsedForAll?.Invoke(special);

        /// <summary>
        /// User special used for all event
        /// </summary>
        /// <param name="senderUserID">Sender user ID</param>
        /// <param name="special">Special</param>
        private void UserSpecialUsedForAllEvent(int senderUserID, ESpecial special)
        {
            if (users.IsIDValid(senderUserID))
            {
                OnUserSpecialUsedForAll?.Invoke(users[senderUserID], special);
            }
        }

        /// <summary>
        /// Server special used event
        /// </summary>
        /// <param name="targetUserID">Target user ID</param>
        /// <param name="special">Special</param>
        private void ServerSpecialUsedEvent(int targetUserID, ESpecial special)
        {
            if (users.IsIDValid(targetUserID))
            {
                OnServerSpecialUsed?.Invoke(users[targetUserID], special);
            }
        }

        /// <summary>
        /// User special used event
        /// </summary>
        /// <param name="senderUserID">Sender user ID</param>
        /// <param name="targetUserID">Target user ID</param>
        /// <param name="special">Special</param>
        private void UserSpecialUsedEvent(int senderUserID, int targetUserID, ESpecial special)
        {
            if (users.IsIDValid(senderUserID) && users.IsIDValid(targetUserID))
            {
                OnUserSpecialUsed?.Invoke(users[senderUserID], users[targetUserID], special);
            }
        }

        /// <summary>
        /// User lost event
        /// </summary>
        /// <param name="userID">User ID</param>
        private void UserLostEvent(int userID)
        {
            if (users.IsIDValid(userID))
            {
                OnUserLost?.Invoke(users[userID]);
            }
        }

        /// <summary>
        /// User won event
        /// </summary>
        /// <param name="userID">User ID</param>
        private void UserWonEvent(int userID)
        {
            if (users.IsIDValid(userID))
            {
                OnUserWon?.Invoke(users[userID]);
            }
        }

        /// <summary>
        /// Pause game event
        /// </summary>
        private void PauseGameEvent()
        {
            if (IsGameInProgress && !IsGamePaused)
            {
                IsGamePaused = true;
                OnPauseGame?.Invoke();
            }
        }

        /// <summary>
        /// Resume game event
        /// </summary>
        private void ResumeGameEvent()
        {
            if (IsGameInProgress && IsGamePaused)
            {
                IsGamePaused = false;
                OnResumeGame?.Invoke();
            }
        }

        /// <summary>
        /// End game event
        /// </summary>
        private void EndGameEvent()
        {
            if (IsGameInProgress)
            {
                IsGameInProgress = false;
                IsGamePaused = false;
                OnEndGame?.Invoke();
            }
        }

        /// <summary>
        /// Connection denied event
        /// </summary>
        private void ConnectionDeniedEvent() => OnConnectionDenied?.Invoke();

        /// <summary>
        /// Heart beat event
        /// </summary>
        private void HeartBeatEvent() => OnHeartBeat?.Invoke();

        /// <summary>
        /// User full field update event
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <param name="cells">Cells</param>
        private void UserFullFieldUpdateEvent(int userID, ECell[] cells)
        {
            if (users.IsIDValid(userID))
            {
                User user = users[userID];
                ECell[] old_cells = new ECell[Field.width * Field.height];
                if (user.Field.CopyCellsTo(old_cells))
                {
                    if (user.FieldInternal.UpdateCells(cells))
                    {
                        OnUserFieldUpdate?.Invoke(user, old_cells, cells);
                    }
                }
            }
        }

        /// <summary>
        /// User partial field update event
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <param name="cellPositions">Cell positions</param>
        private void UserPartialFieldUpdateEvent(int userID, CellPosition[] cellPositions)
        {
            if (users.IsIDValid((int)userID))
            {
                User user = users[(int)userID];
                ECell[] old_cells = new ECell[Field.width * Field.height];
                if (user.Field.CopyCellsTo(old_cells))
                {
                    foreach (CellPosition cell_position in cellPositions)
                    {
                        user.FieldInternal.SetCell(cell_position.Cell, (int)(cell_position.X), (int)(cell_position.Y));
                    }
                    ECell[] new_cells = new ECell[old_cells.Length];
                    if (user.Field.CopyCellsTo(new_cells))
                    {
                        OnUserFieldUpdate?.Invoke(user, old_cells, new_cells);
                    }
                }
            }
        }

        /// <summary>
        /// Send chat message
        /// </summary>
        /// <param name="message">Message</param>
        public void SendChatMessage(string message)
        {
            if ((message != null) && (clientConnection != null) && (User != null))
            {
                string trimmed_message = message.Trim();
                if (trimmed_message.Length > 0)
                {
                    clientConnection.SendChatMessageMessageAsync(User.ID, trimmed_message);
                }
            }
        }

        /// <summary>
        /// Send chat action
        /// </summary>
        /// <param name="action">Action</param>
        public void SendChatAction(string action)
        {
            if ((action != null) && (clientConnection != null) && (User != null))
            {
                string trimmed_action = action.Trim();
                if (trimmed_action.Length > 0)
                {
                    clientConnection.SendChatActionMessageAsync(User.ID, trimmed_action);
                }
            }
        }

        /// <summary>
        /// Send game chat action
        /// </summary>
        /// <param name="message">Message</param>
        public void SendGameChatMessage(string message)
        {
            if ((message != null) && (clientConnection != null) && (User != null))
            {
                string trimmed_message = message.Trim();
                if (trimmed_message.Length > 0)
                {
                    clientConnection.SendGameChatMessageMessageAsync(User.Name + " " + trimmed_message);
                }
            }
        }

        /// <summary>
        /// Start game
        /// </summary>
        public void StartGame()
        {
            if ((clientConnection != null) && (User != null))
            {
                clientConnection.SendStartGameMessageAsync(User.ID);
            }
        }

        /// <summary>
        /// Stop game
        /// </summary>
        public void StopGame()
        {
            if ((clientConnection != null) && (User != null))
            {
                clientConnection.SendStopGameMessageAsync(User.ID);
            }
        }

        /// <summary>
        /// Pause game
        /// </summary>
        public void PauseGame()
        {
            if ((clientConnection != null) && (User != null))
            {
                clientConnection.SendPauseGameMessageAsync(User.ID);
            }
        }

        /// <summary>
        /// Resume game
        /// </summary>
        public void ResumeGame()
        {
            if ((clientConnection != null) && (User != null))
            {
                clientConnection.SendResumeGameMessageAsync(User.ID);
            }
        }

        /// <summary>
        /// Close lobby
        /// </summary>
        public void Close()
        {
            if (clientConnection != null)
            {
                clientConnection.Dispose();
                clientConnection = null;
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Close();
        }
    }
}
