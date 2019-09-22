using System.Collections.Generic;

/// <summary>
/// TetriEngine core namespace
/// </summary>
namespace TetriEngine
{
    /// <summary>
    /// Read only pool interface
    /// </summary>
    /// <typeparam name="T">Entry type</typeparam>
    public interface IReadOnlyPool<T> : IReadOnlyCollection<T> where T : class
    {
        /// <summary>
        /// Invalid ID
        /// </summary>
        int InvalidID { get; }

        /// <summary>
        /// Maximal amount of entries
        /// </summary>
        uint MaxEntries { get; }

        /// <summary>
        /// Is ID valid
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>"true" if valid, otherwise "false"</returns>
        bool IsIDValid(int id);

        /// <summary>
        /// Array access operator
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>Element</returns>
        T this[int id] { get; }
    }
}
