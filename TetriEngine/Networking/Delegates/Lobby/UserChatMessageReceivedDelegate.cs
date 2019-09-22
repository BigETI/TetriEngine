/// <summary>
/// TetriEngine networking lobby namespace
/// </summary>
namespace TetriEngine.Networking.Lobby
{
    /// <summary>
    /// User chat message received delegate
    /// </summary>
    /// <param name="user">User</param>
    /// <param name="message">Message</param>
    public delegate void UserChatMessageReceivedDelegate(IUser user, string message);
}
