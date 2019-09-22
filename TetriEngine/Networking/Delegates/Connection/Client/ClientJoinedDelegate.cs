/// <summary>
/// TetriEngine networking connection client namespace
/// </summary>
namespace TetriEngine.Networking.Connection.Client
{
    /// <summary>
    /// Client joined delegate
    /// </summary>
    /// <param name="userID">User ID</param>
    /// <param name="username">Username</param>
    public delegate void ClientJoinedDelegate(int userID, string username);
}
