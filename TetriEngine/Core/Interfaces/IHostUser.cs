/// <summary>
/// TetriEngine namespace
/// </summary>
namespace TetriEngine
{
    /// <summary>
    /// Host user interface
    /// </summary>
    internal interface IHostUser : IUser
    {
        /// <summary>
        /// Field internal
        /// </summary>
        Field FieldInternal { get; }

        /// <summary>
        /// Inventory internal
        /// </summary>
        Inventory InventoryInternal { get; }

        /// <summary>
        /// Set team name
        /// </summary>
        /// <param name="teamName">Team name</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        bool SetTeamName(string teamName);

        /// <summary>
        /// Set level
        /// </summary>
        /// <param name="level">Level</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        bool SetLevel(uint level);

        /// <summary>
        /// Set score
        /// </summary>
        /// <param name="score">Score</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        bool SetScore(long score);
    }
}
