using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

/// <summary>
/// TetriEngine namespace
/// </summary>
namespace TetriEngine
{
    /// <summary>
    /// Read only pattern collection interface
    /// </summary>
    public interface IReadOnlyPatternCollection : IEnumerable<Regex>, IEnumerable, IReadOnlyCollection<Regex>, ISerializable
    {
        /// <summary>
        /// Get matches from input
        /// </summary>
        /// <param name="input">Input</param>
        /// <returns>Matches</returns>
        MatchCollection[] GetMatches(string input);

        /// <summary>
        /// Get matches from inputs
        /// </summary>
        /// <param name="inputs">Inputs</param>
        /// <returns>Matches</returns>
        Tuple<string, MatchCollection[]>[] GetMatches(IEnumerable<string> inputs);

        /// <summary>
        /// Contains input
        /// </summary>
        /// <param name="input">Input</param>
        /// <returns>"true" if contains input, otherwise "false"</returns>
        bool Contains(string input);

        /// <summary>
        /// Contains inputs
        /// </summary>
        /// <param name="inputs">Inputs</param>
        /// <returns>"true" if contains inputs, otherwise "false"</returns>
        bool Contains(IEnumerable<string> inputs);
    }
}
