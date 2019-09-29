using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

/// <summary>
/// TetriEngine namespace
/// </summary>
namespace TetriEngine
{
    /// <summary>
    /// Pattern collection interface
    /// </summary>
    public interface IPatternCollection : IReadOnlyPatternCollection, ICollection<Regex>, ISet<Regex>, IDeserializationCallback
    {
        /// <summary>
        /// Add pattern
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns>"true" if pattern was added, otherwise "false"</returns>
        bool AddPattern(string pattern);

        /// <summary>
        /// Add patterns
        /// </summary>
        /// <param name="patterns">Patterns</param>
        /// <returns>Number of patterns added</returns>
        int AddPatterns(IEnumerable<string> patterns);

        /// <summary>
        /// Add regular expressions
        /// </summary>
        /// <param name="regexes">Regular expressions</param>
        /// <returns>Number of regular expressions added</returns>
        int AddRegexes(IEnumerable<Regex> regexes);

        /// <summary>
        /// Remove pattern
        /// </summary>
        /// <param name="pattern">Pattern</param>
        /// <returns>"true" if pattern was removed, otherwise "false"</returns>
        bool RemovePattern(string pattern);

        /// <summary>
        /// Remove patterns
        /// </summary>
        /// <param name="patterns">Patterns</param>
        /// <returns>Number of removed patterns</returns>
        int RemovePatterns(IEnumerable<string> patterns);

        /// <summary>
        /// Remove regular expressions
        /// </summary>
        /// <param name="regexes">Regular expressions</param>
        /// <returns>Number of regular expressions removed</returns>
        int RemoveRegexes(IEnumerable<Regex> regexes);

        /// <summary>
        /// Remove patterns with input
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Number of patterns removed</returns>
        int RemoveWithInput(string input);

        /// <summary>
        /// Remove patterns with inputs
        /// </summary>
        /// <param name="inputs">Inputs</param>
        /// <returns>Number of patterns removed</returns>
        int RemoveWithInputs(IEnumerable<string> inputs);
    }
}
