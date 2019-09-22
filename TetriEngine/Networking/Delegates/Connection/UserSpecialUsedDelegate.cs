/// <summary>
/// TetriEngine networking connection namespace
/// </summary>
namespace TetriEngine.Networking.Connection
{
    /// <summary>
    /// User special used delegate
    /// </summary>
    /// <param name="senderUserID">Sender user ID</param>
    /// <param name="targetUserID">Target user ID</param>
    /// <param name="special">Special</param>
    public delegate void UserSpecialUsedDelegate(int senderUserID, int targetUserID, ESpecial special);
}
