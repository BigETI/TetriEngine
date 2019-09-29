using System;
using System.Text;

/// <summary>
/// TetriEngine networking namespace
/// </summary>
namespace TetriEngine.Networking
{
    /// <summary>
    /// Message builder class
    /// </summary>
    internal class MessageBuilder
    {
        /// <summary>
        /// Protocol
        /// </summary>
        public EProtocol Protocol { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="protocol">Protocol</param>
        internal MessageBuilder(EProtocol protocol)
        {
            Protocol = protocol;
        }

        /// <summary>
        /// Get boolean string
        /// </summary>
        /// <param name="state">State</param>
        /// <returns>Boolean string</returns>
        private static string GetBoolString(bool state) => (state ? "1" : "0");

        /// <summary>
        /// Get special character
        /// </summary>
        /// <param name="special">Special</param>
        /// <returns>Special character is successful, otherwise "0"</returns>
        private static char GetSpecialChar(ESpecial special)
        {
            char ret = '0';
            switch (special)
            {
                case ESpecial.AddLine:
                    ret = 'a';
                    break;
                case ESpecial.ClearLine:
                    ret = 'c';
                    break;
                case ESpecial.ClearSpecialBlocks:
                    ret = 'n';
                    break;
                case ESpecial.RandomBlocksClear:
                    ret = 'r';
                    break;
                case ESpecial.BlockBomb:
                    ret = 's';
                    break;
                case ESpecial.QuakeField:
                    ret = 'b';
                    break;
                case ESpecial.Gravity:
                    ret = 'g';
                    break;
                case ESpecial.SwitchFields:
                    ret = 'q';
                    break;
                case ESpecial.NukeField:
                    ret = 'o';
                    break;
                    //case ESpecial.Immunity:
                    //    ret = '';
                    //    break;
                    //case ESpecial.ClearColumn:
                    //    ret = '';
                    //    break;
                    //case ESpecial.MutatePieces:
                    //    ret = '';
                    //    break;
                    //case ESpecial.Darkness:
                    //    ret = '';
                    //    break;
                    //case ESpecial.Confusion:
                    //    ret = '';
                    //    break;
                    //case ESpecial.LeftGravity:
                    //    ret = '';
                    //    break;
                    //case ESpecial.PieceChange:
                    //    ret = '';
                    //    break;
                    //case ESpecial.ZebraField:
                    //    ret = '';
                    //    break;
                    // TODO
                    // Specials
            }
            return ret;
        }

        /// <summary>
        /// Build log in message
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>Built message</returns>
        internal string BuildLogInMessage(string username) => (((Protocol == EProtocol.TetriFast) ? "tetrifaster " : "tetrisstart ") + username + " 1.13");

        /// <summary>
        /// Build client information message
        /// </summary>
        /// <returns>Built message</returns>
        internal string BuildClientInformationMessage(string clientName, string clientVersion) => ("clientinfo " + clientName + " " + clientVersion);

        /// <summary>
        /// Build player number message
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Built message</returns>
        internal string BuildPlayerNumberMessage(IUser user) => (((Protocol == EProtocol.TetriFast) ? ")#)(!@(*3 " : "playernum ") + user.ID);

        /// <summary>
        /// Build user joined message
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Built message</returns>
        internal string BuildUserJoinedMessage(IUser user) => ("playerjoin " + user.ID + " " + user.Name);

        /// <summary>
        /// Build user left message
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Built message</returns>
        internal string BuildUserLeftMessage(IUser user) => ("playerleave " + user.ID);

        /// <summary>
        /// Build user changed team message
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Built message</returns>
        internal string BuildUserChangedTeamMessage(IUser user) => ("playerteam " + user.ID + " " + user.TeamName);

        /// <summary>
        /// Build winlist
        /// </summary>
        /// <param name="winlist"></param>
        /// <returns>Built message</returns>
        internal string BuildWinlistMessage(Winlist winlist)
        {
            StringBuilder message = new StringBuilder("winlist");
            foreach (IUser user in winlist.Users)
            {
                message.Append(" p");
                message.Append(user.Name);
                message.Append(";");
                message.Append(user.Score.ToString());
            }
            foreach (Team team in winlist.Teams)
            {
                message.Append(" t");
                message.Append(team.Name);
                message.Append(";");
                message.Append(team.Score.ToString());
            }
            return message.ToString();
        }

        /// <summary>
        /// Build chat message message
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="message">Message</param>
        /// <returns>Built message</returns>
        internal string BuildChatMessageMessage(IUser user, string message) => ("pline " + user.ID + " " + message);

        /// <summary>
        /// Build chat action message
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="action">Action</param>
        /// <returns>Built message</returns>
        internal string BuildChatActionMessage(IUser user, string action) => ("plineact " + user.ID + " " + action);

        /// <summary>
        /// Build game chat message
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>Built message</returns>
        internal string BuildGameChatMessageMessage(string message) => ("gmsg " + message);

        /// <summary>
        /// Build game chat message
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="message">Message</param>
        /// <returns>Built message</returns>
        internal string BuildGameChatMessageMessage(IUser user, string message) => BuildGameChatMessageMessage("<" + user.Name + "> " + message);

        /// <summary>
        /// Build start game message
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Built message</returns>
        internal string BuildStartGameMessage(IUser user) => ("startgame 1 " + user.ID);

        /// <summary>
        /// Build stop game message
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Built message</returns>
        internal string BuildStopGameMessage(IUser user) => ("startgame 0 " + user.ID);

        /// <summary>
        /// Build new game message
        /// </summary>
        /// <param name="gameOptions">Game options</param>
        /// <returns>Built message</returns>
        internal string BuildNewGameMessage(GameOptions gameOptions)
        {
            StringBuilder message = new StringBuilder((Protocol == EProtocol.TetriFast) ? "******* " : "newgame ");
            message.Append(gameOptions.StartingHeight.ToString());
            message.Append(" ");
            message.Append(gameOptions.StartingLevel.ToString());
            message.Append(" ");
            message.Append(gameOptions.LinesPerLevel.ToString());
            message.Append(" ");
            message.Append(gameOptions.LevelIncrement.ToString());
            message.Append(" ");
            message.Append(gameOptions.LinesPerSpecial.ToString());
            message.Append(" ");
            message.Append(gameOptions.SpecialsAdded.ToString());
            message.Append(" ");
            message.Append(gameOptions.StartingLevel.ToString());
            message.Append(" ");
            message.Append(gameOptions.SpecialCapacity.ToString());
            message.Append(" ");
            foreach (EBlock block in gameOptions.BlockFrequencies)
            {
                message.Append(((int)block).ToString());
            }
            message.Append(" ");
            if (gameOptions.SpecialFrequencies.Count > 0)
            {
                foreach (ESpecial special in gameOptions.SpecialFrequencies)
                {
                    message.Append(((int)special).ToString());
                }
            }
            else
            {
                message.Append("0");
            }
            message.Append(" ");
            message.Append(GetBoolString(gameOptions.DisplayAverageLevels));
            message.Append(" ");
            message.Append(GetBoolString(gameOptions.ClassicMode));
            return message.ToString();
        }

        /// <summary>
        /// Build game already in progress message
        /// </summary>
        /// <returns>Built message</returns>
        internal string BuildGameAlreadyInProgressMessage() => "ingame";

        /// <summary>
        /// Build field update message
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="oldFieldCells">Old field cells</param>
        /// <param name="newFieldCells">New field cells</param>
        /// <returns>Built message</returns>
        internal string BuildFieldUpdateMessage(IUser user, ECell[] oldFieldCells, ECell[] newFieldCells)
        {
            StringBuilder message = new StringBuilder("f ");
            message.Append(user.ID.ToString());
            message.Append(" ");
            int length = Math.Min(oldFieldCells.Length, newFieldCells.Length);
            uint count = 0U;
            bool do_full_update = false;
            for (int i = 0; i < length; i++)
            {
                if (oldFieldCells[i] != newFieldCells[i])
                {
                    ++count;
                    if ((count * 2U) >= length)
                    {
                        do_full_update = true;
                        break;
                    }
                }
            }
            if (do_full_update)
            {
                foreach (ECell field_cell in newFieldCells)
                {
                    int cell_index = (int)field_cell;
                    if (cell_index < ((int)ECell.AddLine))
                    {
                        message.Append(cell_index.ToString());
                    }
                    else
                    {
                        switch (field_cell)
                        {
                            case ECell.AddLine:
                                message.Append("a");
                                break;
                            case ECell.ClearLine:
                                message.Append("c");
                                break;
                            case ECell.ClearSpecialBlocks:
                                message.Append("n");
                                break;
                            case ECell.RandomBlocksClear:
                                message.Append("r");
                                break;
                            case ECell.BlockBomb:
                                message.Append("s");
                                break;
                            case ECell.QuakeField:
                                message.Append("b");
                                break;
                            case ECell.Gravity:
                                message.Append("g");
                                break;
                            case ECell.SwitchFields:
                                message.Append("q");
                                break;
                            case ECell.NukeField:
                                message.Append("o");
                                break;
                            //case ECell.Immunity:
                            //    message.Append("");
                            //    break;
                            //case ECell.ClearColumn:
                            //    message.Append("");
                            //    break;
                            //case ECell.MutatePieces:
                            //    message.Append("");
                            //    break;
                            //case ECell.Darkness:
                            //    message.Append("");
                            //    break;
                            //case ECell.Confusion:
                            //    message.Append("");
                            //    break;
                            //case ECell.LeftGravity:
                            //    message.Append("");
                            //    break;
                            //case ECell.PieceChange:
                            //    message.Append("");
                            //    break;
                            //case ECell.ZebraField:
                            //    message.Append("");
                            //    break;
                            default:
                                message.Append("0");
                                break;
                                // TODO
                                // Special cells
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < length; i++)
                {
                    if (oldFieldCells[i] != newFieldCells[i])
                    {
                        switch (newFieldCells[i])
                        {
                            case ECell.Blue:
                                message.Append("\"");
                                break;
                            case ECell.Yellow:
                                message.Append("#");
                                break;
                            case ECell.Green:
                                message.Append("$");
                                break;
                            case ECell.Purple:
                                message.Append("%");
                                break;
                            case ECell.Red:
                                message.Append("&");
                                break;
                            case ECell.AddLine:
                                message.Append("'");
                                break;
                            case ECell.ClearLine:
                                message.Append("(");
                                break;
                            case ECell.ClearSpecialBlocks:
                                message.Append(",");
                                break;
                            case ECell.RandomBlocksClear:
                                message.Append("*");
                                break;
                            case ECell.BlockBomb:
                                message.Append("/");
                                break;
                            case ECell.QuakeField:
                                message.Append(".");
                                break;
                            case ECell.Gravity:
                                message.Append("-");
                                break;
                            case ECell.SwitchFields:
                                message.Append("+");
                                break;
                            case ECell.NukeField:
                                message.Append(")");
                                break;
                            //case ECell.Immunity:
                            //    message.Append("");
                            //    break;
                            //case ECell.ClearColumn:
                            //    message.Append("");
                            //    break;
                            //case ECell.MutatePieces:
                            //    message.Append("");
                            //    break;
                            //case ECell.Darkness:
                            //    message.Append("");
                            //    break;
                            //case ECell.Confusion:
                            //    message.Append("");
                            //    break;
                            //case ECell.LeftGravity:
                            //    message.Append("");
                            //    break;
                            //case ECell.PieceChange:
                            //    message.Append("");
                            //    break;
                            //case ECell.ZebraField:
                            //    message.Append("");
                            //    break;
                            default:
                                message.Append("!");
                                break;
                                // TODO
                                // Special cells
                        }
                        message.Append(Convert.ToChar(0x33 + (i % Field.width)).ToString());
                        message.Append(Convert.ToChar(0x33 + (i / Field.width)).ToString());
                    }
                }
            }
            return message.ToString();
        }

        /// <summary>
        /// Build user level update message
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Built message</returns>
        internal string BuildLevelUpdateMessage(IUser user) => ("lvl " + user.ID + " " + user.Level);

        /// <summary>
        /// Build special used message
        /// </summary>
        /// <param name="senderUser">Sender user</param>
        /// <param name="targetUser">Target user</param>
        /// <param name="special">Special</param>
        /// <returns>Built message</returns>
        internal string BuildSpecialUsedMessage(IUser senderUser, IUser targetUser, ESpecial special) => ("sb " + targetUser.ID + " " + GetSpecialChar(special) + " " + senderUser.ID);

        /// <summary>
        /// Build classic style add lines message
        /// </summary>
        /// <param name="senderUser">Sender user</param>
        /// <param name="numLines">Number of lines</param>
        /// <returns>Built message</returns>
        internal string BuildClassicStyleAddLinesMessage(IUser senderUser, uint numLines) => ("sb 0 cs" + numLines + " " + senderUser.ID);

        /// <summary>
        /// Build user lost message
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Built message</returns>
        internal string BuildUserLostMessage(IUser user) => ("playerlost " + user.ID);

        /// <summary>
        /// Build user won message
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Built message</returns>
        internal string BuildUserWonMessage(IUser user) => ("playerwon " + user.ID);

        /// <summary>
        /// Build pause message
        /// </summary>
        /// <returns>Built message</returns>
        internal string BuildPauseMessage() => "pause 1";

        /// <summary>
        /// Build resume message
        /// </summary>
        /// <returns>Built message</returns>
        internal string BuildResumeMessage() => "pause 0";

        /// <summary>
        /// Build pause request message
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Built message</returns>
        internal string BuildPauseRequestMessage(IUser user) => ("pause 1 " + user.ID);

        /// <summary>
        /// Build resume request message
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Built message</returns>
        internal string BuildResumeRequestMessage(IUser user) => ("pause 0 " + user.ID);

        /// <summary>
        /// Build end game message
        /// </summary>
        /// <returns>Built message</returns>
        internal string BuildEndGameMessage() => "endgame";

        /// <summary>
        /// Build no connecting message
        /// </summary>
        /// <param name="reason">Reason</param>
        /// <returns>Built message</returns>
        internal string BuildNoConnectingMessage(string reason) => ("noconnecting " + reason);

        /// <summary>
        /// Build client information request message
        /// </summary>
        /// <returns>Built message</returns>
        internal string BuildClientInfoRequestMessage() => "lvl 0 0";

        /// <summary>
        /// Build heartbeat message
        /// </summary>
        /// <returns>Built message</returns>
        internal string BuildHeartbeatMessage() => string.Empty;
    }
}
