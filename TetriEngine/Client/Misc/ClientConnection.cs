using System;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TetriEngine.Networking;
using TetriEngine.Networking.Connection;
using TetriEngine.Networking.Connection.Client;

/// <summary>
/// TetriEngine client namespace
/// </summary>
namespace TetriEngine.Client
{
    /// <summary>
    /// Client connection class
    /// </summary>
    internal class ClientConnection : IClientConnection
    {
        /// <summary>
        /// Disconnected result task
        /// </summary>
        private static readonly Task<SendResult> disconnectedTetriResultTask = Task.FromResult(new SendResult(EResult.Failed, "Disconnected from host."));

        /// <summary>
        /// Receive tick time
        /// </summary>
        private static readonly int recieveTickTime = 20;

        /// <summary>
        /// TCP client
        /// </summary>
        private TcpClient tcpClient;

        /// <summary>
        /// IPv4 address
        /// </summary>
        private byte[] ipv4Address;

        /// <summary>
        /// TCP client network stream
        /// </summary>
        private NetworkStream tcpClientNetworkStream;

        /// <summary>
        /// Random
        /// </summary>
        private Random random = new Random();

        /// <summary>
        /// Read thread
        /// </summary>
        private Thread readThread;

        /// <summary>
        /// Message parser
        /// </summary>
        private MessageParser messageParser;

        /// <summary>
        /// Message builder
        /// </summary>
        private MessageBuilder messageBuilder;

        /// <summary>
        /// Client name
        /// </summary>
        private static string ClientName => Assembly.GetExecutingAssembly().FullName;

        /// <summary>
        /// Client version
        /// </summary>
        private static string ClientVersion => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        /// <summary>
        /// Is connected
        /// </summary>
        public bool IsConnected => ((tcpClient == null) ? false : tcpClient.Connected);

        /// <summary>
        /// Can receive
        /// </summary>
        public bool CanReceive => (IsConnected ? ((tcpClientNetworkStream == null) ? false : tcpClientNetworkStream.CanRead) : false);

        /// <summary>
        /// Can send
        /// </summary>
        public bool CanSend => (IsConnected ? ((tcpClientNetworkStream == null) ? false : tcpClientNetworkStream.CanWrite) : false);

        /// <summary>
        /// Protocol
        /// </summary>
        public EProtocol Protocol { get; private set; } = EProtocol.Unspecified;

        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// Team name
        /// </summary>
        public string TeamName { get; private set; }

        /// <summary>
        /// Is heartbeat supported
        /// </summary>
        public bool IsHeartbeatSupported { get; private set; }

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
        public event GameChatMessageReceivedDelegate OnGameChatMessageReceived;

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
        public event ServerSpecialUsedForAllDelegate OnServerSpecialUsedForAll;

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
        public event HeartBeatReceivedDelegate OnHeartBeatReceived;

        /// <summary>
        /// On user full field update event
        /// </summary>
        public event UserFullFieldUpdateDelegate OnUserFullFieldUpdate;

