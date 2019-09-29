using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using TetriEngine.Networking;
using TetriEngine.Networking.Listener;

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
        /// Server connections
        /// </summary>
        private List<ServerConnection> serverConnections = new List<ServerConnection>();

        /// <summary>
        /// Listener thread
        /// </summary>
        private Thread listenerThread;

        /// <summary>
        /// Protocol
        /// </summary>
        public EProtocol Protocol { get; private set; }

        /// <summary>
        /// TCP listener
        /// </summary>
        public TcpListener TCPListener { get; private set; }

        /// <summary>
        /// Whitelist
        /// </summary>
        public PatternCollection Whitelist { get; } = new PatternCollection();

        /// <summary>
        /// Bans
        /// </summary>
        public PatternCollection Bans { get; } = new PatternCollection();

        /// <summary>
        /// On client connection accepted event
        /// </summary>
        public event ClientConnectionAcceptedDelegate OnClientConnectionAccepted;

        /// <summary>
        /// On client connection denied event
        /// </summary>
        public event ClientConnectionDeniedDelegate OnClientConnectionDenied;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="protocol">Protocol</param>
        /// <param name="tcpListener">TCP listener</param>
        private ServerListener(EProtocol protocol, TcpListener tcpListener)
        {
            Protocol = protocol;
            TCPListener = tcpListener;
            listenerThread = new Thread((that) =>
            {
                if (that is ServerListener)
                {
                    ServerListener listener = (ServerListener)that;
                    while (listener.TCPListener != null)
                    {
                        lock (listener.TCPListener)
                        {
                            if (listener.TCPListener.Pending())
                            {
                                TcpClient tcp_client = listener.TCPListener.AcceptTcpClient();
                                if (tcp_client != null)
                                {
                                    bool allow = (Whitelist.Count <= 0);
                                    string address = ((IPEndPoint)(tcp_client.Client.RemoteEndPoint)).Address.ToString();
                                    if (!allow)
                                    {
                                        allow = Whitelist.Contains(address);
                                    }
                                    if (allow)
                                    {
                                        allow = !(Bans.Contains(address));
                                    }
                                    if (allow)
                                    {
                                        ServerConnection server_connection = new ServerConnection(tcp_client, protocol);
                                        lock (listener.serverConnections)
                                        {
                                            listener.serverConnections.Add(server_connection);
                                        }
                                        OnClientConnectionAccepted?.Invoke(server_connection);
                                    }
                                    else
                                    {
                                        OnClientConnectionDenied?.Invoke(tcp_client);
                                        tcp_client.Dispose();
                                    }
                                }
                            }
                        }
                        lock (listener.serverConnections)
                        {
                            List<Tuple<int, ServerConnection>> dispose_server_connections = null;
                            for (int i = 0; i < listener.serverConnections.Count; i++)
                            {
                                ServerConnection server_connection = listener.serverConnections[i];
                                if ((!(server_connection.CanReceive)) && (!(server_connection.CanSend)))
                                {
                                    if (dispose_server_connections == null)
                                    {
                                        dispose_server_connections = new List<Tuple<int, ServerConnection>>();
                                    }
                                    dispose_server_connections.Add(new Tuple<int, ServerConnection>(i, server_connection));
                                }
                            }
                            dispose_server_connections.Reverse();
                            foreach (Tuple<int, ServerConnection> server_connection in dispose_server_connections)
                            {
                                listener.serverConnections.RemoveAt(server_connection.Item1);
                            }
                        }
                        Thread.Sleep(20);
                    }
                }
            });
            listenerThread.Start(this);
        }

        /// <summary>
        /// Disconnect
        /// </summary>
        /// <param name="serverConnection">Server connection</param>
        internal void Disconnect(ServerConnection serverConnection)
        {
            lock (serverConnections)
            {
                serverConnections.Remove(serverConnection);
            }
        }

        /// <summary>
        /// Create server listener
        /// </summary>
        /// <param name="protocol">Protocol</param>
        /// <param name="port">Port</param>
        /// <param name="maxUsers">Maximal amount of users</param>
        /// <returns></returns>
        public static ServerListener Create(EProtocol protocol, ushort port, uint maxUsers)
        {
            ServerListener ret = null;
            TcpListener tcp_listener = TcpListener.Create(port);
            tcp_listener.Start((int)maxUsers);
            AddressFamily used_address_family = tcp_listener.LocalEndpoint.AddressFamily;
            if (used_address_family == AddressFamily.InterNetwork)
            {
                ret = new ServerListener(protocol, tcp_listener);
            }
            else
            {
                tcp_listener.Stop();
                throw new UnsupportedAddressFamilyException(used_address_family);
            }
            return ret;
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
            if (listenerThread != null)
            {
                listenerThread.Join();
                listenerThread = null;
            }
            lock (serverConnections)
            {
                foreach (ServerConnection server_connection in serverConnections)
                {
                    server_connection.Dispose();
                }
                serverConnections.Clear();
            }
        }
    }
}
