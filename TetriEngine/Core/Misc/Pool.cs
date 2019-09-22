using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// TetriEngine namespace
/// </summary>
namespace TetriEngine
{
    /// <summary>
    /// Pool class
    /// </summary>
    /// <typeparam name="T">Element type</typeparam>
    public class Pool<T> : IPool<T> where T : class
    {
        /// <summary>
        /// Entries
        /// </summary>
        private List<T> entries = default;

        /// <summary>
        /// Lowest available index
        /// </summary>
        private uint lowestAvailableIndex;

        /// <summary>
        /// Invalid ID
        /// </summary>
        public int InvalidID { get; private set; }

        /// <summary>
        /// Maximal amount of entries
        /// </summary>
        public uint MaxEntries { get; private set; }

        /// <summary>
        /// Count
        /// </summary>
        public int Count { get; private set; } = 0;

        /// <summary>
        /// Next available ID
        /// </summary>
        public int NextAvailableID => ((lowestAvailableIndex < MaxEntries) ? GetIDByIndex(lowestAvailableIndex) : InvalidID);

        /// <summary>
        /// Default constructor
        /// </summary>
        public Pool()
        {
            InvalidID = -1;
            MaxEntries = uint.MaxValue;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="invalidID"></param>
        public Pool(int invalidID)
        {
            InvalidID = invalidID;
            MaxEntries = uint.MaxValue;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="invalidID">Invalid ID</param>
        /// <param name="maxEntries">Maximal amount of entries</param>
        public Pool(int invalidID, uint maxEntries)
        {
            InvalidID = invalidID;
            MaxEntries = maxEntries;
        }

        /// <summary>
        /// Get ID by index
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns></returns>
        internal int GetIDByIndex(uint index) => (int)((InvalidID < 0) ? index : ((index >= InvalidID) ? (index + 1) : index));

        /// <summary>
        /// Get index by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal uint GetIndexByID(int id)
        {
            if ((id == InvalidID) || (id < 0))
            {
                throw new InvalidIDException(id);
            }
            return (uint)((InvalidID < 0) ? id : ((id > InvalidID) ? (id - 1) : id));
        }

        /// <summary>
        /// Add entry
        /// </summary>
        /// <param name="entry">Entry</param>
        /// <returns>Valid ID if successful otherwise an invalid ID</returns>
        public int Add(T entry)
        {
            int ret = InvalidID;
            if ((entry != null) && (lowestAvailableIndex < MaxEntries))
            {
                ret = GetIDByIndex(lowestAvailableIndex);
                if (lowestAvailableIndex < entries.Count)
                {
                    entries[(int)lowestAvailableIndex] = entry;
                }
                else
                {
                    entries.Add(entry);
                }
                ++lowestAvailableIndex;
                for (int i = (int)lowestAvailableIndex; i < entries.Count; i++)
                {
                    if (entries[i] != null)
                    {
                        ++lowestAvailableIndex;
                    }
                }
                ++Count;
            }
            return ret;
        }

        /// <summary>
        /// Insert entry
        /// </summary>
        /// <param name="entry">Entry</param>
        /// <param name="id">ID</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        public bool Insert(T entry, int id)
        {
            bool ret = false;
            if ((entry != null) && (id != InvalidID) && (id >= 0))
            {
                uint index = GetIndexByID(id);
                if (index < MaxEntries)
                {
                    while (index >= entries.Count)
                    {
                        entries.Add(null);
                    }
                    if (entries[(int)index] == null)
                    {
                        entries[(int)index] = entry;
                        if (index <= lowestAvailableIndex)
                        {
                            lowestAvailableIndex = index + 1U;
                            for (int i = (int)lowestAvailableIndex; i < entries.Count; i++)
                            {
                                if (entries[i] != null)
                                {
                                    ++lowestAvailableIndex;
                                }
                            }
                        }
                        ret = true;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Remove entry by ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        public bool Remove(int id)
        {
            bool ret = IsIDValid(id);
            if (ret)
            {
                uint index = GetIndexByID(id);
                entries[(int)index] = null;
                if (lowestAvailableIndex > index)
                {
                    lowestAvailableIndex = index;
                }
                while (entries.Count > 0)
                {
                    int last_index = entries.Count - 1;
                    if (entries[last_index] == null)
                    {
                        entries.RemoveAt(last_index);
                    }
                    else
                    {
                        break;
                    }
                }
                --Count;
            }
            return ret;
        }

        /// <summary>
        /// Is ID valid
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>"true" if valid, otherwise "false"</returns>
        public bool IsIDValid(int id)
        {
            bool ret = false;
            if ((id != InvalidID) && (id >= 0))
            {
                uint index = GetIndexByID(id);
                if (index < entries.Count)
                {
                    ret = (entries[(int)index] != null);
                }
            }
            return ret;
        }

        /// <summary>
        /// Get enumerator
        /// </summary>
        /// <returns>Enumerator</returns>
        public IEnumerator<T> GetEnumerator()
        {
            // TODO
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get enumerator
        /// </summary>
        /// <returns>Enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Array access operator
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>Element</returns>
        public T this[int id]
        {
            get
            {
                uint index = GetIndexByID(id);
                if (index >= entries.Count)
                {
                    throw new InvalidIDException(id);
                }
                T ret = entries[(int)index];
                if (ret == null)
                {
                    throw new InvalidIDException(id);
                }
                return ret;
            }
        }
    }
}
