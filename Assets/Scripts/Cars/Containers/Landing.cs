using System;
using Scripts.Cars.Model;
using Scripts.Queues;
using UnityEngine;

namespace Scripts.Cars.Containers
{
    public class Landing : MonoBehaviour
    {
        [SerializeField] private AnimalQueue _animalQueue;
        [SerializeField] private SecondParking _parking;

        private void OnValidate()
        {
            if (_animalQueue == null)
                throw new NullReferenceException(nameof(_animalQueue));

            if (_parking == null)
                throw new NullReferenceException(nameof(_parking));
        }

        public void StartLevel(CarList carList)
        {
            _animalQueue.StartLevel();
            _parking.StartLevel(carList);

            _parking.CarWasParked += ParkedCarInQueue;
        }

        public void Unload()
        {
            _animalQueue.Unload();
            _parking.Unload();

            _parking.CarWasParked -= ParkedCarInQueue;
        }

        private void ParkedCarInQueue(CarWithSeats car)
        {
            if (car == null)
                throw new NullReferenceException(nameof(car));

            _animalQueue.TryPutAnimalToCar();
        }
    }
}