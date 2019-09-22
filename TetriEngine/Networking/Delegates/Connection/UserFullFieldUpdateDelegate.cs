/// <summary>
/// TetriEngine networking connection namespace
/// </summary>
namespace TetriEngine.Networking.Connection
{
    /// <summary>
    /// User full field update delegate
    /// </summary>
    /// <param name="userID">User ID</param>
    /// <param name="cells">Cells</param>
    public delegate void UserFullFieldUpdateDelegate(int userID, ECell[] cells);
}
