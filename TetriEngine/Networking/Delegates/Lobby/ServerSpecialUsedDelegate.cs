/// <summary>
/// TetriEngine networking lobby namespace
/// </summary>
namespace TetriEngine.Networking.Lobby
{
    /// <summary>
    /// Server special used delegate
    /// </summary>
    /// <param name="targetUser">Target user</param>
    /// <param name="special">Special</param>
    public delegate void ServerSpecialUsedDelegate(IUser targetUser, ESpecial special);
}
