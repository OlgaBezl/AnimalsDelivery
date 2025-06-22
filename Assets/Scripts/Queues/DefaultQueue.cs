using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts.Helpers;
using UnityEngine;

namespace Scripts.Queues
{
    public abstract class DefaultQueue<T> : MonoBehaviour
        where T : MonoBehaviour, IQueueItem
    {
        [SerializeField] public Transform _startPoint;
        [SerializeField] public Transform _endPoint;
        [SerializeField] private Transform _container;
        [SerializeField] protected float _additionalZOffset = 0.2f;

        protected Queue<T> Queue;
        protected bool QueueIsMoving;

        private void OnValidate()
        {
            if (_container == null)
                throw new NullReferenceException(nameof(_container));

            if (_startPoint == null)
                throw new NullReferenceException(nameof(_startPoint));

            if (_endPoint == null)
                throw new NullReferenceException(nameof(_endPoint));
        }

        public virtual void StartLevel()
        {
            gameObject.SetActive(true);
            Queue = new Queue<T>();
        }

        public void Unload()
        {
            for (int i = 0; i < _container.childCount; i++)
            {
                Destroy(_container.GetChild(i).gameObject);
            }

            gameObject.SetActive(false);
            Queue = null;
        }

        public T Spawn(T prefab, int colorIndex, Vector3 offset)
        {
            if (prefab == null)
                throw new ArgumentNullException(nameof(prefab));

            if (colorIndex < 0 || colorIndex > ColorPallet.ColorsCountWithoutGray)
                throw new ArgumentOutOfRangeException(nameof(colorIndex));

            T item = Instantiate(prefab, _startPoint.position + offset, _startPoint.rotation, _container);
            item.Initialize(colorIndex);
            Queue.Enqueue(item);

            return item;
        }

        public void MoveQueue(float zOffset = 0)
        {
            QueueIsMoving = true;

            Queue.Dequeue();

            if (Queue.Count == 0)
            {
                QueueIsMoving = false;
                return;
            }

            foreach (T item in Queue)
            {
                item.MoveToNextPosition(item.transform.position + new Vector3(0, 0, zOffset));
            }

            T lastItem = Queue.Last();
            lastItem.WasMovedInQueue += ContinueMoveQueue;
        }

        public abstract void FirstAction();

        private void ContinueMoveQueue(IQueueItem item)
        {
            QueueIsMoving = false;
            item.WasMovedInQueue -= ContinueMoveQueue;
            FirstAction();
        }
    }
}