using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts.Cars;
using Scripts.Cars.Containers;
using Scripts.Cars.Model;
using UnityEngine;

namespace Scripts.Queues
{
    public class CarQueue : DefaultQueue<CarWithSeats>
    {
        [SerializeField] private List<CarWithSeats> _carPrefabs;
        [SerializeField] private SecondParking _parking;

        private void OnValidate()
        {
            if (_carPrefabs == null)
                throw new NullReferenceException(nameof(_carPrefabs));

            if (_parking == null)
                throw new NullReferenceException(nameof(_parking));
        }

        public override void StartLevel()
        {
            base.StartLevel();
            _parking.NewPlaceUnlocked += Unlock;
        }

        public List<Tuple<int, int>> GetColors()
        {
            return Queue.Select(car => new Tuple<int, int>(car.ColorIndex, car.Specification.SeatsCount)).ToList();
        }

        public void QueueUpCar(CarModel carModel)
        {
            CarWithSeats prefab = _carPrefabs.FirstOrDefault(car => car.Specification.SeatsCount == carModel.SeatsCount);

            Vector3 offset = Vector3.zero;

            if (Queue.Count > 0)
            {
                float zOffset = Queue.Sum(item => item.Length) - Queue.First().Specification.Length + prefab.Specification.Length;
                offset = Vector3.back * zOffset;
            }

            CarWithSeats carWithSeats = Spawn(prefab, carModel.ColorIndex, offset);
            carWithSeats.SetModel(carModel);
            carWithSeats.StartLeftParking += CarLeftParking;

            if (_parking.HasFreePlace())
            {
                _parking.TakePlace(carWithSeats);
                MoveCarQueue();
            }
        }

        private void MoveCarQueue()
        {
            if (Queue.Count <= 1)
            {
                MoveQueue(0);
                return;
            }

            CarWithSeats[] queueList = Queue.ToArray();
            float zOffset = queueList[1].Length;
            MoveQueue(zOffset);
        }

        private void CarLeftParking(CarWithSeats carWithSeats)
        {
            carWithSeats.LeftParking -= CarLeftParking;

            if (Queue.Count > 0 && _parking.HasFreePlace())
            {
                _parking.TakePlace(Queue.Peek());
                MoveCarQueue();
            }
        }

        private void Unlock()
        {
            if (Queue.Count > 0 && _parking.HasFreePlace())
            {
                _parking.TakePlace(Queue.Peek());
                MoveCarQueue();
            }
        }

        public override void FirstAction()
        {
        }
    }
}