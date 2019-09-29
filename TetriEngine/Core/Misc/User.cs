/// <summary>
/// TetriEngine namespace
/// </summary>
namespace TetriEngine
{
    /// <summary>
    /// User class
    /// </summary>
    internal class User : IHostUser
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Team name
        /// </summary>
        public string TeamName { get; private set; } = string.Empty;

        /// <summary>
        /// Score
        /// </summary>
        public long Score { get; internal set; }

        /// <summary>
        /// Level
        /// </summary>
        public uint Level { get; internal set; } = 1U;

        /// <summary>
        /// Field (internal)
        /// </summary>
        public Field FieldInternal { get; private set; }

        /// <summary>
        /// Inventory internal
        /// </summary>
        public Inventory InventoryInternal { get; private set; }

        /// <summary>
        /// Field
        /// </summary>
        public IField Field => FieldInternal;

        /// <summary>
        /// Inventory
        /// </summary>
        public IInventory Inventory => InventoryInternal;

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="user">User</param>
        internal User(IUser user)
        {
            Name = user.Name;
            ID = user.ID;
            TeamName = user.TeamName;
            Score = user.Score;
            FieldInternal = new Field(user.Field);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Name</param>
        internal User(int id, string name)
        {
            Name = name;
            ID = id;
            FieldInternal = new Field();
            InventoryInternal = new Inventory();
        }

        /// <summary>
        /// Set team name
        /// </summary>
        /// <param name="teamName">Team name</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        public bool SetTeamName(string teamName)
        {
            bool ret = (teamName != null);
            if (ret)
            {
                TeamName = teamName.Trim();
            }
            return ret;
        }

        /// <summary>
        /// Set level
        /// </summary>
        /// <param name="level">Level</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        public bool SetLevel(uint level)
        {
            bool ret = (level > 0U);
            if (ret)
            {
                Level = level;
            }
            return ret;
        }

        /// <summary>
        /// Set score
        /// </summary>
        /// <param name="score">Score</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        public bool SetScore(long score)
        {
            bool ret = (score >= 0L);
            if (ret)
            {
                Score = score;
            }
            return ret;
        }
    }
}
