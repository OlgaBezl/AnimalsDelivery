using System;
using System.Collections.Generic;
using Scripts.Cars.Matrix;
using Scripts.Cars.Model;
using Scripts.Effects;
using Scripts.Queues;
using UnityEngine;

namespace Scripts.Cars.Containers
{
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

        public void Load(CarList carList)
        {
            _cars = new List<ArrowCar>();
            _carList = carList;
        }

        public void Unload()
        {
            _cars = null;
        }

        public void AddCar(ArrowCar car)
        {
            if (car == null)
                throw new ArgumentNullException(nameof(car));

            _cars.Add(car);
            car.ParkingStartLeaving += StartDriveCarOut;
            car.ParkingFinalLeaved += FinishDriveCarOut;
        }

        public void UpdateGrayMode()
        {
            foreach (ArrowCar car in _cars)
            {
                if (_carMatrix.CkeckIfCanLeaveParking(car))
                {
                    car.TurnOffGrayMode();
                }
                else
                {
                    car.TurnOnGrayMode();
                }
            }
        }

        private void StartDriveCarOut(ArrowCar car)
        {
            car.ParkingStartLeaving -= StartDriveCarOut;
            car.Model.ChangeStatus(CarModelStatus.FirstParkingLast);

            _cars.Remove(car);
            _carMatrix.ClearCarPlace(car);
            UpdateGrayMode();
        }

        private void FinishDriveCarOut(ArrowCar car)
        {
            car.ParkingFinalLeaved -= FinishDriveCarOut;
            car.Model.ChangeStatus(CarModelStatus.QueueWait);
            _carList.ShowHintIfNeeded();

            Destroy(car.gameObject);
            Instantiate(_cloud, car.transform.position, car.transform.rotation);

            _carQueue.QueueUpCar(car.Model);
        }
    }
}