/// <summary>
/// TetriEngine namespace
/// </summary>
namespace TetriEngine
{
    /// <summary>
    /// Team class
    /// </summary>
    internal class Team : ITeam
    {
        /// <summary>
        /// Name
        /// </summary>
        private string name = string.Empty;

        /// <summary>
        /// Name
        /// </summary>
        public string Name
        {
            get => name;
            private set
            {
                if (value != null)
                {
                    name = value;
                }
            }
        }

        /// <summary>
        /// Score
        /// </summary>
        public long Score { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="score">Score</param>
        internal Team(string name, long score)
        {
            Name = name;
            Score = score;
        }
    }
}
