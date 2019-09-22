using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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
        /// Winlist entry regex
        /// </summary>
        private static readonly Regex winlistEntryRegex = new Regex(@"([t,p])(.+?);([0-9]+)");

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
            tcpClientNetworkStream = tcpClient.GetStream();
            this.ipv4Address = ipv4Address;
            Username = username;
            TeamName = teamName;
            readThread = new Thread((that) =>
            {
                try
                {
                    if (that is ClientConnection)
                    {
                        ClientConnection connection = (ClientConnection)that;
                        using (MemoryStream buffer = new MemoryStream())
                        {
                            while (connection.CanReceive)
                            {
                                lock (tcpClientNetworkStream)
                                {
                                    int available = connection.tcpClient.Available;
                                    if (available > 0)
                                    {
                                        byte[] data = new byte[available];
                                        if (tcpClientNetworkStream.Read(data, 0, data.Length) == data.Length)
                                        {
                                            foreach (byte b in data)
                                            {
                                                if (b == 0xFF)
                                                {
                                                    buffer.Seek(0L, SeekOrigin.Begin);
                                                    byte[] buffer_data = new byte[buffer.Length];
                                                    if (buffer.Read(buffer_data, 0, buffer_data.Length) == buffer_data.Length)
                                                    {
                                                        ParseMessage(Encoding.UTF8.GetString(buffer_data));
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
        /// <param name="username">Username</param>
        /// <returns>Send result task</returns>
        private Task<SendResult> SendLogInMessageAsync()
        {
            Task<SendResult> ret = disconnectedTetriResultTask;
            if ((Username != null) && (Protocol != EProtocol.Unspecified))
            {
                ret = SendMessageAsync(EncodeMessage(((Protocol == EProtocol.TetriFast) ? "tetrifaster " : "tetrisstart ") + Username + " 1.13"));
            }
            return ret;
        }

        /// <summary>
        /// Send client information message (asynchronous)
        /// </summary>
        /// <returns>Send result task</returns>
        private Task<SendResult> SendClientInformationMessageAsync() => SendMessageAsync("clientinfo " + ClientName + " " + ClientVersion);

        /// <summary>
        /// Send chat message (asynchronous)
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <param name="message"></param>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendChatMessageMessageAsync(int userID, string message)
        {
            Task<SendResult> ret = disconnectedTetriResultTask;
            if (message != null)
            {
                ret = SendMessageAsync("pline " + userID + " " + message);
            }
            return ret;
        }

        /// <summary>
        /// Send chat action (asynchronous)
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <param name="action">Action</param>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendChatActionMessageAsync(int userID, string action)
        {
            Task<SendResult> ret = disconnectedTetriResultTask;
            if (action != null)
            {
                ret = SendMessageAsync("plineact " + userID + " " + action);
            }
            return ret;
        }

        /// <summary>
        /// Send game chat message message (asynchronous)
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendGameChatMessageMessageAsync(string message)
        {
            Task<SendResult> ret = disconnectedTetriResultTask;
            if (message != null)
            {
                ret = SendMessageAsync("gmsg " + message);
            }
            return ret;
        }

        /// <summary>
        /// Send start game (asynchronous)
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendStartGameMessageAsync(int userID) => SendMessageAsync("startgame 1 " + userID);

        /// <summary>
        /// Send stop game message (asynchronous)
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendStopGameMessageAsync(int userID) => SendMessageAsync("startgame 0 " + userID);

        /// <summary>
        /// Send update level message (asynchronous)
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <param name="level">Level</param>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendUpdateLevelMessageAsync(int userID, uint level)
        {
            Task<SendResult> ret = disconnectedTetriResultTask;
            if (level > 0U)
            {
                ret = SendMessageAsync("lvl " + userID + " " + level);
            }
            return ret;
        }

        /// <summary>
        /// Send special message (asynchronous)
        /// </summary>
        /// <param name="senderUserID">Sender user ID</param>
        /// <param name="targetUserID">Target user ID</param>
        /// <param name="special">Special</param>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendSpecialMessageAsync(int senderUserID, int targetUserID, ESpecial special)
        {
            Task<SendResult> ret = disconnectedTetriResultTask;
            if (special != ESpecial.Nothing)
            {
                char special_char = '\0';
                switch (special)
                {
                    case ESpecial.AddLine:
                        special_char = 'a';
                        break;
                    case ESpecial.ClearLine:
                        special_char = 'c';
                        break;
                    case ESpecial.NukeField:
                        special_char = 'n';
                        break;
                    case ESpecial.RandomBlocksClear:
                        special_char = 'r';
                        break;
                    case ESpecial.SwitchFields:
                        special_char = 's';
                        break;
                    case ESpecial.ClearSpecialBlocks:
                        special_char = 'b';
                        break;
                    case ESpecial.Gravity:
                        special_char = 'g';
                        break;
                    case ESpecial.QuakeField:
                        special_char = 'q';
                        break;
                    case ESpecial.BlockBomb:
                        special_char = 'o';
                        break;
                        // TODO
                        // More special types
                }
                if (special_char != '\0')
                {
                    ret = SendMessageAsync("sb " + targetUserID + " " + special_char + " " + senderUserID);
                }
            }
            return ret;
        }

        /// <summary>
        /// Send classic mode add line message (asynchronous)
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <param name="numLines">Number of lines</param>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendClassicModeAddLineMessageAsync(int userID, uint numLines)
        {
            Task<SendResult> ret = disconnectedTetriResultTask;
            if ((numLines == 1U) || (numLines == 2U) || (numLines == 4U))
            {
                ret = SendMessageAsync("sb 0 cs" + numLines + " " + userID);
            }
            return ret;
        }

        /// <summary>
        /// Send user lost message (asynchronous)
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendUserLostMessageAsync(int userID) => SendMessageAsync("playerlost " + userID);

        /// <summary>
        /// Send pause game message (asynchronous)
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendPauseGameMessageAsync(int userID) => SendMessageAsync("pause 1 " + userID);

        /// <summary>
        /// Send resume game message (asynchronous)
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendResumeGameMessageAsync(int userID) => SendMessageAsync("pause 0 " + userID);

        /// <summary>
        /// Send full field update message (asynchronous)
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <param name="newCells">New cells</param>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendFullFieldUpdateMessageAsync(int userID, ECell[] newCells)
        {
            Task<SendResult> ret = disconnectedTetriResultTask;
            if (newCells.Length == (Field.width * Field.height))
            {
                char[] field_chars = new char[newCells.Length];
                for (int i = 0; i < field_chars.Length; i++)
                {
                    ECell cell = newCells[i];
                    int cell_num = (int)cell;
                    if ((cell_num >= ((int)(ECell.Nothing))) && (cell_num <= ((int)(ECell.Red))))
                    {
                        field_chars[i] = Convert.ToChar(cell_num + Convert.ToByte('0'));
                    }
                    else
                    {
                        switch (cell)
                        {
                            case ECell.AddLine:
                                field_chars[i] = 'a';
                                break;
                            case ECell.ClearLine:
                                field_chars[i] = 'c';
                                break;
                            case ECell.NukeField:
                                field_chars[i] = 'n';
                                break;
                            case ECell.RandomBlocksClear:
                                field_chars[i] = 'r';
                                break;
                            case ECell.SwitchFields:
                                field_chars[i] = 's';
                                break;
                            case ECell.ClearSpecialBlocks:
                                field_chars[i] = 'b';
                                break;
                            case ECell.Gravity:
                                field_chars[i] = 'g';
                                break;
                            case ECell.QuakeField:
                                field_chars[i] = 'q';
                                break;
                            case ECell.BlockBomb:
                                field_chars[i] = 'o';
                                break;
                            case ECell.LeftGravity:
                                field_chars[i] = 'l';
                                break;
                            case ECell.PieceChange:
                                field_chars[i] = 'p';
                                break;
                            case ECell.ZebraField:
                                field_chars[i] = 'z';
                                break;
                                // TODO
                                // More special cells
                        }
                    }
                    if (field_chars[i] == '\0')
                    {
                        field_chars[i] = '0';
                    }
                }
                ret = SendMessageAsync("f " + userID + " " + (new string(field_chars)));
            }
            return ret;
        }

        /// <summary>
        /// Send partial field update message (asynchronous)
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <param name="oldCells">Old cells</param>
        /// <param name="newCells">New cells</param>
        /// <returns>Send result task</returns>
        internal Task<SendResult> SendPartialFieldUpdateMessageAsync(int userID, ECell[] oldCells, ECell[] newCells)
        {
            Task<SendResult> ret = disconnectedTetriResultTask;
            if (newCells.Length == (Field.width * Field.height))
            {
                List<char> field_char_list = new List<char>();
                for (int i = 0; i < oldCells.Length; i++)
                {
                    ECell new_cell = newCells[i];
                    if (oldCells[i] != new_cell)
                    {
                        char cell_char = '0';
                        switch (new_cell)
                        {
                            case ECell.Nothing:
                                cell_char = '!';
                                break;
                            case ECell.Blue:
                                cell_char = '"';
                                break;
                            case ECell.Yellow:
                                cell_char = '#';
                                break;
                            case ECell.Green:
                                cell_char = '$';
                                break;
                            case ECell.Purple:
                                cell_char = '%';
                                break;
                            case ECell.Red:
                                cell_char = '&';
                                break;
                            case ECell.AddLine:
                                cell_char = '\'';
                                break;
                            case ECell.ClearLine:
                                cell_char = '(';
                                break;
                            case ECell.NukeField:
                                cell_char = ')';
                                break;
                            case ECell.RandomBlocksClear:
                                cell_char = '*';
                                break;
                            case ECell.SwitchFields:
                                cell_char = '+';
                                break;
                            case ECell.ClearSpecialBlocks:
                                cell_char = ',';
                                break;
                            case ECell.Gravity:
                                cell_char = '-';
                                break;
                            case ECell.QuakeField:
                                cell_char = '.';
                                break;
                            case ECell.BlockBomb:
                                cell_char = '/';
                                break;
                        }
                        field_char_list.Add(cell_char);
                        field_char_list.Add(Convert.ToChar((i % Field.width) + 0x33));
                        field_char_list.Add(Convert.ToChar((i / Field.width) + 0x33));
                    }
                }
                if (field_char_list.Count > 0)
                {
                    ret = SendMessageAsync("f " + userID + " " + (new string(field_char_list.ToArray())));
                    field_char_list.Clear();
                }
            }
            return ret;
        }

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

        /// <summary>
        /// Try parse boolean
        /// </summary>
        /// <param name="str">String</param>
        /// <param name="value">Value</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        private static bool TryParseBool(string str, out bool value)
        {
            uint val;
            bool ret = uint.TryParse(str, out val);
            value = default;
            if (ret)
            {
                ret = (val <= 1);
                if (ret)
                {
                    value = (val == 1);
                }
            }
            return ret;
        }

        /// <summary>
        /// Parse player number message
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="rawContent">Raw content</param>
        /// <param name="protocol">Protocol</param>
        private void ParsePlayerNumberMessage(string message, string rawContent, EProtocol protocol)
        {
            if ((Protocol == EProtocol.Unspecified) && (rawContent.Length > 1))
            {
                int user_id;
                if (int.TryParse(rawContent, out user_id))
                {
                    Protocol = protocol;
                    SendLogInMessageAsync();
                    OnClientJoined?.Invoke(user_id, Username);
                }
                else
                {
                    throw new MalformedMessageException(message);
                }
            }
            else
            {
                throw new MalformedMessageException(message);
            }
        }

        /// <summary>
        /// Parse new game message
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="splitMessage">Split message</param>
        private void ParseNewGameMessage(string message, string[] splitMessage)
        {
            if ((Protocol == EProtocol.TetriNET) && (splitMessage.Length > 11))
            {
                uint starting_height;
                uint starting_level;
                uint lines_per_level;
                uint level_increment;
                uint lines_per_special;
                uint specials_added;
                uint special_capacity;
                bool display_average_levels;
                bool classic_mode;
                if (uint.TryParse(splitMessage[0], out starting_height) &&
                    uint.TryParse(splitMessage[1], out starting_level) &&
                    uint.TryParse(splitMessage[2], out lines_per_level) &&
                    uint.TryParse(splitMessage[3], out level_increment) &&
                    uint.TryParse(splitMessage[4], out lines_per_special) &&
                    uint.TryParse(splitMessage[5], out specials_added) &&
                    uint.TryParse(splitMessage[6], out special_capacity) &&
                    (splitMessage[7].Length == 100) &&
                    TryParseBool(splitMessage[9], out display_average_levels) &&
                    TryParseBool(splitMessage[10], out classic_mode))
                {
                    if ((starting_level > 0U) && (lines_per_level > 0U) && (lines_per_special > 0U) && (classic_mode ? true : (splitMessage[8].Length == 100)))
                    {
                        EBlock[] block_frequencies = new EBlock[100];
                        ESpecial[] special_frequencies = (classic_mode ? Array.Empty<ESpecial>() : (new ESpecial[100]));
                        for (int i = 0; i < block_frequencies.Length; i++)
                        {
                            uint block_index;
                            if (uint.TryParse(splitMessage[7][i].ToString(), out block_index))
                            {
                                if ((block_index > 0) && (block_index < 8))
                                {
                                    block_frequencies[i] = (EBlock)block_index;
                                }
                                else
                                {
                                    throw new MalformedMessageException(message);
                                }
                            }
                            else
                            {
                                throw new MalformedMessageException(message);
                            }
                        }
                        if (!classic_mode)
                        {
                            for (int i = 0; i < special_frequencies.Length; i++)
                            {
                                uint special_index;
                                if (uint.TryParse(splitMessage[8][i].ToString(), out special_index))
                                {
                                    if ((special_index > 0) && (special_index < 10))
                                    {
                                        special_frequencies[i] = (ESpecial)special_index;
                                    }
                                    else
                                    {
                                        throw new MalformedMessageException(message);
                                    }
                                }
                                else
                                {
                                    throw new MalformedMessageException(message);
                                }
                            }
                        }
                        OnNewGameStarted?.Invoke(starting_height, starting_level, lines_per_level, level_increment, lines_per_special, specials_added, special_capacity, block_frequencies, special_frequencies, display_average_levels, classic_mode);
                    }
                    else
                    {
                        throw new MalformedMessageException(message);
                    }
                }
                else
                {
                    throw new MalformedMessageException(message);
                }
            }
            else
            {
                throw new MalformedMessageException(message);
            }
        }

        /// <summary>
        /// Get cells from full update
        /// </summary>
        /// <param name="fieldString">Field string</param>
        /// <returns>Cells if successful, otherwise "null"</returns>
        private ECell[] GetCellsFromFullUpdate(string fieldString)
        {
            ECell[] ret = new ECell[Field.width * Field.height];
            for (int i = 0; i < ret.Length; i++)
            {
                switch (fieldString[i])
                {
                    case '0':
                        ret[i] = ECell.Nothing;
                        break;
                    case '1':
                        ret[i] = ECell.Blue;
                        break;
                    case '2':
                        ret[i] = ECell.Yellow;
                        break;
                    case '3':
                        ret[i] = ECell.Green;
                        break;
                    case '4':
                        ret[i] = ECell.Purple;
                        break;
                    case '5':
                        ret[i] = ECell.Red;
                        break;
                    case 'a':
                        ret[i] = ECell.AddLine;
                        break;
                    case 'c':
                        ret[i] = ECell.ClearLine;
                        break;
                    case 'n':
                        ret[i] = ECell.NukeField;
                        break;
                    case 'r':
                        ret[i] = ECell.RandomBlocksClear;
                        break;
                    case 's':
                        ret[i] = ECell.SwitchFields;
                        break;
                    case 'b':
                        ret[i] = ECell.ClearSpecialBlocks;
                        break;
                    case 'g':
                        ret[i] = ECell.Gravity;
                        break;
                    case 'q':
                        ret[i] = ECell.QuakeField;
                        break;
                    case 'o':
                        ret[i] = ECell.BlockBomb;
                        break;
                    // TODO
                    // More cell specials
                    default:
                        return null;
                }
            }
            return ret;
        }

        /// <summary>
        /// Get cell positions from partial update
        /// </summary>
        /// <param name="fieldString">Field string</param>
        /// <returns>Cells if successdful, otherwise "null"</returns>
        private CellPosition[] GetCellPositionsFromPartialUpdate(string fieldString)
        {
            List<CellPosition> cell_positions = null;
            if ((fieldString.Length % 3) == 0)
            {
                cell_positions = new List<CellPosition>();
                for (int i = 0; i < fieldString.Length; i += 3)
                {
                    ECell cell;
                    switch (fieldString[i])
                    {
                        case '!':
                            cell = ECell.Nothing;
                            break;
                        case '"':
                            cell = ECell.Blue;
                            break;
                        case '#':
                            cell = ECell.Yellow;
                            break;
                        case '$':
                            cell = ECell.Green;
                            break;
                        case '%':
                            cell = ECell.Purple;
                            break;
                        case '&':
                            cell = ECell.Red;
                            break;
                        case '\'':
                            cell = ECell.AddLine;
                            break;
                        case '(':
                            cell = ECell.ClearLine;
                            break;
                        case ')':
                            cell = ECell.NukeField;
                            break;
                        case '*':
                            cell = ECell.RandomBlocksClear;
                            break;
                        case '+':
                            cell = ECell.SwitchFields;
                            break;
                        case ',':
                            cell = ECell.ClearSpecialBlocks;
                            break;
                        case '-':
                            cell = ECell.Gravity;
                            break;
                        case '.':
                            cell = ECell.QuakeField;
                            break;
                        case '/':
                            cell = ECell.BlockBomb;
                            break;
                        default:
                            return null;
                    }
                    int x = Convert.ToByte(fieldString[i + 1]) - 0x33;
                    int y = Convert.ToByte(fieldString[i + 2]) - 0x33;
                    if ((x >= 0) && (x < Field.width) && (y >= 0) && (y < Field.height))
                    {
                        cell_positions.Add(new CellPosition(cell, (uint)x, (uint)y));
                    }
                    else
                    {
                        cell_positions.Clear();
                        cell_positions = null;
                        break;
                    }
                }
            }
            CellPosition[] ret = null;
            if (cell_positions != null)
            {
                if (cell_positions.Count > 0)
                {
                    ret = cell_positions.ToArray();
                }
            }
            return ret;
        }

        /// <summary>
        /// Parse message
        /// </summary>
        /// <param name="message">Message</param>
        private void ParseMessage(string message)
        {
            string[] split_message = message.Split(' ');
            if (split_message.Length > 0)
            {
                string raw_content = string.Empty;
                if (message.Length > split_message[0].Length)
                {
                    raw_content = message.Substring(split_message[0].Length + 1);
                }
                switch (split_message[0])
                {
                    // Player number (TetriNET)
                    case "playernum":
                        ParsePlayerNumberMessage(message, raw_content, EProtocol.TetriNET);
                        break;

                    // Player number (TetriFast)
                    case ")#)(!@(*3":
                        ParsePlayerNumberMessage(message, raw_content, EProtocol.TetriFast);
                        break;

                    // Player has joined
                    case "playerjoin":
                        if (split_message.Length > 2)
                        {
                            int user_id;
                            if (int.TryParse(split_message[1], out user_id))
                            {
                                OnUserJoined?.Invoke(user_id, split_message[2]);
                            }
                            else
                            {
                                throw new MalformedMessageException(message);
                            }
                        }
                        else
                        {
                            throw new MalformedMessageException(message);
                        }
                        break;

                    // Player has left
                    case "playerleave":
                        if (split_message.Length > 1)
                        {
                            int user_id;
                            if (int.TryParse(split_message[1], out user_id))
                            {
                                OnUserLeft?.Invoke(user_id);
                            }
                            else
                            {
                                throw new MalformedMessageException(message);
                            }
                        }
                        else
                        {
                            throw new MalformedMessageException(message);
                        }
                        break;

                    // Player team name has changed
                    case "team":
                        if (split_message.Length > 2)
                        {
                            int user_id;
                            string team_name = string.Empty;
                            if (int.TryParse(split_message[1], out user_id))
                            {
                                int head_length = (split_message[0].Length + split_message[1].Length + 2);
                                if (message.Length > head_length)
                                {
                                    team_name = message.Substring(head_length);
                                }
                                OnUserTeamNameChanged?.Invoke(user_id, team_name);
                            }
                            else
                            {
                                throw new MalformedMessageException(message);
                            }
                        }
                        else
                        {
                            throw new MalformedMessageException(message);
                        }
                        break;

                    // Winlist
                    case "winlist":
                        if (split_message.Length > 1)
                        {
                            List<WinlistEntry> winlist = new List<WinlistEntry>();
                            StringBuilder buffer = new StringBuilder();
                            for (int i = 1; i < split_message.Length; i++)
                            {
                                if (buffer.Length > 0)
                                {
                                    buffer.Append(" ");
                                }
                                buffer.Append(split_message[i]);
                                Match match = winlistEntryRegex.Match(buffer.ToString());
                                if (match != null)
                                {
                                    if (match.Success)
                                    {
                                        if (match.Groups.Count >= 4)
                                        {
                                            string name = match.Groups[2].Value;
                                            long score;
                                            if (long.TryParse(match.Groups[3].Value, out score))
                                            {
                                                winlist.Add(new WinlistEntry(match.Groups[1].Value == "t", name, score));
                                            }
                                        }
                                        buffer.Clear();
                                    }
                                }
                            }
                            if (buffer.Length > 0)
                            {
                                throw new MalformedMessageException(message);
                            }
                            OnWinlistReceived?.Invoke(winlist.ToArray());
                            winlist.Clear();
                        }
                        else
                        {
                            throw new MalformedMessageException(message);
                        }
                        break;

                    // Partyline chat message
                    case "pline":
                        if (split_message.Length > 2)
                        {
                            int user_id;
                            if (int.TryParse(split_message[1], out user_id))
                            {
                                string chat_message = string.Empty;
                                int head_length = (split_message[0].Length + split_message[1].Length + 2);
                                if (message.Length > head_length)
                                {
                                    chat_message = message.Substring(head_length);
                                }
                                if (user_id == 0U)
                                {
                                    OnServerChatMessageReceived?.Invoke(chat_message);
                                }
                                else
                                {
                                    OnUserChatMessageReceived?.Invoke(user_id, chat_message);
                                }
                            }
                            else
                            {
                                throw new MalformedMessageException(message);
                            }
                        }
                        else
                        {
                            throw new MalformedMessageException(message);
                        }
                        break;

                    // Partyline chat action
                    case "plineact":
                        if (split_message.Length > 2)
                        {
                            int user_id;
                            if (int.TryParse(split_message[1], out user_id))
                            {
                                string chat_message = string.Empty;
                                int head_length = (split_message[0].Length + split_message[1].Length + 2);
                                if (message.Length > head_length)
                                {
                                    chat_message = message.Substring(head_length);
                                }
                                if (user_id == 0U)
                                {
                                    OnServerChatActionReceived?.Invoke(chat_message);
                                }
                                else
                                {
                                    OnUserChatActionReceived?.Invoke(user_id, chat_message);
                                }
                            }
                            else
                            {
                                throw new MalformedMessageException(message);
                            }
                        }
                        else
                        {
                            throw new MalformedMessageException(message);
                        }
                        break;

                    // Game chat message
                    case "gmsg":
                        if (split_message.Length > 1)
                        {
                            string game_chat_message = string.Empty;
                            int head_length = (split_message[0].Length + 1);
                            if (message.Length > head_length)
                            {
                                game_chat_message = message.Substring(head_length);
                            }
                            OnGameChatReceived?.Invoke(game_chat_message);
                        }
                        else
                        {
                            throw new MalformedMessageException(message);
                        }
                        break;

                    // New game (TetriNET)
                    case "newgame":
                        ParseNewGameMessage(message, split_message);
                        break;

                    // New game (TetriFast)
                    case "*******":
                        ParseNewGameMessage(message, split_message);
                        break;

                    // In-game
                    case "ingame":
                        OnGameIsAlreadyInProgress?.Invoke();
                        break;

                    // Level update or client info request
                    case "lvl":
                        if (split_message.Length > 2)
                        {
                            int user_id;
                            uint level;
                            if (int.TryParse(split_message[1], out user_id) &&
                                uint.TryParse(split_message[2], out level))
                            {
                                if ((user_id == 0U) && (level == 0U))
                                {
                                    SendClientInformationMessageAsync();
                                    OnRequestClientInformation?.Invoke();
                                }
                                else
                                {
                                    OnUserLevelUpdate?.Invoke(user_id, level);
                                }
                            }
                            else
                            {
                                throw new MalformedMessageException(message);
                            }
                        }
                        else
                        {
                            throw new MalformedMessageException(message);
                        }
                        break;

                    // Special used
                    case "sb":
                        if (split_message.Length > 3)
                        {
                            int target_user_id;
                            int sender_user_id;
                            if (int.TryParse(split_message[1], out target_user_id) &&
                                int.TryParse(split_message[3], out sender_user_id))
                            {
                                ESpecial special = ESpecial.Nothing;
                                switch (split_message[2])
                                {
                                    case "a":
                                        special = ESpecial.AddLine;
                                        break;
                                    case "c":
                                        special = ESpecial.ClearLine;
                                        break;
                                    case "n":
                                        special = ESpecial.NukeField;
                                        break;
                                    case "r":
                                        special = ESpecial.RandomBlocksClear;
                                        break;
                                    case "s":
                                        special = ESpecial.SwitchFields;
                                        break;
                                    case "b":
                                        special = ESpecial.ClearSpecialBlocks;
                                        break;
                                    case "g":
                                        special = ESpecial.Gravity;
                                        break;
                                    case "q":
                                        special = ESpecial.QuakeField;
                                        break;
                                    case "o":
                                        special = ESpecial.BlockBomb;
                                        break;
                                    case "l":
                                        special = ESpecial.LeftGravity;
                                        break;
                                    case "p":
                                        special = ESpecial.PieceChange;
                                        break;
                                    case "z":
                                        special = ESpecial.ZebraField;
                                        break;
                                        // TODO
                                        // TetriNET2 special types
                                }
                                if (special == ESpecial.Nothing)
                                {
                                    if (split_message[2].StartsWith("cs") && (split_message[2].Length > 2))
                                    {
                                        uint num_lines;
                                        if (uint.TryParse(split_message[2].Substring(2), out num_lines) && (target_user_id == 0U))
                                        {
                                            OnClassicModeAddLines?.Invoke(sender_user_id, num_lines);
                                        }
                                        else
                                        {
                                            throw new MalformedMessageException(message);
                                        }
                                    }
                                    else
                                    {
                                        throw new MalformedMessageException(message);
                                    }
                                }
                                else
                                {
                                    if (target_user_id == 0U)
                                    {
                                        if (sender_user_id == 0U)
                                        {
                                            OnSpecialUsedForAll?.Invoke(special);
                                        }
                                        else
                                        {
                                            OnUserSpecialUsedForAll?.Invoke(sender_user_id, special);
                                        }
                                    }
                                    else
                                    {
                                        if (sender_user_id == 0U)
                                        {
                                            OnServerSpecialUsed?.Invoke(target_user_id, special);
                                        }
                                        else
                                        {
                                            OnUserSpecialUsed?.Invoke(sender_user_id, target_user_id, special);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                throw new MalformedMessageException(message);
                            }
                        }
                        break;

                    // Player has lost
                    case "playerlost":
                        if (split_message.Length > 1)
                        {
                            int user_id;
                            if (int.TryParse(raw_content, out user_id))
                            {
                                OnUserLost?.Invoke(user_id);
                            }
                            else
                            {
                                throw new MalformedMessageException(message);
                            }
                        }
                        else
                        {
                            throw new MalformedMessageException(message);
                        }
                        break;

                    // Player has won
                    case "playerwon":
                        if (split_message.Length > 1)
                        {
                            int user_id;
                            if (int.TryParse(raw_content, out user_id))
                            {
                                OnUserWon?.Invoke(user_id);
                            }
                            else
                            {
                                throw new MalformedMessageException(message);
                            }
                        }
                        else
                        {
                            throw new MalformedMessageException(message);
                        }
                        break;

                    // Pause/Resume game
                    case "pause":
                        if (split_message.Length > 1)
                        {
                            bool pause;
                            if (TryParseBool(raw_content, out pause))
                            {
                                if (pause)
                                {
                                    OnPauseGame?.Invoke();
                                }
                                else
                                {
                                    OnResumeGame?.Invoke();
                                }
                            }
                            else
                            {
                                throw new MalformedMessageException(message);
                            }
                        }
                        else
                        {
                            throw new MalformedMessageException(message);
                        }
                        break;

                    // End game
                    case "endgame":
                        OnEndGame?.Invoke();
                        break;

                    // Connection denied
                    case "noconnecting":
                        OnConnectionDenied?.Invoke();
                        break;

                    // Field update
                    case "f":
                        // TODO
                        // Implement partial update
                        if (split_message.Length > 2)
                        {
                            int user_id;
                            string field_string = split_message[2];
                            if (int.TryParse(split_message[1], out user_id) &&
                                field_string.Length > 2)
                            {
                                switch (field_string[0])
                                {
                                    case '0':
                                    case '1':
                                    case '2':
                                    case '3':
                                    case '4':
                                    case '5':
                                    case 'a':
                                    case 'c':
                                    case 'n':
                                    case 'r':
                                    case 's':
                                    case 'b':
                                    case 'g':
                                    case 'q':
                                    case 'o':
                                        ECell[] cells = GetCellsFromFullUpdate(field_string);
                                        if (cells == null)
                                        {
                                            throw new MalformedMessageException(message);
                                        }
                                        else
                                        {
                                            OnUserFullFieldUpdate?.Invoke(user_id, cells);
                                        }
                                        break;
                                    // TODO
                                    // More special cells
                                    case '!':
                                    case '"':
                                    case '#':
                                    case '$':
                                    case '%':
                                    case '&':
                                    case '\'':
                                    case '(':
                                    case ')':
                                    case '*':
                                    case '+':
                                    case ',':
                                    case '-':
                                    case '.':
                                    case '/':
                                        CellPosition[] cell_positions = GetCellPositionsFromPartialUpdate(field_string);
                                        if (cell_positions == null)
                                        {
                                            throw new MalformedMessageException(message);
                                        }
                                        else
                                        {
                                            OnUserPartialFieldUpdate?.Invoke(user_id, cell_positions);
                                        }
                                        break;
                                    default:
                                        throw new MalformedMessageException(message);
                                        // TODO
                                        // More special cells
                                }
                            }
                        }
                        break;

                    // Heartbeat
                    case "":
                        IsHeartbeatSupported = true;
                        OnHeartBeat?.Invoke();
                        break;

                    // Unknown message
                    default:
                        throw new MalformedMessageException(message);
                }
            }
        }
    }
}
