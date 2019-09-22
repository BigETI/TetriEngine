using System;
using System.Net.Sockets;
using System.Threading;

/// <summary>
/// TetriEngine networking namespace
/// </summary>
namespace TetriEngine.Networking
{
    /// <summary>
    /// Network user class
    /// </summary>
    internal class NetworkUser : User, IDisposable
    {
        /// <summary>
        /// Listener thread
        /// </summary>
        private Thread listenerThread;

        /// <summary>
        /// TCP client
        /// </summary>
        public TcpClient TCPClient { get; private set; }

        /// <summary>
        /// Network stream
        /// </summary>
        public NetworkStream NetworkStream { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Name</param>
        /// <param name="listenerThread">Listener thread</param>
        /// <param name="tcpClient">TCP client</param>
        /// <param name="networkStream">Network stream</param>
        internal NetworkUser(int id, string name, Thread listenerThread, TcpClient tcpClient, NetworkStream networkStream) : base(id, name)
        {
            this.listenerThread = listenerThread;
            TCPClient = tcpClient;
            NetworkStream = networkStream;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (TCPClient != null)
            {
                TCPClient.Close();
                TCPClient = null;
                NetworkStream = null;
            }
            if (listenerThread != null)
            {
                listenerThread.Join();
                listenerThread = null;
            }
        }
    }
}
