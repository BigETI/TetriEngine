/// <summary>
/// TetriEngine networking lobby namespace
/// </summary>
namespace TetriEngine.Networking.Lobby
{
    /// <summary>
    /// Classic mode add lines delegate
    /// </summary>
    /// <param name="user">User</param>
    /// <param name="numLines">Number of lines</param>
    public delegate void ClassicModeAddLinesDelegate(IUser user, uint numLines);
}
