/// <summary>
/// TetriEngine networking lobby namespace
/// </summary>
namespace TetriEngine.Networking.Lobby
{
    /// <summary>
    /// User special used for all delegate
    /// </summary>
    /// <param name="senderUser">Sender user</param>
    /// <param name="special">Special</param>
    public delegate void UserSpecialUsedForAllDelegate(IUser senderUser, ESpecial special);
}
