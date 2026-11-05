using System;
using NUnit.Framework;
using FrontierDispatch.Core;

namespace FrontierDispatch.Tests
{
    public class MinPriorityQueueTests
    {
        [Test]
        public void EmptyQueue_PeekAndDequeueThrow()
        {
            var queue = new MinPriorityQueue<int>();

            Assert.Throws<InvalidOperationException>(() => queue.Peek());
            Assert.Throws<InvalidOperationException>(() => queue.Dequeue());
            Assert.That(queue.Count, Is.Zero);
        }
        
        [Test]
        public void SingleItem_CanBeRemovedAndQueueReused()
        {
            var queue = new MinPriorityQueue<string>();

            for (int i = 0; i < 3; i++)
            {
                queue.Enqueue("item", i);

                Assert.That(queue.Count, Is.EqualTo(1));
                Assert.That(queue.Peek(), Is.EqualTo("item"));
                Assert.That(queue.Count, Is.EqualTo(1));

                Assert.That(queue.Dequeue(), Is.EqualTo("item"));
                Assert.That(queue.Count, Is.Zero);
            }
        }
        
        [TestCase(new int[] { 1, 2, 3, 4, 5, 6 })]
        [TestCase(new int[] { 6, 5, 4, 3, 2, 1 })]
        [TestCase(new int[] { 4, 1, 6, 2, 5, 3 })]
        public void DifferentInsertionOrders_DequeueInPriorityOrder(int[] priorities)
        {
            var queue = new MinPriorityQueue<int>();

            foreach (int priority in priorities)
            {
                queue.Enqueue(priority, priority);
            }

            for (int expected = 1; expected <= priorities.Length; expected++)
            {
                Assert.That(queue.Dequeue(), Is.EqualTo(expected));
                Assert.That(queue.Count, Is.EqualTo(priorities.Length - expected));
            }
        }
        
        [Test]
        public void MixedOperations_PreserveMinimumAndDuplicateItems()
        {
            var queue = new MinPriorityQueue<string>();

            queue.Enqueue("A", 2);
            queue.Enqueue("B", 2);
            queue.Enqueue("C", 9);
            queue.Enqueue("D", -1);

            Assert.That(queue.Dequeue(), Is.EqualTo("D"));

            queue.Enqueue("next", 0);

            Assert.That(queue.Peek(), Is.EqualTo("next"));
            Assert.That(queue.Peek(), Is.EqualTo("next"));
            Assert.That(queue.Count, Is.EqualTo(4));
            Assert.That(queue.Dequeue(), Is.EqualTo("next"));

            var itemsWithEqualPriority = new[] { queue.Dequeue(), queue.Dequeue() };

            Assert.That(itemsWithEqualPriority, Is.EquivalentTo(new[] { "A", "B" }));
            Assert.That(queue.Dequeue(), Is.EqualTo("C"));
            Assert.That(queue.Count, Is.Zero);
        }
    }
}