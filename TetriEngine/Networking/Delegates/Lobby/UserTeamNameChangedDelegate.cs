/// <summary>
/// TetriEngine networking lobby namespace
/// </summary>
namespace TetriEngine.Networking.Lobby
{
    /// <summary>
    /// User team name changed delegate
    /// </summary>
    /// <param name="user">User</param>
    /// <param name="oldTeamName">Old team name</param>
    /// <param name="newTeamName">New team name</param>
    public delegate void UserTeamNameChangedDelegate(IUser user, string oldTeamName, string newTeamName);
}
