using System;
using UnityEngine;

public interface IQueueItem
{
    public float Length { get; }
    public void Initialize(int colorIndex);
    public void MoveToNextPosition(Vector3 nextPosition);
    public event Action<IQueueItem> WasMovedInQueue;
}
