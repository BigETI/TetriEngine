using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// TetriEngine namespace
/// </summary>
namespace TetriEngine
{
    /// <summary>
    /// Pool interface
    /// </summary>
    /// <typeparam name="T">Entry type</typeparam>
    public interface IPool<T> : IReadOnlyPool<T> where T : class
    {
        /// <summary>
        /// Next available ID
        /// </summary>
        int NextAvailableID { get; }

        /// <summary>
        /// Add entry
        /// </summary>
        /// <param name="entry">Entry</param>
        /// <returns>Valid ID if successful otherwise an invalid ID</returns>
        int Add(T entry);

        /// <summary>
        /// Insert entry
        /// </summary>
        /// <param name="entry">Entry</param>
        /// <param name="id">ID</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        bool Insert(T entry, int id);

        /// <summary>
        /// Remove entry by ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        bool Remove(int id);

        /// <summary>
        /// Clear pool
        /// </summary>
        void Clear();
    }
}
