using System;

/// <summary>
/// TetriEngine namespace
/// </summary>
namespace TetriEngine
{
    /// <summary>
    /// Invalid ID exception
    /// </summary>
    public class InvalidIDException : Exception
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">ID</param>
        public InvalidIDException(int id) : base("ID " + id + " is invalid.")
        {
            ID = id;
        }
    }
}
