/// <summary>
/// TetriEngine networking lobby server namespace
/// </summary>
namespace TetriEngine.Networking.Lobby.Server
{
    /// <summary>
    /// Client log in delegate
    /// </summary>
    /// <param name="user">User</param>
    /// <param name="version">Version</param>
    /// <param name="protocol">Protocol</param>
    public delegate void ClientLogInDelegate(IUser user, string version, EProtocol protocol);
}
