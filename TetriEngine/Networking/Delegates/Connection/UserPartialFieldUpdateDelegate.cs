/// <summary>
/// TetriEngine networking connection namespace
/// </summary>
namespace TetriEngine.Networking.Connection
{
    /// <summary>
    /// User partial field update delegate
    /// </summary>
    /// <param name="userID">User ID</param>
    /// <param name="cellPositions">Call positions</param>
    public delegate void UserPartialFieldUpdateDelegate(int userID, CellPosition[] cellPositions);
}
