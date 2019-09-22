/// <summary>
/// TetriEngine networking connection namespace
/// </summary>
namespace TetriEngine.Networking.Connection
{
    /// <summary>
    /// User joined delegate
    /// </summary>
    /// <param name="userID">User ID</param>
    /// <param name="username">Username</param>
    public delegate void UserJoinedDelegate(int userID, string username);
}
