/// <summary>
/// TetriEngine namespace
/// </summary>
namespace TetriEngine
{
    /// <summary>
    /// User class
    /// </summary>
    internal class User : IUser
    {
        /// <summary>
        /// Name
        /// </summary>
        private string name = string.Empty;

        /// <summary>
        /// Team name
        /// </summary>
        private string teamName = string.Empty;

        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name
        {
            get => name;
            internal set
            {
                if (value != null)
                {
                    name = value;
                }
            }
        }

        /// <summary>
        /// Team name
        /// </summary>
        public string TeamName
        {
            get => teamName;
            internal set
            {
                if (value != null)
                {
                    teamName = value;
                }
            }
        }

        /// <summary>
        /// Score
        /// </summary>
        public long Score { get; internal set; }

        /// <summary>
        /// Level
        /// </summary>
        public uint Level { get; internal set; }

        /// <summary>
        /// Field (internal)
        /// </summary>
        internal Field FieldInternal { get; private set; }

        /// <summary>
        /// Field
        /// </summary>
        public IField Field => FieldInternal;

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="user">User</param>
        internal User(User user)
        {
            Name = user.Name;
            ID = user.ID;
            TeamName = user.TeamName;
            Score = user.Score;
            FieldInternal = new Field(user.FieldInternal);
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
        }
    }
}
