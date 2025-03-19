using System;
using UnityEngine;

namespace Scripts.Queues
{
    public interface IQueueItem
    {
        public event Action<IQueueItem> WasMovedInQueue;

        public float Length { get; }

        public void Initialize(int colorIndex);
        public void MoveToNextPosition(Vector3 nextPosition);
    }
}