/// <summary>
/// TetriEngine networking connection namespace
/// </summary>
namespace TetriEngine.Networking.Connection
{
    /// <summary>
    /// Classic mode add lines delegate
    /// </summary>
    /// <param name="userID">User ID</param>
    /// <param name="numLines">Number of lines</param>
    public delegate void ClassicModeAddLinesDelegate(int userID, uint numLines);
}
