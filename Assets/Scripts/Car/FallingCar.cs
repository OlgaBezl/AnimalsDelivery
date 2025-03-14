using System;
using UnityEngine;

public class FallingCar : MonoBehaviour
{
    [field: SerializeField] public CarWithSeats CarWithSeats {get; private set;}

    private void OnValidate()
    {
        if (CarWithSeats == null)
            throw new NullReferenceException(nameof(CarWithSeats));
    }

    public void Park()
    {
        CarWithSeats.Park();
    }

}
