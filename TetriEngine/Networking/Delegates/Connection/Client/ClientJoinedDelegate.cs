/// <summary>
/// TetriEngine networking connection client namespace
/// </summary>
namespace TetriEngine.Networking.Connection.Client
{
    /// <summary>
    /// Client joined delegate
    /// </summary>
    /// <param name="userID">User ID</param>
    /// <param name="protocol">Protocol</param>
    public delegate void ClientJoinedDelegate(int userID, EProtocol protocol);
}
