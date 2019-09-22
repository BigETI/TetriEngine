using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// TetriEngine namespace
/// </summary>
namespace TetriEngine
{
    /// <summary>
    /// Level interface
    /// </summary>
    public interface ILevel
    {
        /// <summary>
        /// Starting height
        /// </summary>
        uint StartingHeight { get; }

        /// <summary>
        /// Starting level
        /// </summary>
        uint StartingLevel { get; }

        /// <summary>
        /// Lines per level
        /// </summary>
        uint LinesPerLevel { get; }

        /// <summary>
        /// Level increment
        /// </summary>
        uint LevelIncrement { get; }

        /// <summary>
        /// Lines per special
        /// </summary>
        uint LinesPerSpecial { get; }

        /// <summary>
        /// Specials added
        /// </summary>
        uint SpecialsAdded { get; }

        /// <summary>
        /// Special capacity
        /// </summary>
        uint SpecialCapacity { get; }

        /// <summary>
        /// Block frequencies
        /// </summary>
        IReadOnlyList<EBlock> BlockFrequencies { get; }

        /// <summary>
        /// Special frequencies
        /// </summary>
        IReadOnlyList<ESpecial> SpecialFrequencies { get; }

        /// <summary>
        /// Display average levels
        /// </summary>
        bool DisplayAverageLevels { get; }

        /// <summary>
        /// Classic mode
        /// </summary>
        bool ClassicMode { get; }
    }
}
