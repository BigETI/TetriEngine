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
    /// Pattern collection
    /// </summary>
    public class PatternCollection : IPatternCollection
    {
        /// <summary>
        /// Patterns
        /// </summary>
        private HashSet<Regex> patterns = new HashSet<Regex>();

        /// <summary>
        /// Count
        /// </summary>
        public int Count => patterns.Count;

        /// <summary>
        /// Is read only
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns>"true" if added, otherwise "false"</returns>
        public bool Add(Regex item) => patterns.Add(item);

        /// <summary>
        /// Add pattern
        /// </summary>
        /// <param name="pattern">Pattern</param>
        /// <returns>"true" if added, otherwise "false"</returns>
        public bool AddPattern(string pattern)
        {
            bool ret = false;
            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }
            else
            {
                try
                {
                    ret = patterns.Add(new Regex(pattern));
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e);
                }
            }
            return ret;
        }

        /// <summary>
        /// Add patterns
        /// </summary>
        /// <param name="patterns">Patterns</param>
        /// <returns>Number of patterns added</returns>
        public int AddPatterns(IEnumerable<string> patterns)
        {
            int ret = 0;
            if (patterns == null)
            {
                throw new ArgumentNullException(nameof(patterns));
            }
            else
            {
                foreach (string pattern in patterns)
                {
                    if (AddPattern(pattern))
                    {
                        ++ret;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Add regular expressions
        /// </summary>
        /// <param name="regexes">Regular expressions</param>
        /// <returns>Number of regular expresions added</returns>
        public int AddRegexes(IEnumerable<Regex> regexes)
        {
            int ret = 0;
            if (regexes == null)
            {
                throw new ArgumentNullException(nameof(regexes));
            }
            else
            {
                foreach (Regex regex in regexes)
                {
                    if (Add(regex))
                    {
                        ++ret;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Clear
        /// </summary>
        public void Clear() => patterns.Clear();

        /// <summary>
        /// Contains input
        /// </summary>
        /// <param name="input">Input</param>
        /// <returns>"true" if contains input, otherwise "false"</returns>
        public bool Contains(string input)
        {
            bool ret = false;
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }
            else
            {
                foreach (Regex pattern in patterns)
                {
                    ret = pattern.IsMatch(input);
                    if (ret)
                    {
                        break;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Contains inputs
        /// </summary>
        /// <param name="inputs">Inputs</param>
        /// <returns>"true" if contains inputs, otherwise "false"</returns>
        public bool Contains(IEnumerable<string> inputs)
        {
            bool ret = false;
            if (inputs == null)
            {
                throw new ArgumentNullException(nameof(inputs));
            }
            else
            {
                foreach (string input in inputs)
                {
                    ret = Contains(input);
                    if (ret)
                    {
                        break;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Contains
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns>"true" if contains item, otherwise "false"</returns>
        public bool Contains(Regex item) => patterns.Contains(item);

        /// <summary>
        /// Copy to
        /// </summary>
        /// <param name="array">Array</param>
        /// <param name="arrayIndex">Array index</param>
        public void CopyTo(Regex[] array, int arrayIndex) => patterns.CopyTo(array, arrayIndex);

        public void ExceptWith(IEnumerable<Regex> other) => patterns.ExceptWith(other);

        public IEnumerator<Regex> GetEnumerator() => patterns.GetEnumerator();

        /// <summary>
        /// Get matches from input
        /// </summary>
        /// <param name="input">Input</param>
        /// <returns>Matches from input</returns>
        public MatchCollection[] GetMatches(string input)
        {
            List<MatchCollection> match_collections_list = new List<MatchCollection>();
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }
            else
            {
                foreach (Regex pattern in patterns)
                {
                    MatchCollection match_collection = pattern.Matches(input);
                    if (match_collection != null)
                    {
                        match_collections_list.Add(match_collection);
                    }
                }
            }
            MatchCollection[] ret = match_collections_list.ToArray();
            match_collections_list.Clear();
            return ret;
        }

        /// <summary>
        /// Get matches from inputs
        /// </summary>
        /// <param name="inputs">Inputs</param>
        /// <returns>Matches from inputs</returns>
        public Tuple<string, MatchCollection[]>[] GetMatches(IEnumerable<string> inputs)
        {
            List<Tuple<string, MatchCollection[]>> match_collections_list = new List<Tuple<string, MatchCollection[]>>();
            if (inputs == null)
            {
                throw new ArgumentNullException(nameof(inputs));
            }
            else
            {
                foreach (string input in inputs)
                {
                    MatchCollection[] match_collections = GetMatches(input);
                    if (match_collections.Length > 0)
                    {
                        match_collections_list.Add(new Tuple<string, MatchCollection[]>(input, match_collections));
                    }
                }
            }
            Tuple<string, MatchCollection[]>[] ret = match_collections_list.ToArray();
            match_collections_list.Clear();
            return ret;
        }

        /// <summary>
        /// Get object data
        /// </summary>
        /// <param name="info">Information</param>
        /// <param name="context">Context</param>
        public void GetObjectData(SerializationInfo info, StreamingContext context) => patterns.GetObjectData(info, context);

        /// <summary>
        /// Intersect with
        /// </summary>
        /// <param name="other">Other</param>
        public void IntersectWith(IEnumerable<Regex> other) => patterns.IntersectWith(other);

        /// <summary>
        /// Is proper subset of
        /// </summary>
        /// <param name="other">Other</param>
        /// <returns>"true" if it is a proper subset of other, otherwise "false"</returns>
        public bool IsProperSubsetOf(IEnumerable<Regex> other) => patterns.IsProperSubsetOf(other);

        /// <summary>
        /// Is proper superset of
        /// </summary>
        /// <param name="other">Other</param>
        /// <returns>"true" if it is a proper superset of other, otherwise "false"</returns>
        public bool IsProperSupersetOf(IEnumerable<Regex> other) => patterns.IsProperSupersetOf(other);

        /// <summary>
        /// Is subset of
        /// </summary>
        /// <param name="other">Other</param>
        /// <returns>"true" if it is a subset of other, otherwise "false"</returns>
        public bool IsSubsetOf(IEnumerable<Regex> other) => patterns.IsSubsetOf(other);

        /// <summary>
        /// Is superset of
        /// </summary>
        /// <param name="other">Other</param>
        /// <returns>"true" if it is a superset of other, otherwise "false"</returns>
        public bool IsSupersetOf(IEnumerable<Regex> other) => patterns.IsSupersetOf(other);

        /// <summary>
        /// On deserialization
        /// </summary>
        /// <param name="sender">Sender</param>
        public void OnDeserialization(object sender) => patterns.OnDeserialization(sender);

        /// <summary>
        /// Overlaps
        /// </summary>
        /// <param name="other">Other</param>
        /// <returns>"true" if overlaps, otherwise "false"</returns>
        public bool Overlaps(IEnumerable<Regex> other) => patterns.Overlaps(other);

        /// <summary>
        /// Remove item
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns>"true" if item has been removed, otherwise "false"</returns>
        public bool Remove(Regex item) => patterns.Remove(item);

        /// <summary>
        /// Remove pattern
        /// </summary>
        /// <param name="pattern">Pattern</param>
        /// <returns>"true" if pattern was removed, otherwise "false"</returns>
        public bool RemovePattern(string pattern)
        {
            bool ret = false;
            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }
            else
            {
                Regex regex = null;
                try
                {
                    regex = new Regex(pattern);
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e);
                }
                if (regex != null)
                {
                    ret = Remove(regex);
                }
            }
            return ret;
        }

        /// <summary>
        /// Remove patterns
        /// </summary>
        /// <param name="patterns">Patterns</param>
        /// <returns>Number of patterns removed</returns>
        public int RemovePatterns(IEnumerable<string> patterns)
        {
            int ret = 0;
            if (patterns == null)
            {
                throw new ArgumentNullException(nameof(patterns));
            }
            else
            {
                foreach (string pattern in patterns)
                {
                    if (RemovePattern(pattern))
                    {
                        ++ret;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Remove regular expressions
        /// </summary>
        /// <param name="regexes">Regular expression</param>
        /// <returns>Number of regular expressions removed</returns>
        public int RemoveRegexes(IEnumerable<Regex> regexes)
        {
            int ret = 0;
            if (patterns == null)
            {
                throw new ArgumentNullException(nameof(patterns));
            }
            else
            {
                foreach (Regex regex in regexes)
                {
                    if (Remove(regex))
                    {
                        ++ret;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Remove patterns with input
        /// </summary>
        /// <param name="input">Input</param>
        /// <returns>Number of patterns removed with input</returns>
        public int RemoveWithInput(string input)
        {
            int ret = 0;
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }
            else
            {
                List<Regex> remove_regexes = new List<Regex>();
                foreach (Regex pattern in patterns)
                {
                    if (pattern.IsMatch(input))
                    {
                        remove_regexes.Add(pattern);
                    }
                }
                ret = RemoveRegexes(remove_regexes);
                remove_regexes.Clear();
            }
            return ret;
        }

        /// <summary>
        /// Remove patterns with inputs
        /// </summary>
        /// <param name="inputs">Inputs</param>
        /// <returns>Number of patterns removed with inputs</returns>
        public int RemoveWithInputs(IEnumerable<string> inputs)
        {
            int ret = 0;
            if (inputs == null)
            {
                throw new ArgumentNullException(nameof(inputs));
            }
            else
            {
                foreach (string input in inputs)
                {
                    ret += RemoveWithInput(input);
                }
            }
            return ret;
        }

        /// <summary>
        /// Set equals
        /// </summary>
        /// <param name="other">Other</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        public bool SetEquals(IEnumerable<Regex> other) => patterns.SetEquals(other);

        /// <summary>
        /// Symmetric except with
        /// </summary>
        /// <param name="other">Other</param>
        public void SymmetricExceptWith(IEnumerable<Regex> other) => patterns.SymmetricExceptWith(other);

        /// <summary>
        /// Union with
        /// </summary>
        /// <param name="other">Other</param>
        public void UnionWith(IEnumerable<Regex> other) => patterns.UnionWith(other);

        /// <summary>
        /// Add item
        /// </summary>
        /// <param name="item">Item</param>
        void ICollection<Regex>.Add(Regex item) => patterns.Add(item);

        /// <summary>
        /// Get enumerator
        /// </summary>
        /// <returns>Enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator() => patterns.GetEnumerator();
    }
}
