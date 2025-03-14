using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Color = UnityEngine.Color;

public class AnimalQueue : DefaultQueue<Animal>
{
    [SerializeField] private Animal[] _prefabs;
    //[SerializeField] private Transform _container;
    //[SerializeField] private Transform _startPoint;
    //[SerializeField] private Transform _exitParkingPoint;
    [SerializeField] private CarCololrsList _carCololrsList;
    [SerializeField] private SecondParking _parking;
    [SerializeField] private int _length = 20; 
    [SerializeField] private AddCarButton _addCarButton;

    //public event Action<Animal> CarNotFinded;
    private void OnValidate()
    {
        if (_prefabs == null)
            throw new NullReferenceException(nameof(_prefabs));

        if (_startPoint == null)
            throw new NullReferenceException(nameof(_startPoint));

        if (_endPoint == null)
            throw new NullReferenceException(nameof(_endPoint));

        if (_carCololrsList == null)
            throw new NullReferenceException(nameof(_carCololrsList));

        if (_parking == null)
            throw new NullReferenceException(nameof(_parking));

        if (_length <=0)
            throw new ArgumentOutOfRangeException(nameof(_length));

        if (_addCarButton == null)
            throw new NullReferenceException(nameof(_addCarButton));        
    }
    float _zOffset;
    public override void StartLevel()
    {
        float _currentOffset = 0;
        _zOffset = Vector3.Distance(_startPoint.position, _endPoint.position) / (_length-1);

        base.StartLevel(); 

        for (int i = 0; i < _length; i++)
        {
            if (_startPoint.position.z + _currentOffset < _startPoint.position.z)
                return;

            Vector3 offset = _endPoint.position - _startPoint.position - new Vector3(0, 0, _currentOffset);
            int colorIndex = _carCololrsList.GetFreeRandomColorIndex(GetColors());
            Animal prefab = _prefabs.FirstOrDefault(animal => animal.ColorIndex == colorIndex);
            Spawn(prefab, colorIndex, offset);
            //Spawn(_prefabs, _carCololrsList.GetRandomColor(), Vector3.forward);
            _currentOffset += _zOffset;
        }
    }

    public override void FirstAction()
    {
        int colorIndex = _carCololrsList.GetFreeRandomColorIndex(GetColors());

        if (colorIndex != ColorPallet.GrayIndex)
        {
            Animal prefab = _prefabs.FirstOrDefault(animal => animal.ColorIndex == colorIndex);
            Spawn(prefab, colorIndex, Vector3.zero);
        }

        TryPutAnimalToCar();
    }

    private List<Tuple<int, int>> GetColors()
    {
        return Queue.Select(animal => new Tuple<int, int>(animal.ColorIndex, 1)).ToList();
    }

    public int GetFirstColorIndex()
    {
        return Queue.Count > 0 ? Queue.First().ColorIndex : ColorPallet.GrayIndex;
    }

    public bool TryPutAnimalToCar()
    {
        if (QueueIsMoving)
        {
            //Debug.Log($"TryPut MOVING");
            return false;
        }

        Animal animal = Queue.Peek();

        CarWithSeats car = _parking.GetFreeCar(animal.ColorIndex);

        if (car == null)
        {
            return false;
        }

        animal.transform.parent = car.transform;
        Seat seat = car.TakeSeat();
        animal.MoveToSeat(seat, car.EntryPoint);

        MoveQueue(_zOffset);

        return true;
    }
}
