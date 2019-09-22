/// <summary>
/// TetriEngine networking connection namespace
/// </summary>
namespace TetriEngine.Networking.Connection
{
    /// <summary>
    /// User chat message received delegate
    /// </summary>
    /// <param name="userID">User ID</param>
    /// <param name="message">Message</param>
    public delegate void UserChatMessageReceivedDelegate(int userID, string message);
}
