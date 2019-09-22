/// <summary>
/// TetriEngine networking lobby namespace
/// </summary>
namespace TetriEngine.Networking.Lobby
{
    /// <summary>
    /// User special used delegate
    /// </summary>
    /// <param name="senderUser">Sender user</param>
    /// <param name="targetUser">Target user</param>
    /// <param name="special">Special</param>
    public delegate void UserSpecialUsedDelegate(IUser senderUser, IUser targetUser, ESpecial special);
}
