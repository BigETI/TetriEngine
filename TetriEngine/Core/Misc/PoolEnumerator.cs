using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TetriEngine
{
    class PoolEnumerator<T> : IEnumerator<T> where T : class
    {
        private Pool<T> pool;

        private uint currentIndex = 0U;

        internal PoolEnumerator(Pool<T> pool)
        {
            this.pool = pool;
        }

        public T Current => pool[CurrentID];

        public int CurrentID => pool.GetIDByIndex(currentIndex);

        object IEnumerator.Current => throw new NotImplementedException();

        private bool AdvanceToValidEntry()
        {
            // TODO
            return false;
        }

        public void Dispose()
        {
            // ...
        }

        public bool MoveNext()
        {
            ++currentIndex;
            return AdvanceToValidEntry();
        }

        public void Reset()
        {
            currentIndex = 0U;
            AdvanceToValidEntry();
        }
    }
}