        /// <summary>
        /// On user partial field update event
        /// </summary>
        public event UserPartialFieldUpdateDelegate OnUserPartialFieldUpdate;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tcpClient">TCP client</param>
        /// <param name="ipv4Address">IPv4 address</param>
        /// <param name="username">Username</param>
        /// <param name="teamName">Team name</param>
        internal ClientConnection(TcpClient tcpClient, byte[] ipv4Address, string username, string teamName)
        {
            this.tcpClient = tcpClient;
            messageParser = new MessageParser(EProtocol.Unspecified);
            messageBuilder = new MessageBuilder(EProtocol.Unspecified);
            tcpClientNetworkStream = tcpClient.GetStream();
            this.ipv4Address = ipv4Address;
            Username = username;
            TeamName = teamName;
            messageParser.OnClientJoined += ClientJoinedEvent;
            messageParser.OnUserJoined += OnUserJoined;
            messageParser.OnUserLeft += OnUserLeft;
            messageParser.OnUserTeamNameChanged += OnUserTeamNameChanged;
            messageParser.OnWinlistReceived += OnWinlistReceived;
            messageParser.OnServerChatMessageReceived += OnServerChatMessageReceived;
            messageParser.OnUserChatMessageReceived += OnUserChatMessageReceived;
            messageParser.OnServerChatActionReceived += OnServerChatActionReceived;
            messageParser.OnUserChatActionReceived += OnUserChatActionReceived;
            messageParser.OnGameChatMessageReceived += OnGameChatMessageReceived;
            messageParser.OnNewGameStarted += OnNewGameStarted;
            messageParser.OnGameIsAlreadyInProgress += OnGameIsAlreadyInProgress;
            messageParser.OnRequestClientInformation += RequestClientInformationEvent;
            messageParser.OnUserLevelUpdate += OnUserLevelUpdate;
            messageParser.OnClassicModeAddLines += OnClassicModeAddLines;
            messageParser.OnServerSpecialUsedForAll += OnServerSpecialUsedForAll;
            messageParser.OnUserSpecialUsedForAll += OnUserSpecialUsedForAll;
            messageParser.OnServerSpecialUsed += OnServerSpecialUsed;
            messageParser.OnUserSpecialUsed += OnUserSpecialUsed;
            messageParser.OnUserLost += OnUserLost;
            messageParser.OnUserWon += OnUserWon;
            messageParser.OnPauseGame += OnPauseGame;
            messageParser.OnResumeGame += OnResumeGame;
            messageParser.OnEndGame += OnEndGame;
            messageParser.OnConnectionDenied += OnConnectionDenied;
            messageParser.OnHeartBeat += HeartBeatEvent;
            messageParser.OnUserFullFieldUpdate += OnUserFullFieldUpdate;
            messageParser.OnUserPartialFieldUpdate += OnUserPartialFieldUpdate;
            readThread = new Thread((that) =>
            {
                try
                {
                    if (that is ClientConnection)
                    {
                        ClientConnection client_connection = (ClientConnection)that;
                        using (MemoryStream buffer = new MemoryStream())
                        {
                            while (client_connection.CanReceive)
                            {
                                lock (client_connection.tcpClientNetworkStream)
                                {
                                    int available = client_connection.tcpClient.Available;
                                    if (available > 0)
                                    {
                                        byte[] data = new byte[available];
                                        if (client_connection.tcpClientNetworkStream.Read(data, 0, data.Length) == data.Length)
                                        {
                                            foreach (byte b in data)
                                            {
                                                if (b == 0xFF)
                                                {
                                                    buffer.Seek(0L, SeekOrigin.Begin);
                                                    byte[] buffer_data = new byte[buffer.Length];
                                                    if (buffer.Read(buffer_data, 0, buffer_data.Length) == buffer_data.Length)
                                                    {
                                                        client_connection.messageParser.ParseMessage(Encoding.UTF8.GetString(buffer_data));
                                                    }
                                                    buffer.SetLength(0L);
                                                }
                                                else
                                                {
                                                    buffer.WriteByte(b);
                                                }
                                            }
                                        }
                                    }
                                }
                                Thread.Sleep(recieveTickTime);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e);
                }
            });
            readThread.Start(this);
        }

        /// <summary>
        /// Client joined event
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <param name="protocol">Protocol</param>
        private void ClientJoinedEvent(int userID, EProtocol protocol)
        {
            if (Protocol == EProtocol.Unspecified)
            {
                Protocol = protocol;
                SendLogInMessageAsync();
                OnClientJoined?.Invoke(userID, protocol);
            }
        }

        /// <summary>
        /// Request client information event
        /// </summary>
        private void RequestClientInformationEvent()
        {
            SendClientInformationMessageAsync();
            OnRequestClientInformation?.Invoke();
        }

        /// <summary>
        /// Heart beat event
        /// </summary>
        private void HeartBeatEvent()
        {
            IsHeartbeatSupported = true;
            OnHeartBeatReceived?.Invoke();
        }

        /// <summary>
        /// Encode message
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>Encoded message</returns>
        private string EncodeMessage(string message)
        {
            StringBuilder encoded_message = new StringBuilder();
            if (ipv4Address != null)
            {
                if (ipv4Address.Length == 4)
                {
                    string encoded_ipv4_address = ((54 * ipv4Address[0]) + (41 * ipv4Address[1]) + (29 * ipv4Address[2]) + (17 * ipv4Address[3])).ToString();
                    int b = random.Next(256);
                    encoded_message.Append(b.ToString("X2"));
                    for (int i = 0; i < message.Length; i++)
                    {
                        b = ((b + message[i]) % 255) ^ encoded_ipv4_address[i % encoded_ipv4_address.Length];
                        encoded_message.Append(b.ToString("X2"));
                    }
                }
            }
            return encoded_message.ToString();
        }

        /// <summary>
        /// Send message (asynchronous)
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>Send result task</returns>
        private Task<SendResult> SendMessageAsync(string message)
        {
            Task<SendResult> ret = new Task<SendResult>(() =>
            {
                SendResult send_result = null;
                string error_message = "Can not send message.";
                if (CanSend)
                {
                    lock (tcpClientNetworkStream)
                    {
                        byte[] data = Encoding.UTF8.GetBytes(message);
                        tcpClientNetworkStream.Write(data, 0, data.Length);
                        tcpClientNetworkStream.WriteByte(0xFF);
                        tcpClientNetworkStream.Flush();
                        send_result = new SendResult(EResult.Successful, string.Empty);
                    }
                }
                if (send_result == null)
                {
                    send_result = new SendResult(EResult.Failed, error_message);
                }
                return new SendResult(EResult.Successful, null);
            });
            ret.Start();
            return ret;
        }

        /// <summary>
        /// Send log in message (asynchronous)
        /// </summary>
        /// <returns>Send result task</returns>
        private Task<SendResult> SendLogInMessageAsync()
        {
            Task<SendResult> ret = disconnectedTetriResultTask;
            if ((Username != null) && (Protocol != EProtocol.Unspecified))
            {
                ret = SendMessageAsync(EncodeMessage(messageBuilder.BuildLogInMessage(Username)));
            }
            return ret;
        }

        /// <summary>
        /// Send client information message (asynchronous)
        /// </summary>
        /// <returns>Send result task</returns>
        private Task<SendResult> SendClientInformationMessageAsync() => SendMessageAsync(messageBuilder.BuildClientInformationMessage(ClientName, ClientVersion));

        /// <summary>
        /// Send chat message (asynchronous)
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="message"></param>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendChatMessageMessageAsync(IUser user, string message) => SendMessageAsync(messageBuilder.BuildChatMessageMessage(user, message));

        /// <summary>
        /// Send chat action (asynchronous)
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="action">Action</param>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendChatActionMessageAsync(IUser user, string action) => SendMessageAsync(messageBuilder.BuildChatActionMessage(user, action));

        /// <summary>
        /// Send game chat message message (asynchronous)
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendGameChatMessageMessageAsync(IUser user, string message) => SendMessageAsync(messageBuilder.BuildGameChatMessageMessage(user, message));

        /// <summary>
        /// Send start game (asynchronous)
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendStartGameMessageAsync(IUser user) => SendMessageAsync(messageBuilder.BuildStartGameMessage(user));

        /// <summary>
        /// Send stop game message (asynchronous)
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendStopGameMessageAsync(IUser user) => SendMessageAsync(messageBuilder.BuildStopGameMessage(user));

        /// <summary>
        /// Send level update message (asynchronous)
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendLevelUpdateMessageAsync(IUser user) => SendMessageAsync(messageBuilder.BuildLevelUpdateMessage(user));

        /// <summary>
        /// Send special used message (asynchronous)
        /// </summary>
        /// <param name="senderUser">Sender user</param>
        /// <param name="targetUser">Target user</param>
        /// <param name="special">Special</param>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendSpecialUsedMessageAsync(IUser senderUser, IUser targetUser, ESpecial special) => SendMessageAsync(messageBuilder.BuildSpecialUsedMessage(senderUser, targetUser, special));

        /// <summary>
        /// Send classic style add line message (asynchronous)
        /// </summary>
        /// <param name="senderUser">Sender user</param>
        /// <param name="numLines">Number of lines</param>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendClassicStyleAddLinesMessageAsync(IUser senderUser, uint numLines) => SendMessageAsync(messageBuilder.BuildClassicStyleAddLinesMessage(senderUser, numLines));

        /// <summary>
        /// Send user lost message (asynchronous)
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendUserLostMessageAsync(IUser user) => SendMessageAsync(messageBuilder.BuildUserLostMessage(user));

        /// <summary>
        /// Send pause request message (asynchronous)
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendPauseRequestMessageAsync(IUser user) => SendMessageAsync(messageBuilder.BuildPauseRequestMessage(user));

        /// <summary>
        /// Send resume request message (asynchronous)
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendResumeRequestMessageAsync(IUser user) => SendMessageAsync(messageBuilder.BuildResumeRequestMessage(user));

        /// <summary>
        /// Send field update message (asynchronous)
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="oldFieldCells">Old field cells</param>
        /// <param name="newFieldCells">New field cells</param>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendFieldUpdateMessageAsync(IUser user, ECell[] oldFieldCells, ECell[] newFieldCells) => SendMessageAsync(messageBuilder.BuildFieldUpdateMessage(user, oldFieldCells, newFieldCells));

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (tcpClient != null)
            {
                tcpClient.Dispose();
                tcpClient = null;
                tcpClientNetworkStream = null;
            }
        }
    }
}
