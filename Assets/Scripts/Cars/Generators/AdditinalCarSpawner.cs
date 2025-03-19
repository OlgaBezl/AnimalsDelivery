using System;
using UnityEngine;
using Scripts.Cars.CarPlaces;
using Scripts.Cars.Containers;
using Scripts.Helpers;
using Scripts.Queues;
using Scripts.Progress;

namespace Scripts.Cars.Generators
{
    public class AdditinalCarSpawner : MonoBehaviour
    {
        [SerializeField] private AnimalQueue _animalQueue;
        [SerializeField] private CarPlacesQueue _carPlacesQueue;
        [SerializeField] private SecondParking _secondParking;
        [SerializeField] private Result _result;
        [SerializeField] private CarWithSeats _carPrefab;

        private void OnValidate()
        {
            if (_animalQueue == null)
                throw new NullReferenceException(nameof(_animalQueue));

            if (_carPlacesQueue == null)
                throw new NullReferenceException(nameof(_carPlacesQueue));

            if (_result == null)
                throw new NullReferenceException(nameof(_result));

            if (_carPrefab == null)
                throw new NullReferenceException(nameof(_carPrefab));
        }

        public void SpawnCar()
        {
            int colorIndex = _animalQueue.GetFirstColorIndex();
            CarPlace freePlace = _carPlacesQueue.GetFreePlace();

            if (freePlace != null && colorIndex != ColorPallet.GrayIndex)
            {
                _result.AddTotalPoints();
                CarWithSeats car = Instantiate(_carPrefab, freePlace.transform.position, freePlace.transform.rotation);
                car.Initialize(colorIndex);
                _secondParking.TakePlaceWithoutQueue(car);
            }
        }
    }
}