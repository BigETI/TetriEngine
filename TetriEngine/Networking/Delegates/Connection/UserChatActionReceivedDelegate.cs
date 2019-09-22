/// <summary>
/// TetriEngine networking connection namespace
/// </summary>
namespace TetriEngine.Networking.Connection
{
    /// <summary>
    /// User chat action received delegate
    /// </summary>
    /// <param name="userID">User ID</param>
    /// <param name="action">Action</param>
    public delegate void UserChatActionReceivedDelegate(int userID, string action);
}
