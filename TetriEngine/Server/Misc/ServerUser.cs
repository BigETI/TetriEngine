/// <summary>
/// TetriEngine server namespace
/// </summary>
namespace TetriEngine.Server
{
    /// <summary>
    /// Server user class
    /// </summary>
    internal class ServerUser : User
    {
        /// <summary>
        /// Server connection
        /// </summary>
        public ServerConnection ServerConnection { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Name</param>
        /// <param name="serverConnection">Server connection</param>
        internal ServerUser(int id, string name, ServerConnection serverConnection) : base(id, name)
        {
            ServerConnection = serverConnection;
        }
    }
}
