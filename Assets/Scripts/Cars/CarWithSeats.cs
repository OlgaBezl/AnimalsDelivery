using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Scripts.Cars.CarPlaces;
using Scripts.Cars.Seats;
using Scripts.Queues;
using UnityEngine;

namespace Scripts.Cars
{
    public class CarWithSeats : Car, IQueueItem
    {
        [SerializeField] private Transform _seatsContainer;

        [field: SerializeField] public Transform EntryPoint;

        private CarPlace _currentParkingPlace;
        private Transform _exitParkingPoint;
        private List<Seat> _seats;
        private Transform _transform;

        public event Action<IQueueItem> WasMovedInQueue;
        public event Action<CarWithSeats> WasParked;
        public event Action<CarWithSeats> StartLeftParking;
        public event Action<CarWithSeats> LeftParking;

        public CarState State { get; private set; }
        public bool HasFreeSeats =>
            _seats == null ? false : _seats.Any(seat => seat.State == SeatState.Free);
        public int FreeSeatsCount =>
            _seats == null ? Specification.SeatsCount : _seats.Count(seat => seat.State == SeatState.Free);
        public float Length => Specification.Length;

        private void OnValidate()
        {
            if (RotateDuration <= 0)
                throw new ArgumentOutOfRangeException(nameof(RotateDuration));

            if (EntryPoint == null)
                throw new NullReferenceException(nameof(EntryPoint));

            if (_seatsContainer == null)
                throw new NullReferenceException(nameof(_seatsContainer));
        }

        private void OnMouseUp()
        {
            if (State == CarState.Parked && !_seats.Any(seat => seat.State == SeatState.Wait))
            {
                SlowLeaveParking(_exitParkingPoint);
            }
        }

        public override void Initialize(int colorIndex)
        {
            base.Initialize(colorIndex);

            _transform = transform;
            _seats = new List<Seat>();

            foreach (Seat seat in _seatsContainer.GetComponentsInChildren<Seat>())
            {
                _seats.Add(seat);
                seat.Taked += SeatWasTaked;
            }
        }

        public void MoveToParkingPlace(CarPlace carPlace, Transform exitParkingPoint)
        {
            if (carPlace == null)
                throw new NullReferenceException(nameof(carPlace));

            if (exitParkingPoint == null)
                throw new NullReferenceException(nameof(exitParkingPoint));

            State = CarState.GoingToParking;
            _currentParkingPlace = carPlace;
            _exitParkingPoint = exitParkingPoint;
            Vector3 carLengthOffset = carPlace.transform.forward * (Specification.Length - 1f) / 2f;

            Trace.Show();

            Vector3 directionBeforeTurn = carPlace.EntryPoint.position - _transform.position;
            float moveDurationBeforeTurn = Vector3.Magnitude(directionBeforeTurn) / Speed;

            Vector3 directionAfterTurn = carPlace.transform.position - carLengthOffset - carPlace.EntryPoint.position;
            float moveDurationAfterTurn = Vector3.Magnitude(directionAfterTurn) / Speed;

            DOTween.Sequence().
                Append(_transform.DOMove(carPlace.EntryPoint.position, moveDurationBeforeTurn)).
                Append(_transform.DORotateQuaternion(carPlace.transform.rotation, RotateDuration)).
                Append(_transform.DOMove(carPlace.transform.position - carLengthOffset, moveDurationAfterTurn)).
                SetEase(Ease.Linear).
                onComplete += () => Park();
        }

        public void TeleportToParkingPlace(CarPlace carPlace, Transform exitParkingPoint)
        {
            if (carPlace == null)
                throw new NullReferenceException(nameof(carPlace));

            if (exitParkingPoint == null)
                throw new NullReferenceException(nameof(exitParkingPoint));

            State = CarState.GoingToParking;
            _currentParkingPlace = carPlace;
            _exitParkingPoint = exitParkingPoint;
        }

