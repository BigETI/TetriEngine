using System.Collections.Generic;

/// <summary>
/// TetriEngine namespace
/// </summary>
namespace TetriEngine
{
    /// <summary>
    /// Winlist interface
    /// </summary>
    public interface IWinlist
    {
        /// <summary>
        /// Users
        /// </summary>
        IReadOnlyList<IUser> Users { get; }

        /// <summary>
        /// Teams
        /// </summary>
        IReadOnlyList<ITeam> Teams { get; }
    }
}
