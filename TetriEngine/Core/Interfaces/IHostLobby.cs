/// <summary>
/// TetriEngine namespace
/// </summary>
namespace TetriEngine
{
    /// <summary>
    /// Host lobby interface
    /// </summary>
    public interface IHostLobby : ILobby
    {
        /// <summary>
        /// Start game
        /// </summary>
        IGameManager StartGame();

        /// <summary>
        /// Stop game
        /// </summary>
        /// <returns>"true" if successful, otherwise "false"</returns>
        bool StopGame();

        /// <summary>
        /// Add bot
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>User if successful, otherwise "null"</returns>
        IUser AddBotUser(string username);
    }
}
