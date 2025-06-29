using System;
using System.Collections.Generic;
using System.Linq;

namespace Scripts.Cars.Model
{
    public class CarList
    {
        private List<CarModel> _cars;
        private int _maxParkingPlaces = 4;

        public event Action NeedShowHint;
        public event Action LevelFinished;

        public void Initialize()
        {
            _cars = new List<CarModel>();
        }

        public void Unload()
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

        public void ShowHintIfNeeded()
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

            if (_cars.All(car => car.Status == CarModelStatus.SecondParkingStay ||
                                 car.Status == CarModelStatus.SecondParkingLeft))
            {
                if (_cars.Any(car => car.Status == CarModelStatus.SecondParkingStay && !car.IsLinked))
                {
                    LevelFinished?.Invoke();
                }

                return new List<CarModel>();
            }

            if (_cars.Count(car => car.Status == CarModelStatus.SecondParkingStay) == _maxParkingPlaces)
            {
                return _cars.OrderBy(car => car.OrderParking).
                    Where(car => car.Status == CarModelStatus.SecondParkingStay).ToList();
            }

            return new List<CarModel>();
        }
    }
}