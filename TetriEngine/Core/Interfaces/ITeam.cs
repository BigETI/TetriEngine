/// <summary>
/// TetriEngine namespace
/// </summary>
namespace TetriEngine
{
    /// <summary>
    /// Team interface
    /// </summary>
    public interface ITeam
    {
        /// <summary>
        /// Name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Score
        /// </summary>
        long Score { get; }
    }
}
