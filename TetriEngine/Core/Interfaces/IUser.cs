/// <summary>
/// TetriEngine namespace
/// </summary>
namespace TetriEngine
{
    /// <summary>
    /// User interface
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// ID
        /// </summary>
        int ID { get; }

        /// <summary>
        /// Name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Team name
        /// </summary>
        string TeamName { get; }

        /// <summary>
        /// Score
        /// </summary>
        long Score { get; }

        /// <summary>
        /// Level
        /// </summary>
        uint Level { get; }

        /// <summary>
        /// Field
        /// </summary>
        IField Field { get; }
    }
}
