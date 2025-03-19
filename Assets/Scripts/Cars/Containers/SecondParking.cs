using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Scripts.Cars.CarPlaces;
using Scripts.Cars.Model;
using Scripts.Effects;

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

        public void StartLevel(CarList carList)
        {
            _carList = carList;
            _carList.NeedShowHint += ShowHintAfterDelay;

            _carsInQueue = new List<CarWithSeats>();
            _carPlacesQueue.StartLevel();
            _carPlacesQueue.NewPlaceUnlocked += NewPlaceUnlock;
        }

        public void FinishLevel()
        {
            _carList.NeedShowHint -= ShowHintAfterDelay;

            _carsInQueue = null;
            _carPlacesQueue.FinishLevel();
            _carPlacesQueue.NewPlaceUnlocked -= NewPlaceUnlock;
        }

        public bool HasFreePlace()
        {
            return _carPlacesQueue.GetFreePlace() != null;
        }

        public bool TakePlace(CarWithSeats carWithSeats)
        {
            CarPlace freePlace = _carPlacesQueue.GetFreePlace();

            if (freePlace == null)
                return false;

            carWithSeats.Model.MoveToSecondParking(freePlace.OrderNumber);
            freePlace.Take();

            _carsInQueue.Add(carWithSeats);
            carWithSeats.WasParked += ParkedCar;
            carWithSeats.StartLeftParking += CarStartLeftParking;
            carWithSeats.LeftParking += CarLeftParking;
            carWithSeats.MoveToParkingPlace(freePlace, _endPoint);
            return true;
        }

        public bool TakePlaceWithoutQueue(CarWithSeats carWithSeats)
        {
            CarPlace freePlace = _carPlacesQueue.GetFreePlace();

            if (freePlace == null)
                return false;

            freePlace.Take();
            carWithSeats.Model.StayOnSecondParking();

            _carsInQueue.Add(carWithSeats);
            carWithSeats.WasParked += ParkedCar;
            carWithSeats.StartLeftParking += CarStartLeftParking;
            carWithSeats.LeftParking += CarLeftParking;
            carWithSeats.TeleportToParkingPlace(freePlace, _endPoint);
            return true;
        }

        public CarWithSeats GetFreeCar(int colorIndex)
        {
            return _carsInQueue.FirstOrDefault(car =>
            car.State == CarWithSeatsState.Parked && car.HasFreeSeats && car.ColorIndex == colorIndex);
        }

        private void ParkedCar(CarWithSeats car)
        {
            car.WasParked -= ParkedCar;
            car.Model.StayOnSecondParking();
            CarWasParked?.Invoke(car);

            _carList.FindHintCarAndInvokeAction();
        }

        private void CarStartLeftParking(CarWithSeats car)
        {
            car.StartLeftParking -= CarStartLeftParking;
            car.Model.LeftSecondParking();
            _carsInQueue.Remove(car);
        }

        private void CarLeftParking(CarWithSeats car)
        {
            car.LeftParking -= CarLeftParking;

            Instantiate(_cloud, car.transform.position, car.transform.rotation);
            Destroy(car.gameObject);
            CarLeft?.Invoke(car);

            _carList.FindHintCarAndInvokeAction();
        }

        private void ShowHintAfterDelay()
        {
            if (_hintCarsJumping)
                return;

            _hintCarsJumping = true;
            StartCoroutine(ShowHintWithDelay(3));
        }

        private void NewPlaceUnlock()
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