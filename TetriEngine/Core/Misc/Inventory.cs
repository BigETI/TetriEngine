using System.Collections.Generic;

/// <summary>
/// TetriEngine namespace
/// </summary>
namespace TetriEngine
{
    /// <summary>
    /// Inventory class
    /// </summary>
    internal class Inventory : IInventory
    {
        /// <summary>
        /// Special capacity
        /// </summary>
        private uint specialCapacity = 0U;

        /// <summary>
        /// Specials
        /// </summary>
        private List<ESpecial> specials;

        /// <summary>
        /// Specials
        /// </summary>
        public IReadOnlyList<ESpecial> Specials => specials;

        /// <summary>
        /// Special capacity
        /// </summary>
        public uint SpecialCapacity
        {
            get => specialCapacity;
            set
            {
                specialCapacity = value;
                if (specialCapacity > specials.Count)
                {
                    specials.RemoveRange((int)specialCapacity, specials.Count - (int)specialCapacity);
                }
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        internal Inventory()
        {
            // ...
        }

        /// <summary>
        /// Add special
        /// </summary>
        /// <param name="special">Special</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        internal bool AddSpecial(ESpecial special)
        {
            bool ret = false;
            if ((special != ESpecial.Nothing) && (specials.Count < SpecialCapacity))
            {
                specials.Add(special);
            }
            return ret;
        }

        /// <summary>
        /// Use special
        /// </summary>
        /// <param name="special">Special</param>
        /// <returns>"true" if special is used, otherwise "false"</returns>
        internal bool UseSpecial(ESpecial special)
        {
            bool ret = false;
            if (special != ESpecial.Nothing)
            {
                for (int i = 0; i < specials.Count; i++)
                {
                    if (specials[i] == special)
                    {
                        specials.RemoveAt(i);
                        ret = true;
                        break;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Clear
        /// </summary>
        internal void Clear()
        {
            specials.Clear();
        }
    }
}
