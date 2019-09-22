/// <summary>
/// TetriEngine networking connection namespace
/// </summary>
namespace TetriEngine.Networking.Connection
{
    /// <summary>
    /// User team name changed connection delegate
    /// </summary>
    /// <param name="userID">User ID</param>
    /// <param name="newTeamName">New team name</param>
    public delegate void UserTeamNameChangedDelegate(int userID, string newTeamName);
}
