using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TetriEngine.Networking;
using TetriEngine.Networking.Lobby;
using TetriEngine.Networking.Lobby.Server;

/// <summary>
/// TetriEngine server namespace
/// </summary>
namespace TetriEngine.Server
{
    /// <summary>
    /// Server lobby class
    /// </summary>
    internal class ServerLobby : IServerLobby
    {
        /// <summary>
        /// Server listener
        /// </summary>
        private ServerListener serverListener;

        /// <summary>
        /// User
        /// </summary>
        private User user = new User(0, "Server");

        /// <summary>
        /// Game manager
        /// </summary>
        private GameManager gameManager;

        /// <summary>
        /// Chat messages or actions
        /// </summary>
        private List<ChatMessageAction> chatMessageActions;

        /// <summary>
        /// Users
        /// </summary>
        private Pool<IUser> users;

        /// <summary>
        /// Whitelist
        /// </summary>
        public PatternCollection Whitelist => serverListener.Whitelist;

        /// <summary>
        /// Bans
        /// </summary>
        public PatternCollection Bans => serverListener.Bans;

        /// <summary>
        /// Is game in progress
        /// </summary>
        public bool IsGameInProgress => (gameManager != null);

        /// <summary>
        /// Is game paused
        /// </summary>
        public bool IsGamePaused => ((gameManager == null) ? true : gameManager.IsPaused);

        /// <summary>
        /// Maximal amount of users
        /// </summary>
        public uint MaxUsers => users.MaxEntries;

        /// <summary>
        /// User
        /// </summary>
        public IUser User => user;

        /// <summary>
        /// Chat messages actions
        /// </summary>
        public IReadOnlyList<ChatMessageAction> ChatMessagesActions => chatMessageActions;

        /// <summary>
        /// Game manager
        /// </summary>
        public IGameManager GameManager => gameManager;

        /// <summary>
        /// Users
        /// </summary>
        public IReadOnlyPool<IUser> Users => users;

        /// <summary>
        /// Winlist
        /// </summary>
        public Winlist Winlist => ((gameManager == null) ? null : gameManager.Winlist);

        /// <summary>
        /// On user joined event
        /// </summary>
        public event UserJoinedDelegate OnUserJoined;

        /// <summary>
        /// On user left event
        /// </summary>
        public event UserLeftDelegate OnUserLeft;

        /// <summary>
        /// On user kicked event
        /// </summary>
        public event UserKickedDelegate OnUserKicked;

        /// <summary>
        /// On user banned event
        /// </summary>
        public event UserBannedDelegate OnUserBanned;

        /// <summary>
        /// On user timed out event
        /// </summary>
        public event UserTimedOutDelegate OnUserTimedOut;

        /// <summary>
        /// On user team name changed event
        /// </summary>
        public event UserTeamNameChangedDelegate OnUserTeamNameChanged;

        /// <summary>
        /// On user chat message received event
        /// </summary>
        public event UserChatMessageReceivedDelegate OnUserChatMessageReceived;

        /// <summary>
        /// On user chat action received event
        /// </summary>
        public event UserChatActionReceivedDelegate OnUserChatActionReceived;

        /// <summary>
        /// On game chat message received event
        /// </summary>
        public event GameChatMessageReceivedDelegate OnGameChatMessageReceived;

        /// <summary>
        /// On new game started event
        /// </summary>
        public event NewGameStartedDelegate OnNewGameStarted;

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
        /// On heart beat event
        /// </summary>
        public event HeartBeatDelegate OnHeartBeat;

        /// <summary>
        /// On user field update event
        /// </summary>
        public event UserFieldUpdateDelegate OnUserFieldUpdate;

        /// <summary>
        /// On client log in event
        /// </summary>
        public event ClientLogInDelegate OnClientLogIn;

        /// <summary>
        /// On client information received event
        /// </summary>
        public event ClientInformationReceivedDelegate OnClientInformationReceived;

        /// <summary>
        /// On winlist sent event
        /// </summary>
        public event WinlistSentDelegate OnWinlistSent;

