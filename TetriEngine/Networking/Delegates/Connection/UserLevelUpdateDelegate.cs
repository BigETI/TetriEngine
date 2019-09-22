/// <summary>
/// TetriEngine networking connection namespace
/// </summary>
namespace TetriEngine.Networking.Connection
{
    /// <summary>
    /// User level update delegate
    /// </summary>
    /// <param name="userID">User ID</param>
    /// <param name="newLevel">New level</param>
    public delegate void UserLevelUpdateDelegate(int userID, uint newLevel);
}
