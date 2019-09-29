using System.Collections.Generic;

/// <summary>
/// TetriEngine namespace
/// </summary>
namespace TetriEngine
{
    /// <summary>
    /// Game options class
    /// </summary>
    public class GameOptions
    {
        /// <summary>
        /// Starting height
        /// </summary>
        public uint StartingHeight { get; private set; }

        /// <summary>
        /// Starting level
        /// </summary>
        public uint StartingLevel { get; private set; }

        /// <summary>
        /// Lines per level
        /// </summary>
        public uint LinesPerLevel { get; private set; }

        /// <summary>
        /// Level increment
        /// </summary>
        public uint LevelIncrement { get; private set; }

        /// <summary>
        /// Lines per special
        /// </summary>
        public uint LinesPerSpecial { get; private set; }

        /// <summary>
        /// Specials added
        /// </summary>
        public uint SpecialsAdded { get; private set; }

        /// <summary>
        /// Special capacity
        /// </summary>
        public uint SpecialCapacity { get; private set; }

        /// <summary>
        /// Block frequencies
        /// </summary>
        public IReadOnlyList<EBlock> BlockFrequencies { get; private set; }

        /// <summary>
        /// Special frequencies
        /// </summary>
        public IReadOnlyList<ESpecial> SpecialFrequencies { get; private set; }

        /// <summary>
        /// Display average levels
        /// </summary>
        public bool DisplayAverageLevels { get; private set; }

        /// <summary>
        /// Classic mode
        /// </summary>
        public bool ClassicMode { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="startingHeight">Starting height</param>
        /// <param name="startingLevel">Starting level</param>
        /// <param name="linesPerLevel">Lines per level</param>
        /// <param name="levelIncrement">Level increment</param>
        /// <param name="linesPerSpecial">Lines per special</param>
        /// <param name="specialsAdded">Specials added</param>
        /// <param name="specialCapacity">Special capacity</param>
        /// <param name="blockFrequencies">Block frequencies</param>
        /// <param name="specialFrequencies">Special frequencies</param>
        /// <param name="displayAverageLevels">Display average levels</param>
        /// <param name="classicMode">Classic mode</param>
        internal GameOptions(uint startingHeight, uint startingLevel, uint linesPerLevel, uint levelIncrement, uint linesPerSpecial, uint specialsAdded, uint specialCapacity, IReadOnlyList<EBlock> blockFrequencies, IReadOnlyList<ESpecial> specialFrequencies, bool displayAverageLevels, bool classicMode)
        {
            StartingHeight = startingHeight;
            StartingLevel = startingLevel;
            LinesPerLevel = linesPerLevel;
            LevelIncrement = levelIncrement;
            LinesPerSpecial = linesPerSpecial;
            SpecialsAdded = specialsAdded;
            SpecialCapacity = specialCapacity;
            BlockFrequencies = blockFrequencies;
            SpecialFrequencies = specialFrequencies;
            DisplayAverageLevels = displayAverageLevels;
            ClassicMode = classicMode;
        }
    }
}
