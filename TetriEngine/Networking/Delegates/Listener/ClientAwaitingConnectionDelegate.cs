using System.Net.Sockets;

/// <summary>
/// TetriEngine networking listener namespace
/// </summary>
namespace TetriEngine.Networking.Listener
{
    /// <summary>
    /// Client awaiting connection delegate
    /// </summary>
    /// <param name="tcpClient">TCP client</param>
    public delegate void ClientAwaitingConnectionDelegate(TcpClient tcpClient);
}
