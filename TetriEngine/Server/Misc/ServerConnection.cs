using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TetriEngine.Networking;
using TetriEngine.Networking.Connection;
using TetriEngine.Networking.Connection.Server;

/// <summary>
/// TetriEngine server namespace
/// </summary>
namespace TetriEngine.Server
{
    /// <summary>
    /// Server connection class
    /// </summary>
    public class ServerConnection : IServerConnection
    {
        /// <summary>
        /// TCP listener
        /// </summary>
        private TcpClient tcpClient;

        /// <summary>
        /// TCP client network stream
        /// </summary>
        private NetworkStream tcpClientNetworkStream;

        /// <summary>
        /// IP address
        /// </summary>
        public IPAddress IPAddress => ((tcpClient == null) ? null : ((IPEndPoint)(tcpClient.Client.RemoteEndPoint)).Address);

        /// <summary>
        /// Listener thread
        /// </summary>
        private Thread listenerThread;

        /// <summary>
        /// Message parser
        /// </summary>
        private MessageParser messageParser;

        /// <summary>
        /// Message builder
        /// </summary>
        private MessageBuilder messageBuilder;

        /// <summary>
        /// Can receive
        /// </summary>
        public bool CanReceive => (tcpClient != null);

        /// <summary>
        /// Can send
        /// </summary>
        public bool CanSend => (tcpClient != null);

        /// <summary>
        /// Protocol
        /// </summary>
        public EProtocol Protocol { get; private set; }

        /// <summary>
        /// Is heart beat supported
        /// </summary>
        public bool IsHeartbeatSupported => true;

        /// <summary>
        /// On user team name changed
        /// </summary>
        public event UserTeamNameChangedDelegate OnUserTeamNameChanged;

        /// <summary>
        /// On user chat message received
        /// </summary>
        public event UserChatMessageReceivedDelegate OnUserChatMessageReceived;

        /// <summary>
        /// On user chat action received
        /// </summary>
        public event UserChatActionReceivedDelegate OnUserChatActionReceived;

        /// <summary>
        /// On game chat message received
        /// </summary>
        public event GameChatMessageReceivedDelegate OnGameChatMessageReceived;

        /// <summary>
        /// On user level update
        /// </summary>
        public event UserLevelUpdateDelegate OnUserLevelUpdate;

        /// <summary>
        /// On classic mode add lines
        /// </summary>
        public event ClassicModeAddLinesDelegate OnClassicModeAddLines;

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
        /// On pause game
        /// </summary>
        public event PauseGameDelegate OnPauseGame;

        /// <summary>
        /// On resume game
        /// </summary>
        public event ResumeGameDelegate OnResumeGame;

        /// <summary>
        /// On user full field update
        /// </summary>
        public event UserFullFieldUpdateDelegate OnUserFullFieldUpdate;

        /// <summary>
        /// On user partial field update
        /// </summary>
        public event UserPartialFieldUpdateDelegate OnUserPartialFieldUpdate;

        /// <summary>
        /// On lient log in
        /// </summary>
        public event ClientLogInDelegate OnClientLogIn;

        /// <summary>
        /// On client information received
        /// </summary>
        public event ClientInformationReceivedDelegate OnClientInformationReceived;

        /// <summary>
        /// On start game
        /// </summary>
        public event StartGameDelegate OnStartGame;

