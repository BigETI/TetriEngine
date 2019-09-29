/// <summary>
/// TetriEngine networking connection server namespace
/// </summary>
namespace TetriEngine.Networking.Connection.Server
{
    /// <summary>
    /// Client log in delegate
    /// </summary>
    /// <param name="username">Username</param>
    /// <param name="version">Version</param>
    /// <param name="protocol">Protocol</param>
    public delegate void ClientLogInDelegate(string username, string version, EProtocol protocol);
}
