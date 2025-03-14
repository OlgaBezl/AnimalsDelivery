using System;
using System.Collections.Generic;
using System.Linq;

public class CarList
{
    private List<CarModel> _cars;

    public event Action NeedShowHint;
    public event Action LevelFinished;

    private int _maxParkingPlaces = 4;

    public void Initialize()
    {
        _cars = new List<CarModel>();
    }

    public void Clear()
    {
        _cars = null;
    }

    public CarModel AddCar(int colorIndex, int seatsCount)
    {
        CarModel newCar = new CarModel(_cars.Count, colorIndex, seatsCount);
        _cars.Add(newCar);

        return newCar;
    }

    public int GetRandomIndex()
    {
        var notLinkedCars = _cars.Where(car => !car.IsLinked);

        if (!notLinkedCars.Any())
            return 0;

        CarModelStatus maxLevelStatus = notLinkedCars.Max(car => car.Status);
        List<CarModel> maxLevelCars = notLinkedCars.Where(car => car.Status == maxLevelStatus).ToList();

        CarModel randomCar = maxLevelCars[UnityEngine.Random.Range(0, maxLevelCars.Count())];
        randomCar.AddAnimal();

        return randomCar.ColorIndex;
    }

    public void UpdateParkingPlaces(int parkingPlaces)
    {
        _maxParkingPlaces = parkingPlaces;
    }

    public void FindHintCarAndInvokeAction()
    {
        IReadOnlyList<CarModel> cars = GetHintCars();

        if (cars.Count > 0)
        {
            NeedShowHint?.Invoke();
        }
    }

    public IReadOnlyList<CarModel> GetHintCars()
    {
        if (_cars == null)
        {
            return new List<CarModel>();
        }

        if (_cars.All(car => car.Status == CarModelStatus.SecondParkingStay || car.Status == CarModelStatus.SecondParkingLeft))
        {
            if(_cars.Any(car => car.Status == CarModelStatus.SecondParkingStay && !car.IsLinked))
            {
                LevelFinished?.Invoke();
            }

            return new List<CarModel>();
        }
        if (_cars.Count(car => car.Status == CarModelStatus.SecondParkingStay) == _maxParkingPlaces)
        {
            return _cars.OrderBy(car => car.OrderParking).Where(car => car.Status == CarModelStatus.SecondParkingStay).ToList();
        }

        return new List<CarModel>();
    }
}

public class CarModel
{
    public int Id { get; private set; }
    public int ColorIndex { get; private set; }
    public int SeatsCount { get; private set; }
    public int OrderParking { get; private set; }
    public CarModelStatus Status { get; private set; }
    public bool IsLinked => SeatsCount == _animalsCount;

    private int _animalsCount;

    public CarModel(int id, int colorIndex, int seatsCount)
    {
        Id = id;
        _animalsCount = 0;
        ColorIndex = colorIndex;
        SeatsCount = seatsCount;
        Status = CarModelStatus.FirstParkingGray;
    }

    public void AddAnimal()
    {
        _animalsCount++;
    }

    public void GrayModeOff()
    {
        Status = CarModelStatus.FirstParkingColor;
    }

    public void StartLeaveFirstParking()
    {
        Status = CarModelStatus.FirstParkingLast;
    }

    public void LeaveFirstParking()
    {
        Status = CarModelStatus.QueueWait;
    }

    public void MoveToSecondParking(int orderParking)
    {
        OrderParking = orderParking;
        Status = CarModelStatus.SecondParkingMove;
    }

    public void StayOnSecondParking()
    {
        Status = CarModelStatus.SecondParkingStay;
    }

    public void LeftSecondParking()
    {
        Status = CarModelStatus.SecondParkingLeft;
    }
}

public enum CarModelStatus
{
    FirstParkingGray = 1,
    FirstParkingColor = 2,
    FirstParkingLast = 3,
    QueueWait = 4,
    SecondParkingMove = 5,
    SecondParkingStay = 6,
    SecondParkingLeft = -1
}