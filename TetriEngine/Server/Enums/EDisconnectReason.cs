/// <summary>
/// TetriEngine server namespace
/// </summary>
namespace TetriEngine.Server
{
    /// <summary>
    /// Disconnect reason enumerator
    /// </summary>
    public enum EDisconnectReason
    {
        /// <summary>
        /// Left
        /// </summary>
        Left,

        /// <summary>
        /// Kicked
        /// </summary>
        Kicked,

        /// <summary>
        /// Banned
        /// </summary>
        Banned,

        /// <summary>
        /// Timed out
        /// </summary>
        TimedOut
    }
}
