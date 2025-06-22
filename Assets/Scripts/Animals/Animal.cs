using System;
using DG.Tweening;
using Scripts.Cars.Seats;
using Scripts.Enviroment;
using Scripts.Helpers;
using Scripts.Queues;
using UnityEngine;

namespace Scripts.Animals
{
    public class Animal : MonoBehaviour, IQueueItem
    {
        [SerializeField] private ObjectPainter _objectPainter;
        [SerializeField] private float _rotateDuration = 0.25f;
        [SerializeField] private Animator _animator;

        [field: SerializeField] public float Speed { get; private set; } = 10f;
        [field: SerializeField] public int ColorIndex { get; private set; }
        [field: SerializeField] public float Length { get; private set; } = 0.35f;

        private string _danceAnimationName = "Joy";
        private string _despondAnimationName = "Sad";

        public event Action<IQueueItem> WasMovedInQueue;

        private void OnValidate()
        {
            if (_rotateDuration <= 0)
                throw new ArgumentOutOfRangeException(nameof(_rotateDuration));

            if (_objectPainter == null)
                throw new NullReferenceException(nameof(_animator));

            if (_animator == null)
                throw new NullReferenceException(nameof(_objectPainter));
        }

        public void Initialize(int colorIndex)
        {
            Initialize(colorIndex, true);
        }

        public void Initialize(int colorIndex, bool selfColor)
        {
            int index = selfColor ? ColorIndex : colorIndex;
            _objectPainter.PaintByIndex(index, ColorPallet.GrayIndex);
        }

        public void MoveToSeat(Seat seat, Transform entryPoint)
        {
            if (seat == null)
                throw new NullReferenceException(nameof(seat));

            Vector3 seatPosition = seat.transform.position;
            float moveDurationToEntryPoint = Vector3.Magnitude(seat.EntryPoint.position - transform.position) / Speed;
            float moveDurationFromEntryPoint = Vector3.Magnitude(seatPosition - seat.EntryPoint.position) / Speed;

            DOTween.Sequence().
                SetEase(Ease.Linear).
                Append(transform.DORotate(entryPoint.rotation.eulerAngles, _rotateDuration)).
                Append(transform.DOMove(entryPoint.position, moveDurationToEntryPoint)).
                Append(transform.DOMove(seatPosition, moveDurationFromEntryPoint)).
                Append(transform.DORotate(seat.transform.rotation.eulerAngles, _rotateDuration)).
                SetEase(Ease.Linear).
                onComplete += () => seat.Take();
        }

        public void MoveToNextPosition(Vector3 nextPosition)
        {
            float moveDuration = Vector3.Magnitude(transform.position - nextPosition) / Speed;

            DOTween.Sequence().
                Append(transform.DOMove(nextPosition, moveDuration)).
                SetEase(Ease.Linear).
                onComplete += () =>
                {
                    if (this != null && gameObject != null)
                    {
                        WasMovedInQueue?.Invoke(this);
                    }
                };
        }

        public void Dance()
        {
            _animator.Play(_danceAnimationName);
        }

        public void Despond()
        {
            _animator.Play(_despondAnimationName);
        }
    }
}