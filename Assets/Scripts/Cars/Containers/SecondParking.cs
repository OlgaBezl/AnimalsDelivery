using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts.Cars.CarPlaces;
using Scripts.Cars.Model;
using Scripts.Effects;
using UnityEngine;

namespace Scripts.Cars.Containers
{
    public class SecondParking : MonoBehaviour
    {
        [SerializeField] public Transform _endPoint;
        [SerializeField] private Cloud _cloud;
        [SerializeField] private CarPlacesQueue _carPlacesQueue;

        private List<CarWithSeats> _carsInQueue;
        private CarList _carList;
        private bool _hintCarsJumping;
        
        public bool HasFreePlace => _carPlacesQueue?.GetFreePlace() != null;
        
        public event Action<CarWithSeats> CarWasParked;
        public event Action<CarWithSeats> CarLeft;
        public event Action NewPlaceUnlocked;

        private void OnValidate()
        {
            if (_carPlacesQueue == null)
                throw new NullReferenceException(nameof(_carPlacesQueue));

            if (_cloud == null)
                throw new NullReferenceException(nameof(_cloud));
        }

        public void Load(CarList carList)
        {
            _carList = carList;
            _carList.NeedShowHint += ShowHintAfterDelay;

            _carsInQueue = new List<CarWithSeats>();
            _carPlacesQueue.Load();
            _carPlacesQueue.UnlockedNewPlace += UnlockUnlockedNewPlace;
        }

        public void Unload()
        {
            _carList.NeedShowHint -= ShowHintAfterDelay;

            _carsInQueue = null;
            _carPlacesQueue.Unload();
            _carPlacesQueue.UnlockedNewPlace -= UnlockUnlockedNewPlace;
        }

        public void TakePlace(CarWithSeats carWithSeats)
        {
            CarPlace freePlace = _carPlacesQueue.GetFreePlace();

            if (freePlace == null)
                return;

            carWithSeats.Model.MoveToSecondParking(freePlace.OrderNumber);
            freePlace.Take();

            _carsInQueue.Add(carWithSeats);
            carWithSeats.WasParked += ParkCar;
            carWithSeats.StartLeftParking += StartDriveCarOut;
            carWithSeats.LeftParking += FinishDriveCarOut;
            carWithSeats.MoveToParkingPlace(freePlace, _endPoint);
        }

        public CarWithSeats GetFreeCar(int colorIndex)
        {
            return _carsInQueue.FirstOrDefault(car =>
            car.State == CarState.Parked && car.HasFreeSeats && car.ColorIndex == colorIndex);
        }

        private void ParkCar(CarWithSeats car)
        {
            car.WasParked -= ParkCar;
            car.Model.ChangeStatus(CarModelStatus.SecondParkingStay);
            CarWasParked?.Invoke(car);

            _carList.ShowHintIfNeeded();
        }

        private void StartDriveCarOut(CarWithSeats car)
        {
            car.StartLeftParking -= StartDriveCarOut;
            car.Model.ChangeStatus(CarModelStatus.SecondParkingLeft);
            _carsInQueue.Remove(car);
        }

        private void FinishDriveCarOut(CarWithSeats car)
        {
            car.LeftParking -= FinishDriveCarOut;

            Instantiate(_cloud, car.transform.position, car.transform.rotation);
            Destroy(car.gameObject);
            CarLeft?.Invoke(car);

            _carList.ShowHintIfNeeded();
        }

        private void ShowHintAfterDelay()
        {
            if (_hintCarsJumping)
                return;

            _hintCarsJumping = true;
            StartCoroutine(ShowHintWithDelay(3));
        }

        private void UnlockUnlockedNewPlace()
        {
            _carList.UpdateParkingPlaces(_carPlacesQueue.UnlockedPlacesCount);
            NewPlaceUnlocked?.Invoke();
        }

        private IEnumerator ShowHintWithDelay(int count)
        {
            count--;
            float delay = 1.5f;
            yield return new WaitForSeconds(delay);

            IReadOnlyList<CarModel> carModels = _carList.GetHintCars();

            if (carModels.Count > 0)
            {
                _hintCarsJumping = true;
                float animationDelay = 0f;

                foreach (CarModel carModel in carModels)
                {
                    animationDelay += 0.25f;
                    CarWithSeats car = _carsInQueue.FirstOrDefault(car => car.Model.Id == carModel.Id);
                    car.Jump(animationDelay);
                }

                if (count > 0)
                {
                    StartCoroutine(ShowHintWithDelay(count));
                }
                else
                {
                    _hintCarsJumping = false;
                }
            }
            else
            {
                _hintCarsJumping = false;
            }
        }
    }
}