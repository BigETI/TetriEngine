/// <summary>
/// TetriEngine networking lobby namespace
/// </summary>
namespace TetriEngine.Networking.Lobby
{
    /// <summary>
    /// User level update delegate
    /// </summary>
    /// <param name="user">User</param>
    /// <param name="oldLevel">Old level</param>
    /// <param name="newLevel">New level</param>
    public delegate void UserLevelUpdateDelegate(IUser user, uint oldLevel, uint newLevel);
}