        public void Park()
        {
            State = CarState.Parked;
            Trace.Hide();
            WasParked?.Invoke(this);
        }

        public void SlowLeaveParking(Transform exitParkingPoint)
        {
            State = CarState.LeftParking;
            _currentParkingPlace.FreeUp();
            StartLeftParking?.Invoke(this);

            Vector3 directionBeforeTurn = _currentParkingPlace.ExitPoint.position - _transform.position;
            float moveDurationBeforeTurn = Vector3.Magnitude(directionBeforeTurn) / Speed;

            DOTween.Sequence().
                Append(_transform.DOMove(_currentParkingPlace.ExitPoint.position, moveDurationBeforeTurn)).
                Append(_transform.DORotateQuaternion(Quaternion.identity, RotateDuration)).
                SetEase(Ease.Linear).
                onComplete += () => FastLeaveParking(exitParkingPoint);
        }

        public void FastLeaveParking(Transform exitParkingPoint)
        {
            Trace.Show();
            Vector3 directionAfterTurn = _currentParkingPlace.ExitPoint.position - exitParkingPoint.position;
            float moveDurationAfterTurn = Vector3.Magnitude(directionAfterTurn) / Speed;

            DOTween.Sequence().
                Append(_transform.DOMove(exitParkingPoint.position, moveDurationAfterTurn)).
                SetEase(Ease.Linear).
                onComplete += () => FreeUpParkingPlace();
        }

        public Seat TakeSeat()
        {
            Seat freeSeat = _seats.FirstOrDefault(seat => seat.State == SeatState.Free);
            freeSeat.Wait();

            return freeSeat;
        }

        public void MoveToNextPosition(Vector3 nextPosition)
        {
            float moveDuration = Vector3.Magnitude(transform.position - nextPosition) / Speed;

            DOTween.Sequence().
                Append(transform.DOMove(nextPosition, moveDuration)).
                SetEase(Ease.Linear).
                onComplete += () => WasMovedInQueue?.Invoke(this);
        }

        public void Jump(float delay)
        {
            float jumpHeight = 2.5f;
            Vector3 jumpOffset = new Vector3(0, jumpHeight, 0);
            Vector3 halfJumpOffset = new Vector3(0, jumpHeight / 2f, 0);
            float jumpDuration = jumpHeight / Speed;
            float fallFactor = 0.5f;

            DOTween.Sequence().
                SetDelay(delay).
                Append(_transform.DOMove(_transform.position + jumpOffset, jumpDuration)).
                Append(_transform.DOMove(_transform.position, jumpDuration * fallFactor)).
                Append(_transform.DOMove(_transform.position + halfJumpOffset, jumpDuration)).
                Append(_transform.DOMove(_transform.position, jumpDuration * fallFactor));
        }

        private void FreeUpParkingPlace()
        {
            _currentParkingPlace = null;
            LeftParking?.Invoke(this);
        }

        private void SeatWasTaked(Seat seat)
        {
            seat.Taked += SeatWasTaked;

            if (HasFreeSeats == false)
            {
                JumpAndLeaveParking();
            }
        }

        private void JumpAndLeaveParking()
        {
            State = CarState.LeftParking;
            float jumpHeight = 1.5f;
            Vector3 jumpOffset = new Vector3(0, jumpHeight, 0);
            Vector3 halfJumpOffset = new Vector3(0, jumpHeight / 2f, 0);
            float jumpDuration = jumpHeight / Speed;
            float fallFactor = 0.5f;

            DOTween.Sequence().
                Append(_transform.DOMove(_transform.position + jumpOffset, jumpDuration)).
                Append(_transform.DOMove(_transform.position, jumpDuration * fallFactor)).
                Append(_transform.DOMove(_transform.position + halfJumpOffset, jumpDuration)).
                Append(_transform.DOMove(_transform.position, jumpDuration * fallFactor)).
                onComplete += () => SlowLeaveParking(_exitParkingPoint);
        }
    }
}