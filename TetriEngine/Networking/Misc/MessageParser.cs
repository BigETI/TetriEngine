using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TetriEngine.Networking.Connection;
using TetriEngine.Networking.Connection.Client;
using TetriEngine.Networking.Connection.Server;

/// <summary>
/// TetriEngine  networking namespace
/// </summary>
namespace TetriEngine.Networking
{
    /// <summary>
    /// Message parser class
    /// </summary>
    internal class MessageParser
    {
        /// <summary>
        /// Protocol
        /// </summary>
        public EProtocol Protocol { get; private set; }

        /// <summary>
        /// Winlist entry regex
        /// </summary>
        private static readonly Regex winlistEntryRegex = new Regex(@"([t,p])(.+?);([0-9]+)");

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
        public event HeartBeatReceivedDelegate OnHeartBeat;

        /// <summary>
        /// On user full field update event
        /// </summary>
        public event UserFullFieldUpdateDelegate OnUserFullFieldUpdate;

        /// <summary>
        /// On user partial field update event
        /// </summary>
        public event UserPartialFieldUpdateDelegate OnUserPartialFieldUpdate;

        /// <summary>
        /// On client log in
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
        /// <param name="protocol">Protocol</param>
        internal MessageParser(EProtocol protocol)
        {
            Protocol = protocol;
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
            if (OnClientJoined == null)
            {
                throw new ProtocolMessageNotRegisteredException(message, rawContent);
            }
            else if ((Protocol == EProtocol.Unspecified) && (rawContent.Length > 1))
            {
                int user_id;
                if (int.TryParse(rawContent, out user_id))
                {
                    OnClientJoined(user_id, protocol);
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
        /// <param name="rawContent">Raw content</param>
        private void ParseNewGameMessage(string message, string[] splitMessage, string rawContent)
        {
            if (OnNewGameStarted == null)
            {
                throw new ProtocolMessageNotRegisteredException(message, rawContent);
            }
            else if (splitMessage.Length > 11)
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
                        OnNewGameStarted(starting_height, starting_level, lines_per_level, level_increment, lines_per_special, specials_added, special_capacity, block_frequencies, special_frequencies, display_average_levels, classic_mode);
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
        /// Decode message
        /// </summary>
        /// <param name="encodedMessage">Encoded message</param>
        /// <returns>Decoded message</returns>
        private string DecodeMessage(string encodedMessage)
        {
            byte[] bytes = new byte[encodedMessage.Length / 2];
            char[] char_pair = new char[2];
            for (int i = 0, j; i < bytes.Length; i++)
            {
                for (j = 0; j < char_pair.Length; j++)
                {
                    char_pair[j] = encodedMessage[(i * 2) + j];
                }
                bytes[i] = byte.Parse(new string(char_pair), System.Globalization.NumberStyles.HexNumber);
            }
            string command = ((Protocol == EProtocol.TetriNET) ? "tetrisstart" : "tetrifaster");
            int[] h = new int[command.Length];
            for (int i = 0; i < command.Length; i++)
            {
                h[i] = ((command[i] + bytes[i]) % 255) ^ bytes[i + 1];
            }
            int hLength = 5;
            for (int i = 5; (i == hLength) && (i < 0); i--)
            {
                for (int j = 0; j < command.Length - hLength; j++)
                {
                    if (h[j] != h[j + hLength])
                    {
                        hLength--;
                    }
                }
            }
            char[] decodedString = new char[bytes.Length - 1];
            for (int i = 1; i < bytes.Length; i++)
            {
                decodedString[i - 1] = (char)(((bytes[i] ^ h[(i - 1) % hLength]) + 255 - bytes[i - 1]) % 255);
            }
            return new string(decodedString);
        }

        /// <summary>
        /// Parse message
        /// </summary>
        /// <param name="message">Message</param>
        public void ParseMessage(string message)
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
                        if (OnUserJoined == null)
                        {
                            throw new ProtocolMessageNotRegisteredException(message, raw_content);
                        }
                        else if (split_message.Length > 2)
                        {
                            int user_id;
                            if (int.TryParse(split_message[1], out user_id))
                            {
                                OnUserJoined(user_id, split_message[2]);
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
                        if (OnUserLeft == null)
                        {
                            throw new ProtocolMessageNotRegisteredException(message, raw_content);
                        }
                        else if (split_message.Length > 1)
                        {
                            int user_id;
                            if (int.TryParse(split_message[1], out user_id))
                            {
                                OnUserLeft(user_id);
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
                        if (OnUserTeamNameChanged == null)
                        {
                            throw new ProtocolMessageNotRegisteredException(message, raw_content);
                        }
                        else if (split_message.Length > 2)
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
                                OnUserTeamNameChanged(user_id, team_name);
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
                        if (OnWinlistReceived == null)
                        {
                            throw new ProtocolMessageNotRegisteredException(message, raw_content);
                        }
                        else if (split_message.Length > 1)
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
                            OnWinlistReceived(winlist.ToArray());
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
                                    if (OnServerChatMessageReceived == null)
                                    {
                                        throw new ProtocolMessageFormatNotRegisteredException(message, raw_content);
                                    }
                                    else
                                    {
                                        OnServerChatMessageReceived(chat_message);
                                    }
                                }
                                else
                                {
                                    if (OnUserChatMessageReceived == null)
                                    {
                                        throw new ProtocolMessageFormatNotRegisteredException(message, raw_content);
                                    }
                                    else
                                    {
                                        OnUserChatMessageReceived(user_id, chat_message);
                                    }
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
                                    if (OnServerChatActionReceived == null)
                                    {
                                        throw new ProtocolMessageFormatNotRegisteredException(message, raw_content);
                                    }
                                    else
                                    {
                                        OnServerChatActionReceived(chat_message);
                                    }
                                }
                                else
                                {
                                    if (OnUserChatActionReceived == null)
                                    {
                                        throw new ProtocolMessageFormatNotRegisteredException(message, raw_content);
                                    }
                                    else
                                    {
                                        OnUserChatActionReceived(user_id, chat_message);
                                    }
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
                        if (OnGameChatMessageReceived == null)
                        {
                            throw new ProtocolMessageNotRegisteredException(message, raw_content);
                        }
                        else if (split_message.Length > 1)
                        {
                            string game_chat_message = string.Empty;
                            int head_length = (split_message[0].Length + 1);
                            if (message.Length > head_length)
                            {
                                game_chat_message = message.Substring(head_length);
                            }
                            OnGameChatMessageReceived(game_chat_message);
                        }
                        else
                        {
                            throw new MalformedMessageException(message);
                        }
                        break;

                    // New game (TetriNET)
                    case "newgame":
                        ParseNewGameMessage(message, split_message, raw_content);
                        break;

                    // New game (TetriFast)
                    case "*******":
                        ParseNewGameMessage(message, split_message, raw_content);
                        break;

                    // In-game
                    case "ingame":
                        if (OnGameIsAlreadyInProgress == null)
                        {
                            throw new ProtocolMessageNotRegisteredException(message, raw_content);
                        }
                        else
                        {
                            OnGameIsAlreadyInProgress();
                        }
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
                                    if (OnRequestClientInformation == null)
                                    {
                                        throw new ProtocolMessageFormatNotRegisteredException(message, raw_content);
                                    }
                                    else
                                    {
                                        OnRequestClientInformation();
                                    }
                                }
                                else
                                {
                                    if (OnUserLevelUpdate == null)
                                    {
                                        throw new ProtocolMessageFormatNotRegisteredException(message, raw_content);
                                    }
                                    else
                                    {
                                        OnUserLevelUpdate(user_id, level);
                                    }
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
                                            if (OnClassicModeAddLines == null)
                                            {
                                                throw new ProtocolMessageFormatNotRegisteredException(message, raw_content);
                                            }
                                            else
                                            {
                                                OnClassicModeAddLines(sender_user_id, num_lines);
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
                                else
                                {
                                    if (target_user_id == 0U)
                                    {
                                        if (sender_user_id == 0U)
                                        {
                                            if (OnServerSpecialUsedForAll == null)
                                            {
                                                throw new ProtocolMessageFormatNotRegisteredException(message, raw_content);
                                            }
                                            else
                                            {
                                                OnServerSpecialUsedForAll(special);
                                            }
                                        }
                                        else
                                        {
                                            if (OnUserSpecialUsedForAll == null)
                                            {
                                                throw new ProtocolMessageFormatNotRegisteredException(message, raw_content);
                                            }
                                            else
                                            {
                                                OnUserSpecialUsedForAll(sender_user_id, special);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (sender_user_id == 0U)
                                        {
                                            if (OnServerSpecialUsed == null)
                                            {
                                                throw new ProtocolMessageFormatNotRegisteredException(message, raw_content);
                                            }
                                            else
                                            {
                                                OnServerSpecialUsed(target_user_id, special);
                                            }
                                        }
                                        else
                                        {
                                            if (OnUserSpecialUsed == null)
                                            {
                                                throw new ProtocolMessageFormatNotRegisteredException(message, raw_content);
                                            }
                                            else
                                            {
                                                OnUserSpecialUsed(sender_user_id, target_user_id, special);
                                            }
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
                        if (OnUserLost == null)
                        {
                            throw new ProtocolMessageNotRegisteredException(message, raw_content);
                        }
                        else if (split_message.Length > 1)
                        {
                            int user_id;
                            if (int.TryParse(raw_content, out user_id))
                            {
                                OnUserLost(user_id);
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
                        if (OnUserWon == null)
                        {
                            throw new ProtocolMessageNotRegisteredException(message, raw_content);
                        }
                        else if (split_message.Length > 1)
                        {
                            int user_id;
                            if (int.TryParse(raw_content, out user_id))
                            {
                                OnUserWon(user_id);
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
                                    if (OnPauseGame == null)
                                    {
                                        throw new ProtocolMessageFormatNotRegisteredException(message, raw_content);
                                    }
                                    else
                                    {
                                        OnPauseGame();
                                    }
                                }
                                else
                                {
                                    if (OnResumeGame == null)
                                    {
                                        throw new ProtocolMessageFormatNotRegisteredException(message, raw_content);
                                    }
                                    else
                                    {
                                        OnResumeGame();
                                    }
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
                        if (OnEndGame == null)
                        {
                            throw new ProtocolMessageNotRegisteredException(message, raw_content);
                        }
                        else
                        {
                            OnEndGame();
                        }
                        break;

                    // Connection denied
                    case "noconnecting":
                        if (OnConnectionDenied == null)
                        {
                            throw new ProtocolMessageNotRegisteredException(message, raw_content);
                        }
                        else
                        {
                            OnConnectionDenied();
                        }
                        break;

                    // Field update
                    case "f":
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
                                        else if (OnUserFullFieldUpdate == null)
                                        {
                                            throw new ProtocolMessageFormatNotRegisteredException(message, raw_content);
                                        }
                                        else
                                        {
                                            OnUserFullFieldUpdate(user_id, cells);
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
                                        else if (OnUserPartialFieldUpdate == null)
                                        {
                                            throw new ProtocolMessageFormatNotRegisteredException(message, raw_content);
                                        }
                                        else
                                        {
                                            OnUserPartialFieldUpdate(user_id, cell_positions);
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
                        if (OnHeartBeat == null)
                        {
                            throw new ProtocolMessageNotRegisteredException(message, raw_content);
                        }
                        else
                        {
                            OnHeartBeat();
                        }
                        break;

                    // Client info
                    case "clientinfo":
                        if (OnClientInformationReceived == null)
                        {
                            throw new ProtocolMessageNotRegisteredException(message, raw_content);
                        }
                        else if (split_message.Length > 2)
                        {
                            OnClientInformationReceived(split_message[1], split_message[2]);
                        }
                        else
                        {
                            throw new MalformedMessageException(message);
                        }
                        break;

                    // Game start
                    case "gamestart":
                        if (split_message.Length > 2)
                        {
                            bool start;
                            int user_id;
                            if (TryParseBool(split_message[1], out start) &&
                                int.TryParse(split_message[2], out user_id))
                            {
                                if (start)
                                {
                                    if (OnStartGame == null)
                                    {
                                        throw new ProtocolMessageFormatNotRegisteredException(message, raw_content);
                                    }
                                    else
                                    {
                                        OnStartGame(user_id);
                                    }
                                }
                                else
                                {
                                    if (OnStopGame == null)
                                    {
                                        throw new ProtocolMessageFormatNotRegisteredException(message, raw_content);
                                    }
                                    else
                                    {
                                        OnStopGame(user_id);
                                    }
                                }
                            }
                        }
                        else
                        {
                            throw new MalformedMessageException(message);
                        }
                        break;

                    // Encoded or unknown message
                    default:
                        if (Protocol != EProtocol.Unspecified)
                        {
                            try
                            {
                                string decoded_message = DecodeMessage(message);
                                string[] decoded_split_message = decoded_message.Split(' ');
                                if (decoded_split_message.Length > 3)
                                {
                                    string decoded_raw_content = string.Empty;
                                    if (decoded_message.Length > decoded_split_message[0].Length)
                                    {
                                        decoded_raw_content = decoded_message.Substring(decoded_split_message[0].Length + 1);
                                    }
                                    bool is_valid = false;
                                    switch (decoded_split_message[0])
                                    {
                                        case "tetrisstart":
                                            is_valid = (Protocol == EProtocol.TetriNET);
                                            break;
                                        case "tetrifaster":
                                            is_valid = (Protocol == EProtocol.TetriFast);
                                            break;
                                    }
                                    if (is_valid)
                                    {
                                        if (OnClientLogIn == null)
                                        {
                                            throw new ProtocolMessageFormatNotRegisteredException(decoded_message, decoded_raw_content);
                                        }
                                        else
                                        {
                                            OnClientLogIn(decoded_split_message[1], decoded_split_message[2], Protocol);
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
                            catch (Exception e)
                            {
                                Console.Error.WriteLine(e);
                                throw new MalformedMessageException(message);
                            }
                        }
                        else
                        {
                            throw new MalformedMessageException(message);
                        }
                        break;
                }
            }
        }
    }
}
