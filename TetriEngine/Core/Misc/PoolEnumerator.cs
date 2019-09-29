using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// TetriEngine namespace
/// </summary>
namespace TetriEngine
{
    /// <summary>
    /// Pool enumerator
    /// </summary>
    /// <typeparam name="T">Entry type</typeparam>
    public class PoolEnumerator<T> : IEnumerator<T> where T : class
    {
        /// <summary>
        /// Pool
        /// </summary>
        private Pool<T> pool;

        /// <summary>
        /// Current index
        /// </summary>
        private uint currentIndex = 0U;

        /// <summary>
        /// Current
        /// </summary>
        public T Current => pool[CurrentID];

        /// <summary>
        /// Current ID
        /// </summary>
        public int CurrentID => pool.GetIDByIndex(currentIndex);

        /// <summary>
        /// Current
        /// </summary>
        object IEnumerator.Current => throw new NotImplementedException();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pool">Pool</param>
        internal PoolEnumerator(Pool<T> pool)
        {
            this.pool = pool;
        }

        /// <summary>
        /// Advance to valid entry
        /// </summary>
        /// <returns>"true" if successful, otherwise "false"</returns>
        private bool AdvanceToValidEntry()
        {
            bool ret = false;
            while (currentIndex < pool.MaxEntries)
            {
                ret = pool.IsIDValid(pool.GetIDByIndex(currentIndex));
                if (ret)
                {
                    break;
                }
                else
                {
                    ++currentIndex;
                }
            }
            return false;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            // ...
        }

        /// <summary>
        /// Move next
        /// </summary>
        /// <returns>"true" if successful, otherwise "false"</returns>
        public bool MoveNext()
        {
            ++currentIndex;
            return AdvanceToValidEntry();
        }

        /// <summary>
        /// Reset enumerator
        /// </summary>
        public void Reset()
        {
            currentIndex = 0U;
            AdvanceToValidEntry();
        }
    }
}
