/// <summary>
/// TetriEngine networking connection namespace
/// </summary>
namespace TetriEngine.Networking.Connection
{
    /// <summary>
    /// Server special used delegate
    /// </summary>
    /// <param name="targetUserID">Target user ID</param>
    /// <param name="special">Special</param>
    public delegate void ServerSpecialUsedDelegate(int targetUserID, ESpecial special);
}
