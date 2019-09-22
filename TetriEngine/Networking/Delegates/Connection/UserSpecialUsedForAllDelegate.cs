/// <summary>
/// TetriEngine networking connection namespace
/// </summary>
namespace TetriEngine.Networking.Connection
{
    /// <summary>
    /// User special used for all delegate
    /// </summary>
    /// <param name="senderUserID">Sender user ID</param>
    /// <param name="special">Special</param>
    public delegate void UserSpecialUsedForAllDelegate(int senderUserID, ESpecial special);
}
