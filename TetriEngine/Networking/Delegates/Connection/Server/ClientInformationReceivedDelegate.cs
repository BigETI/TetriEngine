/// <summary>
/// TetriEngine networking connection server namespace
/// </summary>
namespace TetriEngine.Networking.Connection.Server
{
    /// <summary>
    /// Client information received delegate
    /// </summary>
    /// <param name="clientName">Client name</param>
    /// <param name="clientVersion">Client version</param>
    public delegate void ClientInformationReceivedDelegate(string clientName, string clientVersion);
}
