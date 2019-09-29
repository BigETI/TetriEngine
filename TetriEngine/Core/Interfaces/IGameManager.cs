using System.Collections.Generic;

/// <summary>
/// TetriEngine namespace
/// </summary>
namespace TetriEngine
{
    /// <summary>
    /// Game manager interface
    /// </summary>
    public interface IGameManager
    {
        /// <summary>
        /// Field
        /// </summary>
        IUser User { get; }

        /// <summary>
        /// Users
        /// </summary>
        IReadOnlyPool<IUser> Users { get; }

        /// <summary>
        /// Chat messages
        /// </summary>
        IReadOnlyList<string> ChatMessages { get; }

        /// <summary>
        /// Can perform action
        /// </summary>
        bool CanPerformAction { get; }

        /// <summary>
        /// Game options
        /// </summary>
        GameOptions GameOptions { get; }

        /// <summary>
        /// Is paused
        /// </summary>
        bool IsPaused { get; }

        /// <summary>
        /// Winlist
        /// </summary>
        Winlist Winlist { get; }

        /// <summary>
        /// Drop block
        /// </summary>
        /// <returns>"true" if possible, otherwise "false"</returns>
        bool DropBlock();

        /// <summary>
        /// Move block left
        /// </summary>
        /// <returns>"true" if possible, otherwise "false"</returns>
        bool MoveBlockLeft();

        /// <summary>
        /// Move block right
        /// </summary>
        /// <returns>"true" if possible, otherwise "false"</returns>
        bool MoveBlockRight();

        /// <summary>
        /// Move block down
        /// </summary>
        /// <returns>"true" if possible, otherwise "false"</returns>
        bool MoveBlockDown();

        /// <summary>
        /// Turn block left
        /// </summary>
        /// <returns>"true" if successful, otherwise "false"</returns>
        bool TurnBlockLeft();

        /// <summary>
        /// Turn block right
        /// </summary>
        /// <returns>"true" if successful, otherwise "false"</returns>
        bool TurnBlockRight();

        /// <summary>
        /// Use special
        /// </summary>
        /// <param name="special">Special</param>
        /// <returns>"true" if possible, otherwise "false"</returns>
        bool UseSpecial(ESpecial special);
    }
}
