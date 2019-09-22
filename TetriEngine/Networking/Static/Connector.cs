using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using TetriEngine.Client;
using TetriEngine.Server;

/// <summary>
/// TetriEngine networking namespace
/// </summary>
namespace TetriEngine.Networking
{
    /// <summary>
    /// Connector class
    /// </summary>
    internal static class Connector
    {
        /// <summary>
        /// Default port
        /// </summary>
        public static readonly ushort defaultPort = 31457;

        /// <summary>
        /// Default maximal amount of users
        /// </summary>
        public static readonly uint defaultMaxUsers = 6U;

        /// <summary>
        /// Receive buffer size
        /// </summary>
        public static readonly int defaultReceiveBufferSize = 4 * 1024 * 1024;

        /// <summary>
        /// Send buffer size
        /// </summary>
        public static readonly int defaultSendBufferSize = 4 * 1024 * 1024;

        /// <summary>
        /// Receive timeout
        /// </summary>
        public static readonly int defaultReceiveTimeout = 4000;

        /// <summary>
        /// Send timeout
        /// </summary>
        public static readonly int defaultSendTimeout = 4000;

        /// <summary>
        /// Get IPv4 address bytes
        /// </summary>
        /// <param name="ipv4Address">IPv4 address</param>
        /// <returns>IPv4 bytes if successful, otherwise "null"</returns>
        private static byte[] GetIPv4AddressBytes(IPAddress ipv4Address)
        {
            byte[] ret = null;
            byte[] address_bytes = ipv4Address.GetAddressBytes();
            if (address_bytes != null)
            {
                if (address_bytes.Length == 4)
                {
                    ret = address_bytes;
                }
            }
            return ret;
        }

        /// <summary>
        /// Connect (asynchronous)
        /// </summary>
        /// <param name="hostname">Hostname</param>
        /// <param name="port">Port (if 0, default TetriNET port used)</param>
        /// <param name="username">Username</param>
        /// <param name="teamName">Team name</param>
        /// <returns>Tetri connection task</returns>
        internal static Task<ClientConnection> ConnectClientAsync(string hostname, ushort port, string username, string teamName)
        {
            Task<ClientConnection> ret = Task.FromResult<ClientConnection>(null);
            if ((hostname != null) && (username != null) && (teamName != null))
            {
                ushort p = ((port > 0) ? port : defaultPort);
                ret = new Task<ClientConnection>(() =>
                {
                    ClientConnection client_connection = null;
                    TcpClient tcp_client = new TcpClient(AddressFamily.InterNetwork);
                    tcp_client.ReceiveBufferSize = defaultReceiveBufferSize;
                    tcp_client.SendBufferSize = defaultSendBufferSize;
                    tcp_client.ReceiveTimeout = defaultReceiveTimeout;
                    tcp_client.SendTimeout = defaultSendTimeout;
                    tcp_client.NoDelay = true;
                    tcp_client.Connect(hostname, port);
                    byte[] ipv4_address = null;
                    AddressFamily used_address_family = AddressFamily.Unspecified;
                    if (tcp_client.Client.LocalEndPoint is IPEndPoint)
                    {
                        IPEndPoint ip_endpoint = (IPEndPoint)(tcp_client.Client.LocalEndPoint);
                        if (ip_endpoint != null)
                        {
                            used_address_family = ip_endpoint.AddressFamily;
                            if (used_address_family == AddressFamily.InterNetwork)
                            {
                                ipv4_address = GetIPv4AddressBytes(ip_endpoint.Address);
                            }
                        }
                    }
                    else if (tcp_client.Client.LocalEndPoint is DnsEndPoint)
                    {
                        IPAddress[] ip_addresses = Dns.GetHostAddresses(((DnsEndPoint)(tcp_client.Client.LocalEndPoint)).Host);
                        if (ip_addresses != null)
                        {
                            foreach (IPAddress ip_address in ip_addresses)
                            {
                                if (ip_address != null)
                                {
                                    if (ip_address.AddressFamily == AddressFamily.InterNetwork)
                                    {
                                        ipv4_address = GetIPv4AddressBytes(ip_address);
                                        if (ipv4_address != null)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (ipv4_address == null)
                    {
                        tcp_client.Dispose();
                        throw new UnsupportedAddressFamilyException(used_address_family);
                    }
                    else
                    {
                        client_connection = new ClientConnection(tcp_client, ipv4_address, username, teamName);
                    }
                    return client_connection;
                });
                ret.Start();
            }
            return ret;
        }

        /// <summary>
        /// Create listener (asynchronous)
        /// </summary>
        /// <param name="protocol">Protocol</param>
        /// <param name="port">Port (if 0, default TetriNET port used)</param>
        /// <param name="maxUsers">Maximal amount of users (if 0, default TetriNET maximum of users is used)</param>
        /// <returns>Server connection task</returns>
        internal static Task<ServerConnection> CreateListenerAsync(EProtocol protocol, ushort port, uint maxUsers)
        {
            //ushort p = ((port > 0) ? port : defaultPort);
            //uint max_users = ((maxUsers > 0) ? maxUsers : defaultMaxUsers);
            //Task<ServerConnection> ret = new Task<ServerConnection>(() =>
            //{
            //    ServerConnection server_connection = null;
            //    TcpListener tcp_listener = TcpListener.Create(p);
            //    tcp_listener.Start((int)max_users);
            //    AddressFamily used_address_family = tcp_listener.LocalEndpoint.AddressFamily;
            //    if (used_address_family == AddressFamily.InterNetwork)
            //    {
            //        server_connection = new ServerConnection(tcp_listener, protocol);
            //    }
            //    else
            //    {
            //        tcp_listener.Stop();
            //        throw new UnsupportedAddressFamilyException(used_address_family);
            //    }
            //    return server_connection;
            //});
            //ret.Start();
            //return ret;

            // TODO
            throw new NotImplementedException();
        }
    }
}
