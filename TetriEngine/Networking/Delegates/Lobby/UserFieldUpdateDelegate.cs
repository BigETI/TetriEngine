/// <summary>
/// TetriEngine networking lobby namespace
/// </summary>
namespace TetriEngine.Networking.Lobby
{
    /// <summary>
    /// User field update delegate
    /// </summary>
    /// <param name="user">User</param>
    /// <param name="oldCells">Old cells</param>
    /// <param name="newCells">New cells</param>
    public delegate void UserFieldUpdateDelegate(IUser user, ECell[] oldCells, ECell[] newCells);
}
