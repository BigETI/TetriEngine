/// <summary>
/// TetriEngine namespace
/// </summary>
namespace TetriEngine
{
    /// <summary>
    /// Bot user class
    /// </summary>
    internal class BotUser : IUser
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
        public string TeamName { get; internal set; }

        /// <summary>
        /// Score
        /// </summary>
        public long Score { get; internal set; }

        /// <summary>
        /// Level
        /// </summary>
        public uint Level { get; internal set; }

        /// <summary>
        /// Field internal
        /// </summary>
        internal Field FieldInternal { get; private set; }

        /// <summary>
        /// Field
        /// </summary>
        public IField Field => FieldInternal;

        /// <summary>
        /// Inventory internal
        /// </summary>
        internal Inventory InventoryInternal { get; private set; } = new Inventory();

        /// <summary>
        /// Inventory
        /// </summary>
        public IInventory Inventory => InventoryInternal;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Name</param>
        internal BotUser(int id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}
