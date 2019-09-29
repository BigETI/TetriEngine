/// <summary>
/// TetriEngine networking lobby server namespace
/// </summary>
namespace TetriEngine.Networking.Lobby.Server
{
    /// <summary>
    /// Server chat action sent delegate
    /// </summary>
    /// <param name="targetUser">Target user</param>
    /// <param name="action">Action</param>
    public delegate void ServerChatActionSentDelegate(IUser targetUser, string action);
}
