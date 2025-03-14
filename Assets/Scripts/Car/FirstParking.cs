using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FirstParking : MonoBehaviour
{
    [SerializeField] private CarMatrix _carMatrix;
    [SerializeField] private CarQueue _carQueue;
    [SerializeField] private Cloud _cloud;

    private List<ArrowCar> _cars;
    private CarList _carList;

    private void OnValidate()
    {
        if (_carMatrix == null)
            throw new NullReferenceException(nameof(_carMatrix));

        if (_carQueue == null)
            throw new NullReferenceException(nameof(_carQueue));

        if (_cloud == null)
            throw new NullReferenceException(nameof(_cloud));
    }

    public void StartLevel(CarList carList)
    {
        _cars = new List<ArrowCar>();
        _carList = carList;
    }

    public void FinishLevel()
    {
        _cars = null;
    }

    public void AddCar(ArrowCar car)
    {
        if(car == null) 
            throw new ArgumentNullException(nameof(car));

        _cars.Add(car);
        car.LeaveParkingStart += CarStartLeaveParking;
        car.LeaveParkingEnd += CarFinishLeaveParking;
    }
    private void CarStartLeaveParking(ArrowCar car)
    {
        car.LeaveParkingStart -= CarStartLeaveParking;
        car.Model.StartLeaveFirstParking();

        _cars.Remove(car);
        _carMatrix.ClearCarPlace(car);
        UpdateGrayMode();
    }

    private void CarFinishLeaveParking(ArrowCar car)
    {
        car.LeaveParkingEnd -= CarFinishLeaveParking;
        car.Model.LeaveFirstParking();
        _carList.FindHintCarAndInvokeAction();

        Destroy(car.gameObject);
        Instantiate(_cloud, car.transform.position, car.transform.rotation);

        _carQueue.QueueUpCar(car.Model);
    }
    
    //public List<Tuple<int, int>> GetColorOuterCars()
    //{
    //    return _cars.Where(car => car.CanLeftParking).Select(car => new Tuple<int, int>(car.ColorIndex, car.Type.SeatsCount)).ToList();
    //}

    //public List<Tuple<int, int>> GetColorInnerCars()
    //{
    //    return _cars.Where(car => !car.CanLeftParking).Select(car => new Tuple<int, int>(car.ColorIndex, car.Type.SeatsCount)).ToList();
    //}

    public void UpdateGrayMode()
    {
        foreach (ArrowCar car in _cars)
        {
            if (_carMatrix.CanLeaveParking(car))
            {
                car.GrayModeOff();
            }
            else
            {
                car.GrayModeOn();
            }
        }
    }
}
