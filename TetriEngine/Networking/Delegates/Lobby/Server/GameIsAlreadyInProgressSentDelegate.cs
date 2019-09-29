/// <summary>
/// TetriEngine networking lobby server namespace
/// </summary>
namespace TetriEngine.Networking.Lobby.Server
{
    /// <summary>
    /// Game is already in progress sent delegate
    /// </summary>
    /// <param name="targetUser">Target user</param>
    public delegate void GameIsAlreadyInProgressSentDelegate(IUser targetUser);
}
