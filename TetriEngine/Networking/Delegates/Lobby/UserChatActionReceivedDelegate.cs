/// <summary>
/// TetriEngine networking lobby namespace
/// </summary>
namespace TetriEngine.Networking.Lobby
{
    /// <summary>
    /// User chat action received delegate
    /// </summary>
    /// <param name="user">User</param>
    /// <param name="action">Action</param>
    public delegate void UserChatActionReceivedDelegate(IUser user, string action);
}
