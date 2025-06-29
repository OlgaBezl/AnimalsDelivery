using System;
using System.Collections.Generic;
using System.Linq;
using Scripts.UI.Panels;
using UnityEngine;

namespace Scripts.Cars.CarPlaces
{
    public class CarPlacesQueue : MonoBehaviour
    {
        [SerializeField] private CarPlace _carPlacePrefab;
        [SerializeField] private float _count;
        [SerializeField] private float _freeCount;
        [SerializeField] private float _offset;
        [SerializeField] private float _degriesRotation;
        [SerializeField] private ShowRewardPanel _showRewardPanel;

        private List<CarPlace> _carPlacesInQueue;

        public event Action UnlockedNewPlace;

        public int UnlockedPlacesCount =>
            _carPlacesInQueue == null ? 0 : _carPlacesInQueue.Count(place => !place.IsLocked);

        private void OnValidate()
        {
            if (_carPlacePrefab == null)
                throw new NullReferenceException(nameof(_carPlacePrefab));

            if (_count <= 0)
                throw new ArgumentOutOfRangeException(nameof(_count));

            if (_showRewardPanel == null)
                throw new NullReferenceException(nameof(_showRewardPanel));
        }

        public void Load()
        {
            _carPlacesInQueue = new List<CarPlace>();
            Quaternion rotationQuaternion = Quaternion.Euler(0, _degriesRotation, 0);

            for (int i = 0; i < _count; ++i)
            {
                Vector3 newPosition = transform.position + new Vector3(0f, 0f, _offset * i);
                CarPlace place = Instantiate(_carPlacePrefab, newPosition, rotationQuaternion, transform);
                place.Unlocked += UnlockPlace;
                _carPlacesInQueue.Add(place);

                place.Initialize(i >= _freeCount, _showRewardPanel, i);
            }
        }

        public void Unload()
        {
            foreach (CarPlace place in _carPlacesInQueue)
            {
                place.Unlocked -= UnlockPlace;
                Destroy(place.gameObject);
            }

            _carPlacesInQueue.Clear();
        }

        public CarPlace GetFreePlace()
        {
            return _carPlacesInQueue.FirstOrDefault(place => place.IsFree && !place.IsLocked);
        }

        private void UnlockPlace()
        {
            UnlockedNewPlace?.Invoke();
        }
    }
}