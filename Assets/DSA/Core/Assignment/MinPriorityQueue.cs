using System;
using System.Collections.Generic;

namespace FrontierDispatch.Core
{
    // Assignment Phase 3. Implement as a binary min-heap.

    /// <summary>
    /// A minimum-priority queue implemented as a binary min-heap.
    ///
    /// Items with lower integer priority values should be returned before
    /// items with higher priority values.
    /// </summary>
    public sealed class MinPriorityQueue<T>
    {
        // The list stores the physical representation of the binary heap.
        // Your implementation is responsible for preserving the min-heap invariant.
        private readonly List<Entry> _heap = new List<Entry>();

        /// <summary>
        /// Number of items currently stored in the queue.
        /// </summary>
        public int Count => _heap.Count;

        /// <summary>
        /// Adds an item with the supplied priority.
        /// Lower priority values are removed before higher priority values.
        /// </summary>
        public void Enqueue(T item, int priority)
        {
            _heap.Add(new Entry(item, priority));
            int index = _heap.Count - 1;

            while (index > 0)
            {
                int parent = (index - 1) / 2;

                if (_heap[parent].Priority <= _heap[index].Priority)
                {
                    break;
                }
                
                Entry temp = _heap[parent]; 
                _heap[parent] = _heap[index];
                _heap[index] = temp;
                
                index = parent;
            }
        }

        /// <summary>
        /// Returns the minimum-priority item without removing it.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the queue is empty.
        /// </exception>
        public T Peek()
        {
            if (_heap.Count == 0)
            {
                throw new InvalidOperationException("The queue is empty.");
            }

            return _heap[0].Item;
        }

        /// <summary>
        /// Removes and returns the minimum-priority item.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the queue is empty.
        /// </exception>
        public T Dequeue()
        {
            if (_heap.Count == 0)
            {
                throw new InvalidOperationException("The queue is empty.");
            }
            
            T item = _heap[0].Item;
            int lastIndex = _heap.Count - 1;
            _heap[0] = _heap[lastIndex];
            _heap.RemoveAt(lastIndex);
            int index = 0;

            while (true)
            {
                int left = 2 * index + 1;
                int right = left + 1;

                if (left >= _heap.Count)
                {
                    break;
                }

                int smallerChild = left;

                if (right < _heap.Count && _heap[right].Priority < _heap[left].Priority)
                {
                    smallerChild = right;
                }

                if (_heap[index].Priority <= _heap[smallerChild].Priority)
                {
                    break;
                }

                Entry temp = _heap[index];
                _heap[index] = _heap[smallerChild];
                _heap[smallerChild] = temp;

                index = smallerChild;
            }
            
            return item;
        }

        /// <summary>
        /// Stores one item together with the priority used by the heap.
        /// </summary>
        private readonly struct Entry
        {
            public Entry(T item, int priority)
            {
                Item = item;
                Priority = priority;
            }

            public T Item { get; }
            public int Priority { get; }
        }
    }
}
