using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class CarWithSeats : Car, IQueueItem
{
    [SerializeField] private Transform _seatsContainer;

    [field: SerializeField] public Transform EntryPoint;

    private CarPlace _currentParkingPlace;

    private Transform _exitParkingPoint;
    private List<Seat> _seats;
    private Transform _transform;
    public CarWithSeatsState State { get; private set; }

    public event Action<IQueueItem> WasMovedInQueue;
    public event Action<CarWithSeats> WasParked;
    public event Action<CarWithSeats> StartLeftParking;
    public event Action<CarWithSeats> LeftParking;

    public bool HasFreeSeats => _seats == null ? false : _seats.Any(seat => seat.State == SeatState.Free);
    public int FreeSeatsCount => _seats == null ? Type.SeatsCount : _seats.Count(seat => seat.State == SeatState.Free);
    //public int WaitSeatsCount => _seats == null ? Type.SeatsCount : _seats.Count(seat => seat.State == SeatState.Wait);

    public float Length => Type.Length;

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
        if (State == CarWithSeatsState.Parked && !_seats.Any(seat => seat.State == SeatState.Wait))
        {
            SlowLeaveParking(_exitParkingPoint);
        }
    }

    public override void Initialize(int colorIndex)
    {
        base.Initialize(colorIndex);

        _transform = transform;
        _seats = new List<Seat>();

        foreach(Seat seat in _seatsContainer.GetComponentsInChildren<Seat>())
        {
            _seats.Add(seat);
            seat.Taked += SeatWasTaked;
        }
    }

    public void MoveToParkingPlace(CarPlace carPlace, Transform exitParkingPoint)
    {
        if(carPlace == null)
            throw new NullReferenceException(nameof(carPlace));

        if(exitParkingPoint == null)
            throw new NullReferenceException(nameof(exitParkingPoint));

        State = CarWithSeatsState.GoingToParking;
        _currentParkingPlace = carPlace;
        _exitParkingPoint = exitParkingPoint;
        Vector3 carLengthOffset = carPlace.transform.forward * (Type.Length - 1f) / 2f;

        Trace.Show();

        float moveDurationBeforeTurn = Vector3.Magnitude(carPlace.EntryPoint.position - _transform.position) / Speed;
        float moveDurationAfterTurn = Vector3.Magnitude(carPlace.transform.position - carLengthOffset - carPlace.EntryPoint.position) / Speed;

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

        State = CarWithSeatsState.GoingToParking;
        _currentParkingPlace = carPlace;
        _exitParkingPoint = exitParkingPoint;
    }

    public void Park()
    {
        State = CarWithSeatsState.Parked;
        Trace.Hide();
        WasParked?.Invoke(this);
    }

    public void SlowLeaveParking(Transform exitParkingPoint)
    {
        State = CarWithSeatsState.LeftParking; 
        _currentParkingPlace.FreeUp();
        StartLeftParking?.Invoke(this); 

        float moveDurationBeforeTurn = Vector3.Magnitude(_currentParkingPlace.ExitPoint.position - _transform.position) / Speed;
        //Debug.Log($"carPlace {carPlace}");

        DOTween.Sequence().
            Append(_transform.DOMove(_currentParkingPlace.ExitPoint.position, moveDurationBeforeTurn)).
            Append(_transform.DORotateQuaternion(Quaternion.identity, RotateDuration)).
            SetEase(Ease.Linear).
            onComplete += () => FastLeaveParking(exitParkingPoint);
    }
    public void FastLeaveParking(Transform exitParkingPoint)
    {
        Trace.Show();
        float moveDurationAfterTurn = Vector3.Magnitude(_currentParkingPlace.ExitPoint.position - exitParkingPoint.position) / Speed;

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

    private void FreeUpParkingPlace()
    {
        //_currentParkingPlace.FreeUp();
        _currentParkingPlace = null;
        LeftParking?.Invoke(this);

    }

    public void MoveToNextPosition(Vector3 nextPosition)
    {
        float moveDuration = Vector3.Magnitude(transform.position - nextPosition) / Speed;

        DOTween.Sequence().
            Append(transform.DOMove(nextPosition, moveDuration)).
            SetEase(Ease.Linear).
            onComplete += () => WasMovedInQueue?.Invoke(this);
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
        State = CarWithSeatsState.LeftParking;
        float jumpHeight = 1.5f;
        Vector3 jumpOffset = new Vector3(0, jumpHeight, 0);
        Vector3 halfJumpOffset = new Vector3(0, jumpHeight/2f, 0);
        float jumpDuration = jumpHeight / Speed;
        float fallFactor = 0.5f;

        DOTween.Sequence().
            Append(_transform.DOMove(_transform.position + jumpOffset, jumpDuration)).
            Append(_transform.DOMove(_transform.position, jumpDuration * fallFactor)).
            Append(_transform.DOMove(_transform.position + halfJumpOffset, jumpDuration)).
            Append(_transform.DOMove(_transform.position, jumpDuration * fallFactor)).
            onComplete += () => SlowLeaveParking(_exitParkingPoint);
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
}
