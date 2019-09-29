using System.Collections.Generic;

/// <summary>
/// TetriEngine namespace
/// </summary>
namespace TetriEngine
{
    /// <summary>
    /// Inventory interface
    /// </summary>
    public interface IInventory
    {
        /// <summary>
        /// Specials
        /// </summary>
        IReadOnlyList<ESpecial> Specials { get; }

        /// <summary>
        /// Special capacity
        /// </summary>
        uint SpecialCapacity { get; }
    }
}
