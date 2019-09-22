using System;
using System.Net.Sockets;
using TetriEngine.Networking;

/// <summary>
/// TetriEngine server namespace
/// </summary>
namespace TetriEngine.Server
{
    /// <summary>
    /// Server listener
    /// </summary>
    internal class ServerListener : IDisposable
    {
        /// <summary>
        /// Protocol
        /// </summary>
        public EProtocol Protocol { get; private set; }

        /// <summary>
        /// TCP listener
        /// </summary>
        public TcpListener TCPListener { get; private set; }

        ///// <summary>
        ///// On client awaiting connection
        ///// </summary>
        //public event ClientAwaitingConnectionDelegate OnClientAwaitingConnection;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="protocol">Protocol</param>
        /// <param name="tcpListener">TCP listener</param>
        private ServerListener(EProtocol protocol, TcpListener tcpListener)
        {
            Protocol = protocol;
            TCPListener = tcpListener;
        }

        /// <summary>
        /// Create server listener
        /// </summary>
        /// <param name="port">Port</param>
        /// <param name="maxUsers">Maximal amount of users</param>
        /// <returns></returns>
        public static ServerListener Create(ushort port, uint maxUsers)
        {
            /*ServerListener ret = null;
            TcpListener tcp_listener = TcpListener.Create(port);
            tcp_listener.Start((int)maxUsers);
            AddressFamily used_address_family = tcp_listener.LocalEndpoint.AddressFamily;
            if (used_address_family == AddressFamily.InterNetwork)
            {
                server_connection = new ServerConnection(tcp_listener, protocol);
            }
            else
            {
                tcp_listener.Stop();
                throw new UnsupportedAddressFamilyException(used_address_family);
            }
            return ret;*/

            // TODO
            throw new NotImplementedException();
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (TCPListener != null)
            {
                TCPListener.Stop();
                TCPListener = null;
            }
        }
    }
}
