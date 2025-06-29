using System;
using System.Collections.Generic;
using Scripts.Cars.Containers;
using Scripts.Cars.Model;
using Scripts.Queues;
using UnityEngine;

namespace Scripts.Helpers
{
    public class CarCololrsList : MonoBehaviour
    {
        [SerializeField] private FirstParking _firstParking;
        [SerializeField] private CarQueue _carQueue;
        [SerializeField] private SecondParking _secondParking;

        private CarList _carList;

        private void OnValidate()
        {
            if (_firstParking == null)
                throw new NullReferenceException(nameof(_firstParking));

            if (_carQueue == null)
                throw new NullReferenceException(nameof(_carQueue));

            if (_secondParking == null)
                throw new NullReferenceException(nameof(_secondParking));
        }

        public void Load(CarList list)
        {
            _carList = list;
        }

        public int GetFreeRandomColorIndex(List<Tuple<int, int>> exceptColorTuples)
        {
            return _carList.GetRandomIndex();
        }
    }
}