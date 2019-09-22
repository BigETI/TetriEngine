using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TetriEngine.Networking;
using TetriEngine.Networking.Connection;

/// <summary>
/// TetriEngine server namespace
/// </summary>
namespace TetriEngine.Server
{
    // TODO
    /// <summary>
    /// Server connection class
    /// </summary>
    public class ServerConnection //: IServerConnection
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
        /// Listener thread
        /// </summary>
        private Thread listenerThread;

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
        ///// On winlist received
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
        ///// On server chat Action received
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

        ///// <summary>
        ///// On pause game
        ///// </summary>
        //public event PauseGameDelegate OnPauseGame;

        ///// <summary>
        ///// On resume game
        ///// </summary>
        //public event ResumeGameDelegate OnResumeGame;

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
        ///// On user full field update
        ///// </summary>
        //public event UserFullFieldUpdateDelegate OnUserFullFieldUpdate;

        ///// <summary>
        ///// On user partial field update
        ///// </summary>
        //public event UserPartialFieldUpdateDelegate OnUserPartialFieldUpdate;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tcpClient">TCP client</param>
        /// <param name="protocol">Protocol</param>
        internal ServerConnection(TcpClient tcpClient, EProtocol protocol)
        {
            this.tcpClient = tcpClient;
            tcpClientNetworkStream = tcpClient.GetStream();
            Protocol = protocol;
            listenerThread = new Thread((that) =>
            {
                if (that is ServerConnection)
                {
                    /*ServerConnection server_connection = (ServerConnection)that;
                    while (server_connection.CanReceive)
                    {
                        lock (server_connection.tcpListener)
                        {
                            TcpClient tcp_client = server_connection.tcpListener.AcceptTcpClient();
                            if (tcp_client != null)
                            {
                                // TODO
                                // Implement bans

                                if (OnClientAwaitingConnection == null)
                                {
                                    tcp_client.Dispose();
                                }
                                else
                                {
                                    tcp_client.ReceiveBufferSize = Connector.defaultReceiveBufferSize;
                                    tcp_client.SendBufferSize = Connector.defaultSendBufferSize;
                                    tcp_client.ReceiveTimeout = Connector.defaultReceiveTimeout;
                                    tcp_client.SendTimeout = Connector.defaultSendTimeout;
                                    tcp_client.NoDelay = true;
                                    OnClientAwaitingConnection.Invoke(tcp_client);
                                }
                            }
                        }
                    }*/
                }
            });
            listenerThread.Start(this);
        }

        private Task<SendResult> SendMessageAsync(NetworkStream networkStream, string message)
        {
            Task<SendResult> ret = new Task<SendResult>(() =>
            {
                SendResult send_result = null;
                string error_message = "Can not send message.";
                if (networkStream.CanWrite)
                {
                    lock (networkStream)
                    {
                        byte[] data = Encoding.UTF8.GetBytes(message);
                        networkStream.Write(data, 0, data.Length);
                        networkStream.WriteByte(0xFF);
                        networkStream.Flush();
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
        /// <param name="networkStream">Network stream</param>
        /// <param name="userID">User ID</param>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendPlayerNumberMessageAsync(NetworkStream networkStream, int userID) => SendMessageAsync(networkStream, ((Protocol == EProtocol.TetriFast) ? ")#)(!@(*3 " : "playernum ") + userID);

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            //if (tcpListener != null)
            //{
            //    tcpListener.Stop();
            //    tcpListener = null;
            //}
            //if (listenerThread != null)
            //{
            //    listenerThread.Join();
            //}

            // TODO
            throw new NotImplementedException();
        }
    }
}
