using System.Collections.Generic;

namespace TakeMeThereXamarinForms.Models
{
    internal class FixSizedQueue<T> : Queue<T>
    {
        public Queue<T> Queue { get; private set; }

        private int _size = 10;

        public FixSizedQueue(int size)
        {
            this.Queue = new Queue<T>();
            this._size = size;
        }

        public new void Enqueue(T item)
        {
            while (this.Queue.Count > this._size)
                this.Queue.Dequeue();
            this.Queue.Enqueue(item);
        }
    }
}