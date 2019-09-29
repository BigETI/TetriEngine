using TetriEngine.Server;

/// <summary>
/// TetriEngine networking listener namespace
/// </summary>
namespace TetriEngine.Networking.Listener
{
    /// <summary>
    /// Client accepted connection delegate
    /// </summary>
    /// <param name="serverConnection">Server connection</param>
    public delegate void ClientConnectionAcceptedDelegate(ServerConnection serverConnection);
}