        /// <summary>
        /// On server chat message sent event
        /// </summary>
        public event ServerChatMessageSentDelegate OnServerChatMessageSent;

        /// <summary>
        /// On server chat action sent event
        /// </summary>
        public event ServerChatActionSentDelegate OnServerChatActionSent;

        /// <summary>
        /// On game is already in progress sent event
        /// </summary>
        public event GameIsAlreadyInProgressSentDelegate OnGameIsAlreadyInProgressSent;

        /// <summary>
        /// On client information sent event
        /// </summary>
        public event RequestClientInformationSentDelegate OnClientInformationSent;

        /// <summary>
        /// On connection denied sent event
        /// </summary>
        public event ConnectionDeniedSentDelegate OnConnectionDenialSent;

        /// <summary>
        /// On start game requested event
        /// </summary>
        public event StartGameRequestedDelegate OnStartGameRequested;

        /// <summary>
        /// On stop game requested event
        /// </summary>
        public event StopGameRequestedDelegate OnStopGameRequested;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serverListener">Server listener</param>
        /// <param name="maxUsers">Maximal amount of users</param>
        internal ServerLobby(ServerListener serverListener, uint maxUsers)
        {
            this.serverListener = serverListener;
            users = new Pool<IUser>(0, maxUsers);
            serverListener.OnClientConnectionAccepted += ClientConnectionAcceptedEvent;
        }

        /// <summary>
        /// Client connection accepted event
        /// </summary>
        /// <param name="serverConnection">Server connection</param>
        private void ClientConnectionAcceptedEvent(ServerConnection serverConnection)
        {
            lock (users)
            {
                int user_id = users.NextAvailableID;
                if (user_id == users.InvalidID)
                {
                    serverListener.Disconnect(serverConnection);
                }
                else
                {
                    ServerUser user = new ServerUser(user_id, "Unknown", serverConnection);
                    users.Add(user);
                    serverConnection.SendPlayerNumberMessageAsync(user);
                }
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
                gameManager.OnBlockMoved += BlockMovedEvent;
                gameManager.OnBlockLanded += LandBlockEvent;
                gameManager.OnNewBlockSelected += NewBlockSelectedEvent;
                gameManager.OnGameLost += LooseGameEvent;
            }
            return gameManager;
        }

        /// <summary>
        /// Move block 
        /// </summary>
        /// <param name="block"></param>
        private void BlockMovedEvent(EBlock block)
        {
            // TODO
            throw new NotImplementedException();
        }

        private void LandBlockEvent(EBlock block)
        {
            // TODO
            throw new NotImplementedException();
        }

        private void NewBlockSelectedEvent(EBlock block)
        {
            // TODO
            throw new NotImplementedException();
        }

