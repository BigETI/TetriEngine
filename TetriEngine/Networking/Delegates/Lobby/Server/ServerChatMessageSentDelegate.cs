/// <summary>
/// TetriEngine networking lobby server namespace
/// </summary>
namespace TetriEngine.Networking.Lobby.Server
{
    /// <summary>
    /// Server chat message delegate
    /// </summary>
    /// <param name="targetUser">Target user</param>
    /// <param name="message">Message</param>
    public delegate void ServerChatMessageSentDelegate(IUser targetUser, string message);
}
