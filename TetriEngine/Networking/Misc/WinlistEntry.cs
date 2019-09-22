/// <summary>
/// TetriEngine networking namespace
/// </summary>
namespace TetriEngine.Networking
{
    /// <summary>
    /// Winlist entry structure
    /// </summary>
    public struct WinlistEntry
    {
        /// <summary>
        /// Is team
        /// </summary>
        public bool IsTeam { get; private set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Score
        /// </summary>
        public long Score { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="isTeam">Is team</param>
        /// <param name="name">Name</param>
        /// <param name="score">Score</param>
        public WinlistEntry(bool isTeam, string name, long score)
        {
            IsTeam = isTeam;
            Name = name;
            Score = score;
        }
    }
}
