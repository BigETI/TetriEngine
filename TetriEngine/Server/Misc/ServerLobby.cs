namespace TetriEngine.Server
{
    // TODO
    internal class ServerLobby //: ILobby
    {
        //private ServerConnection serverConnection;

        //private GameManager gameManager = new GameManager();

        //private Pool<NetworkUser> networkUsers;

        //public List<ChatMessageAction> chatMessagesActions = new List<ChatMessageAction>();

        //public bool IsGameInProgress => gameManager.IsInProgress;

        //public bool IsGamePaused => gameManager.IsPaused;

        //public uint MaxUsers { get; private set; }

        //public IUser User { get; } = new User(0, "Server");

        //public IReadOnlyList<ChatMessageAction> ChatMessagesActions => chatMessagesActions;

        //public ILevel Level { get; private set; }

        //public IWinlist Winlist { get; private set; }

        //public event UserJoinedDelegate OnUserJoined;
        //public event UserLeftDelegate OnUserLeft;
        //public event UserTeamNameChangedDelegate OnUserTeamNameChanged;
        //public event WinlistReceivedDelegate OnWinlistReceived;
        //public event ServerChatMessageReceivedDelegate OnServerChatMessageReceived;
        //public event UserChatMessageReceivedDelegate OnUserChatMessageReceived;
        //public event ServerChatActionReceivedDelegate OnServerChatActionReceived;
        //public event UserChatActionReceivedDelegate OnUserChatActionReceived;
        //public event GameChatReceivedDelegate OnGameChatReceived;
        //public event NewGameStartedDelegate OnNewGameStarted;
        //public event GameIsAlreadyInProgressDelegate OnGameIsAlreadyInProgress;
        //public event RequestClientInformationDelegate OnRequestClientInformation;
        //public event UserLevelUpdateDelegate OnUserLevelUpdate;
        //public event ClassicModeAddLinesDelegate OnClassicModeAddLines;
        //public event SpecialUsedForAllDelegate OnSpecialUsedForAll;
        //public event UserSpecialUsedForAllDelegate OnUserSpecialUsedForAll;
        //public event ServerSpecialUsedDelegate OnServerSpecialUsed;
        //public event UserSpecialUsedDelegate OnUserSpecialUsed;
        //public event UserLostDelegate OnUserLost;
        //public event UserWonDelegate OnUserWon;
        //public event PauseGameDelegate OnPauseGame;
        //public event ResumeGameDelegate OnResumeGame;
        //public event EndGameDelegate OnEndGame;
        //public event ConnectionDeniedDelegate OnConnectionDenied;
        //public event HeartBeatDelegate OnHeartBeat;
        //public event UserFieldUpdateDelegate OnUserFieldUpdate;

        //internal ServerLobby(ServerConnection serverConnection, uint maxUsers)
        //{
        //    this.serverConnection = serverConnection;
        //    MaxUsers = maxUsers;
        //    networkUsers = new Pool<NetworkUser>(0, maxUsers);
        //    serverConnection.OnClientAwaitingConnection += ClientAwaitingConnectionEvent;
        //    serverConnection.OnUserJoined += UserJoinedEvent;
        //    serverConnection.OnUserLeft += UserLeftEvent;
        //    serverConnection.OnUserTeamNameChanged += UserTeamNameChangedEvent;
        //    serverConnection.OnWinlistReceived += WinlistReceivedEvent;
        //    serverConnection.OnServerChatMessageReceived += ServerChatMessageReceivedEvent;
        //    serverConnection.OnUserChatMessageReceived += UserChatMessageReceivedEvent;
        //    serverConnection.OnServerChatActionReceived += ServerChatActionReceivedEvent;
        //    serverConnection.OnUserChatActionReceived += UserChatActionReceivedEvent;
        //    serverConnection.OnGameChatReceived += GameChatReceivedEvent;
        //    serverConnection.OnNewGameStarted += NewGameStartedEvent;
        //    serverConnection.OnGameIsAlreadyInProgress += GameIsAlreadyInProgressEvent;
        //    serverConnection.OnRequestClientInformation += RequestClientInformationEvent;
        //    serverConnection.OnUserLevelUpdate += UserLevelUpdateEvent;
        //    serverConnection.OnClassicModeAddLines += ClassicModeAddLinesEvent;
        //    serverConnection.OnSpecialUsedForAll += SpecialUsedForAllEvent;
        //    serverConnection.OnUserSpecialUsedForAll += UserSpecialUsedForAllEvent;
        //    serverConnection.OnServerSpecialUsed += ServerSpecialUsedEvent;
        //    serverConnection.OnUserSpecialUsed += UserSpecialUsedEvent;
        //    serverConnection.OnUserLost += UserLostEvent;
        //    serverConnection.OnUserWon += UserWonEvent;
        //    serverConnection.OnPauseGame += PauseGameEvent;
        //    serverConnection.OnResumeGame += ResumeGameEvent;
        //    serverConnection.OnEndGame += EndGameEvent;
        //    serverConnection.OnConnectionDenied += ConnectionDeniedEvent;
        //    serverConnection.OnHeartBeat += HeartBeatEvent;
        //    serverConnection.OnUserFullFieldUpdate += UserFullFieldUpdateEvent;
        //    serverConnection.OnUserPartialFieldUpdate += UserPartialFieldUpdateEvent;
        //}

        //private void ClientAwaitingConnectionEvent(TcpClient tcpClient)
        //{
        //    int next_available_id = networkUsers.NextAvailableID;
        //    if (next_available_id != networkUsers.InvalidID)
        //    {
        //        NetworkStream network_stream = tcpClient.GetStream();
        //        if (network_stream != null)
        //        {
        //            Thread listener_thread = new Thread((network_user_obj) =>
        //            {
        //                if (network_user_obj is NetworkUser)
        //                {
        //                    NetworkUser network_user = (NetworkUser)network_user_obj;
        //                    using (MemoryStream buffer = new MemoryStream())
        //                    {
        //                        while (network_user.NetworkStream != null)
        //                        {
        //                            lock (network_user.TCPClient)
        //                            {
        //                                int available = network_user.TCPClient.Available;
        //                                if (available > 0)
        //                                {
        //                                    byte[] data = new byte[available];
        //                                    if (network_user.NetworkStream.Read(data, 0, data.Length) == data.Length)
        //                                    {
        //                                        foreach (byte b in data)
        //                                        {
        //                                            if (b == 0xFF)
        //                                            {
        //                                                buffer.Seek(0L, SeekOrigin.Begin);
        //                                                byte[] buffer_data = new byte[buffer.Length];
        //                                                if (buffer.Read(buffer_data, 0, buffer_data.Length) == buffer_data.Length)
        //                                                {
        //                                                    // TODO
        //                                                    ParseMessage(Encoding.UTF8.GetString(buffer_data));
        //                                                }
        //                                                buffer.SetLength(0L);
        //                                            }
        //                                            else
        //                                            {
        //                                                buffer.WriteByte(b);
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            });
        //            NetworkUser user = new NetworkUser(next_available_id, "Unknown", listener_thread, tcpClient, network_stream);
        //            listener_thread.Start(user);
        //        }
        //    }
        //    serverConnection.SendPlayerNumberMessageAsync()
        //}

        //public void Close()
        //{
        //    throw new NotImplementedException();
        //}

        //public void Dispose()
        //{
        //    throw new NotImplementedException();
        //}

        //public void PauseGame()
        //{
        //    throw new NotImplementedException();
        //}

        //public void ResumeGame()
        //{
        //    throw new NotImplementedException();
        //}

        //public void SendChatAction(string action)
        //{
        //    throw new NotImplementedException();
        //}

        //public void SendChatMessage(string message)
        //{
        //    throw new NotImplementedException();
        //}

        //public void SendGameChatMessage(string message)
        //{
        //    throw new NotImplementedException();
        //}

        //public void StartGame()
        //{
        //    throw new NotImplementedException();
        //}

        //public void StopGame()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