        /// <summary>
        /// On stop game
        /// </summary>
        public event StopGameDelegate OnStopGame;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tcpClient">TCP client</param>
        /// <param name="protocol">Protocol</param>
        internal ServerConnection(TcpClient tcpClient, EProtocol protocol)
        {
            this.tcpClient = tcpClient;
            messageParser = new MessageParser(protocol);
            messageBuilder = new MessageBuilder(protocol);
            tcpClientNetworkStream = tcpClient.GetStream();
            Protocol = protocol;
            messageParser.OnUserTeamNameChanged += OnUserTeamNameChanged;
            messageParser.OnUserChatMessageReceived += OnUserChatMessageReceived;
            messageParser.OnUserChatActionReceived += OnUserChatActionReceived;
            messageParser.OnGameChatMessageReceived += OnGameChatMessageReceived;
            messageParser.OnUserLevelUpdate += OnUserLevelUpdate;
            messageParser.OnClassicModeAddLines += OnClassicModeAddLines;
            messageParser.OnUserSpecialUsedForAll += OnUserSpecialUsedForAll;
            messageParser.OnUserSpecialUsed += OnUserSpecialUsed;
            messageParser.OnUserLost += OnUserLost;
            messageParser.OnPauseGame += OnPauseGame;
            messageParser.OnResumeGame += OnResumeGame;
            messageParser.OnUserFullFieldUpdate += OnUserFullFieldUpdate;
            messageParser.OnUserPartialFieldUpdate += OnUserPartialFieldUpdate;
            messageParser.OnClientLogIn += OnClientLogIn;
            messageParser.OnClientInformationReceived += OnClientInformationReceived;
            messageParser.OnStartGame += OnStartGame;
            messageParser.OnStopGame += OnStopGame;
            listenerThread = new Thread((that) =>
            {
                if (that is ServerConnection)
                {
                    ServerConnection server_connection = (ServerConnection)that;
                    using (MemoryStream buffer = new MemoryStream())
                    {
                        while (server_connection.CanReceive)
                        {
                            lock (server_connection.tcpClient)
                            {
                                int available = server_connection.tcpClient.Available;
                                if (available > 0)
                                {
                                    byte[] available_data = new byte[available];
                                    if (server_connection.tcpClientNetworkStream.Read(available_data, 0, available) == available)
                                    {
                                        foreach (byte b in available_data)
                                        {
                                            if (b == 0xFF)
                                            {
                                                byte[] data = new byte[buffer.Length];
                                                if (buffer.Read(data, 0, data.Length) == data.Length)
                                                {
                                                    messageParser.ParseMessage(Encoding.UTF8.GetString(data));
                                                }
                                                buffer.SetLength(0L);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            });
            listenerThread.Start(this);
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
                if (tcpClientNetworkStream.CanWrite)
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
                return send_result;
            });
            ret.Start();
            return ret;
        }

        /// <summary>
        /// Send player number message (asynchronous)
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendPlayerNumberMessageAsync(IUser user) => SendMessageAsync(messageBuilder.BuildPlayerNumberMessage(user));

        /// <summary>
        /// Send user joined message (asynchronous)
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendUserJoinedMessageAsync(IUser user) => SendMessageAsync(messageBuilder.BuildUserJoinedMessage(user));

        /// <summary>
        /// Send user left message (asynchronous)
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendUserLeftMessageAsync(IUser user) => SendMessageAsync(messageBuilder.BuildUserLeftMessage(user));

        /// <summary>
        /// Send user changed team message (asynchronous)
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendUserChangedTeamMessageAsync(IUser user) => SendMessageAsync(messageBuilder.BuildUserChangedTeamMessage(user));

        /// <summary>
        /// Send winlist message (asynchronous)
        /// </summary>
        /// <param name="winlist">Winlist</param>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendWinlistMessageAsync(Winlist winlist) => SendMessageAsync(messageBuilder.BuildWinlistMessage(winlist));

        /// <summary>
        /// Send chat message (asynchronous)
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="message">Message</param>
        /// <returns>"Send result task"</returns>
        internal Task<SendResult> SendChatMessageMessageAsync(IUser user, string message) => SendMessageAsync(messageBuilder.BuildChatMessageMessage(user, message));

        /// <summary>
        /// Send chat action message (asynchronous)
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="action">Action</param>
        /// <returns>"Send result task"</returns>
        internal Task<SendResult> SendChatActionMessageAsync(IUser user, string action) => SendMessageAsync(messageBuilder.BuildChatActionMessage(user, action));

        /// <summary>
        /// Send game chat message (asynchronous)
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>"Send result task"</returns>
        internal Task<SendResult> SendGameChatMessageMessageAsync(string message) => SendMessageAsync(messageBuilder.BuildGameChatMessageMessage(message));

        /// <summary>
        /// Send game chat message message (asynchronous)
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="message">Message</param>
        /// <returns>"Send result task"</returns>
        internal Task<SendResult> SendGameChatMessageMessageAsync(IUser user, string message) => SendMessageAsync(messageBuilder.BuildGameChatMessageMessage(user, message));

        /// <summary>
        /// Send new game message (asynchronous)
        /// </summary>
        /// <param name="gameOptions">Game options</param>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendNewGameMessageAsync(GameOptions gameOptions) => SendMessageAsync(messageBuilder.BuildNewGameMessage(gameOptions));

        /// <summary>
        /// Send game already in progress message (asynchronous)
        /// </summary>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendGameAlreadyInProgressMessageAsync() => SendMessageAsync(messageBuilder.BuildGameAlreadyInProgressMessage());

        /// <summary>
        /// Send field update message (asynchronous)
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="oldFieldCells">Old field cells</param>
        /// <param name="newFieldCells">New field cells</param>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendFieldUpdateMessageAsync(IUser user, ECell[] oldFieldCells, ECell[] newFieldCells) => SendMessageAsync(messageBuilder.BuildFieldUpdateMessage(user, oldFieldCells, newFieldCells));

        /// <summary>
        /// Send user level update message (asynchronous)
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
        /// Send classic style add lines message (asynchronous)
        /// </summary>
        /// <param name="senderUser">Sender user</param>
        /// <param name="numLines">Number of lines</param>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendClassicStyleAddLinesAsync(IUser senderUser, uint numLines) => SendMessageAsync(messageBuilder.BuildClassicStyleAddLinesMessage(senderUser, numLines));

        /// <summary>
        /// Send user lost message (asynchronous)
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendUserLostMessageAsync(IUser user) => SendMessageAsync(messageBuilder.BuildUserLostMessage(user));

        /// <summary>
        /// Send user won message (asynchronous)
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendUserWonMessageAsync(IUser user) => SendMessageAsync(messageBuilder.BuildUserWonMessage(user));

        /// <summary>
        /// Send pause message (asynchronous)
        /// </summary>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendPauseMessageAsync() => SendMessageAsync(messageBuilder.BuildPauseMessage());

        /// <summary>
        /// Send resume message (asynchronous)
        /// </summary>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendResumeMessageAsync() => SendMessageAsync(messageBuilder.BuildResumeMessage());

        /// <summary>
        /// Send end game message (asynchronous)
        /// </summary>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendEndGameMessageAsync() => SendMessageAsync(messageBuilder.BuildEndGameMessage());

        /// <summary>
        /// Send no connecting message (asynchronous)
        /// </summary>
        /// <param name="reason">Reason</param>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendNoConnectingMessageAsync(string reason) => SendMessageAsync(messageBuilder.BuildNoConnectingMessage(reason));

        /// <summary>
        /// Send client information request message (asynchronous)
        /// </summary>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendClientInfoRequestMessageAsync() => SendMessageAsync(messageBuilder.BuildClientInfoRequestMessage());

        /// <summary>
        /// Send heartbeat message (asynchronous)
        /// </summary>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendHeartbeatMessageAsync() => SendMessageAsync(messageBuilder.BuildHeartbeatMessage());

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (tcpClient != null)
            {
                lock (tcpClient)
                {
                    tcpClient.Dispose();
                }
                tcpClient = null;
                tcpClientNetworkStream = null;
            }
            if (listenerThread != null)
            {
                listenerThread.Join();
                listenerThread = null;
            }
        }
    }
}
