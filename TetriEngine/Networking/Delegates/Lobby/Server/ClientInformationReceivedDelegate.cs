/// <summary>
/// TetriEngine networking lobby server namespace
/// </summary>
namespace TetriEngine.Networking.Lobby.Server
{
    /// <summary>
    /// Client information received delegate
    /// </summary>
    /// <param name="user">User</param>
    /// <param name="clientName">Client name</param>
    /// <param name="clientVersion">Client version</param>
    public delegate void ClientInformationReceivedDelegate(IUser user, string clientName, string clientVersion);
}
