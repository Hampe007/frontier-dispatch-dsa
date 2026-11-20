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
        
        [TestCase(new int[] { 6, 5, 4, 3, 2, 1 })]
        [TestCase(new int[] { 1, 2, 3, 4, 5, 6 })]
        [TestCase(new int[] { 4, 1, 6, 2, 5, 3 })]
        public void DifferentInsertionOrders_DequeueInPriorityOrder(int[] priorities)
        {
            var queue = new MinPriorityQueue<int>();

            foreach (int priority in priorities)
            {
                queue.Enqueue(priority, priority);
            }
            
            var actual = queue.Dequeue();
            Assert.That(actual, Is.EqualTo(1));
        }
    }
}