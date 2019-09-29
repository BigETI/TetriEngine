using System.Collections.Generic;

/// <summary>
/// TetriEngine networking connection client namespace
/// </summary>
namespace TetriEngine.Networking.Connection.Client
{
    /// <summary>
    /// New game started delegate
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
    public delegate void NewGameStartedDelegate(uint startingHeight, uint startingLevel, uint linesPerLevel, uint levelIncrement, uint linesPerSpecial, uint specialsAdded, uint specialCapacity, IReadOnlyList<EBlock> blockFrequencies, IReadOnlyList<ESpecial> specialFrequencies, bool displayAverageLevels, bool classicMode);
}
