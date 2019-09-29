using System.Net.Sockets;

/// <summary>
/// TetriEngine networking listener namespace
/// </summary>
namespace TetriEngine.Networking.Listener
{
    /// <summary>
    /// Client denied connection delegate
    /// </summary>
    /// <param name="tcpClient"></param>
    public delegate void ClientConnectionDeniedDelegate(TcpClient tcpClient);
}
