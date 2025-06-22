using System;
using UnityEngine;

namespace Scripts.Cars.Seats
{
    public class Seat : MonoBehaviour
    {
        public event Action<Seat> Taked;

        [field: SerializeField] public Transform EntryPoint { get; private set; }

        public SeatState State { get; private set; } = SeatState.Free;

        public void Wait()
        {
            State = SeatState.Wait;
        }

        public void Take()
        {
            State = SeatState.Taken;
            Taked?.Invoke(this);
        }
    }
}