        private void LooseGameEvent(EBlock block)
        {
            // TODO
            throw new NotImplementedException();
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
        /// <returns>"true" if successful, otherwise "false"</returns>
        public bool StopGame()
        {
            bool ret = (gameManager != null);
            if (ret)
            {
                OnEndGame?.Invoke();
                foreach (IUser user in users)
                {
                    if (user is ServerUser)
                    {
                        ((ServerUser)user).ServerConnection.SendWinlistMessageAsync(Winlist);
                    }
                }
                gameManager = null;
            }
            return ret;
        }

        /// <summary>
        /// Add bot user
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>User if successful, otherwise "null"</returns>
        public IUser AddBotUser(string username)
        {
            IUser ret = null;
            lock (users)
            {
                int user_id = users.NextAvailableID;
                if ((username != null) && (user_id != users.InvalidID))
                {
                    string trimmed_username = username.Trim();
                    if (trimmed_username.Length > 0)
                    {
                        ret = new BotUser(user_id, username);
                        if (users.Add(ret) == users.InvalidID)
                        {
                            ret = null;
                        }
                        else
                        {
                            foreach (IUser user in users)
                            {
                                if (user is ServerUser)
                                {
                                    ((ServerUser)user).ServerConnection.SendUserJoinedMessageAsync(ret);
                                }
                            }
                        }
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Is real user
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>"true" if user is real, otherwise "false"</returns>
        private bool IsRealUser(IUser user) => (users.IsIDValid(user.ID) || (user.ID == this.user.ID));

        /// <summary>
        /// Get real user
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Real user</returns>
        private IUser GetRealUser(IUser user) => (users.IsIDValid(user.ID)) ? users[user.ID] : ((user.ID == this.user.ID) ? this.user : null);

        /// <summary>
        /// Send chat message
        /// </summary>
        /// <param name="senderUser">Sender user</param>
        /// <param name="targetUser">Target user</param>
        /// <param name="message">Message</param>
        public void SendChatMessage(IUser senderUser, IUser targetUser, string message)
        {
            if ((senderUser != null) && (targetUser != null) && (message != null))
            {
                string trimmed_message = message.Trim();
                if (trimmed_message.Length > 0)
                {
                    lock (users)
                    {
                        if (IsRealUser(senderUser) && users.IsIDValid(targetUser.ID))
                        {
                            IUser sender_user = GetRealUser(senderUser);
                            ServerUser target_user = users[targetUser.ID] as ServerUser;
                            if (target_user != null)
                            {
                                target_user.ServerConnection.SendChatMessageMessageAsync(sender_user, trimmed_message);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Send chat message to all
        /// </summary>
        /// <param name="senderUser">Sender user</param>
        /// <param name="message">Message</param>
        public void SendChatMessageToAll(IUser senderUser, string message)
        {
            if ((senderUser != null) && (message != null))
            {
                string trimmed_message = message.Trim();
                if (trimmed_message.Length > 0)
                {
                    lock (users)
                    {
                        if (IsRealUser(senderUser))
                        {
                            IUser sender_user = GetRealUser(senderUser);
                            foreach (IUser user in users)
                            {
                                ServerUser target_user = user as ServerUser;
                                if (target_user != null)
                                {
                                    target_user.ServerConnection.SendChatMessageMessageAsync(sender_user, trimmed_message);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Send chat action
        /// </summary>
        /// <param name="senderUser">Sender user</param>
        /// <param name="targetUser">Target user</param>
        /// <param name="action">Action</param>
        public void SendChatAction(IUser senderUser, IUser targetUser, string action)
        {
            if ((senderUser != null) && (targetUser != null) && (action != null))
            {
                string trimmed_action = action.Trim();
                if (trimmed_action.Length > 0)
                {
                    lock (users)
                    {
                        if (IsRealUser(senderUser) && users.IsIDValid(targetUser.ID))
                        {
                            IUser sender_user = GetRealUser(senderUser);
                            ServerUser target_user = users[targetUser.ID] as ServerUser;
                            if (target_user != null)
                            {
                                target_user.ServerConnection.SendChatActionMessageAsync(sender_user, trimmed_action);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Send chat action to all
        /// </summary>
        /// <param name="senderUser">Sender user</param>
        /// <param name="action">Action</param>
        public void SendChatActionToAll(IUser senderUser, string action)
        {
            if ((senderUser != null) && (action != null))
            {
                string trimmed_action = action.Trim();
                if (trimmed_action.Length > 0)
                {
                    lock (users)
                    {
                        if (IsRealUser(senderUser))
                        {
                            IUser sender_user = GetRealUser(senderUser);
                            foreach (IUser user in users)
                            {
                                ServerUser target_user = user as ServerUser;
                                if (target_user != null)
                                {
                                    target_user.ServerConnection.SendChatActionMessageAsync(sender_user, trimmed_action);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Send game chat message
        /// </summary>
        /// <param name="senderUser">Sender user</param>
        /// <param name="targetUser">Target user</param>
        /// <param name="message">Message</param>
        public void SendGameChatMessageToAll(IUser senderUser, IUser targetUser, string message)
        {
            if ((senderUser != null) && (targetUser != null) && (message != null))
            {
                string trimmed_message = message.Trim();
                if (trimmed_message.Length > 0)
                {
                    lock (users)
                    {
                        if (IsRealUser(senderUser) && users.IsIDValid(targetUser.ID))
                        {
                            IUser sender_user = GetRealUser(senderUser);
                            ServerUser target_user = users[targetUser.ID] as ServerUser;
                            if (target_user != null)
                            {
                                target_user.ServerConnection.SendGameChatMessageMessageAsync(sender_user, trimmed_message);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Send game chat message
        /// </summary>
        /// <param name="senderUser">Sender user</param>
        /// <param name="message">Message</param>
        public void SendGameChatMessageToAll(IUser senderUser, string message)
        {
            if ((senderUser != null) && (message != null))
            {
                string trimmed_message = message.Trim();
                if (trimmed_message.Length > 0)
                {
                    lock (users)
                    {
                        if (IsRealUser(senderUser))
                        {
                            IUser sender_user = GetRealUser(senderUser);
                            foreach (IUser user in users)
                            {
                                ServerUser target_user = user as ServerUser;
                                if (target_user != null)
                                {
                                    target_user.ServerConnection.SendGameChatMessageMessageAsync(sender_user, trimmed_message);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Send game chat message
        /// </summary>
        /// <param name="targetUser">Target user</param>
        /// <param name="message">Message</param>
        public void SendGameChatMessage(IUser targetUser, string message)
        {
            if ((targetUser != null) && (message != null))
            {
                string trimmed_message = message.Trim();
                if (trimmed_message.Length > 0)
                {
                    lock (users)
                    {
                        if (users.IsIDValid(targetUser.ID))
                        {
                            ServerUser target_user = targetUser as ServerUser;
                            if (target_user != null)
                            {
                                target_user.ServerConnection.SendGameChatMessageMessageAsync(trimmed_message);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Disconnect user
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="disconnectReason">Disconnect reason</param>
        /// <returns>"true" if disconnected, otherwise "false"</returns>
        public bool DisconnectUser(IUser user, EDisconnectReason disconnectReason)
        {
            bool ret = false;
            if (user != null)
            {
                lock (users)
                {
                    if (IsRealUser(user))
                    {
                        IUser real_user = GetRealUser(user);
                        if (real_user != null)
                        {
                            if (real_user is ServerUser)
                            {
                                ServerUser server_user = (ServerUser)real_user;
                                ret = (disconnectReason == (EDisconnectReason.Banned) ? Bans.AddPattern(Regex.Escape(server_user.ServerConnection.IPAddress.ToString())) : true);
                                serverListener.Disconnect(server_user.ServerConnection);
                            }
                            else if (real_user is BotUser)
                            {
                                ret = true;
                            }
                            if (ret)
                            {
                                users.Remove(real_user.ID);
                                switch (disconnectReason)
                                {
                                    case EDisconnectReason.Left:
                                        OnUserLeft?.Invoke(real_user);
                                        break;
                                    case EDisconnectReason.Kicked:
                                        OnUserKicked?.Invoke(real_user);
                                        break;
                                    case EDisconnectReason.Banned:
                                        OnUserBanned?.Invoke(real_user);
                                        break;
                                    case EDisconnectReason.TimedOut:
                                        OnUserTimedOut?.Invoke(real_user);
                                        break;
                                }
                                foreach (IUser u in users)
                                {
                                    ServerUser server_user = u as ServerUser;
                                    if (server_user != null)
                                    {
                                        server_user.ServerConnection.SendUserLeftMessageAsync(real_user);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Close lobby
        /// </summary>
        public void Close()
        {
            lock (users)
            {
                foreach (IUser user in users)
                {
                    if (user is ServerUser)
                    {
                        ((ServerUser)user).ServerConnection.Dispose();
                    }
                }
                users.Clear();
            }
            if (serverListener != null)
            {
                lock (serverListener)
                {
                    serverListener.Dispose();
                    serverListener = null;
                }
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
