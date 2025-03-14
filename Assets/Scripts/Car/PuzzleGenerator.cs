using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using static Rotation;

public class PuzzleGenerator : MonoBehaviour
{
    [SerializeField] private List<ArrowCar> _carPrefabs;
    [SerializeField] private CarCololrsList _carCololrsList; 
    [SerializeField] private CarMatrix _carMatrix; 
    [SerializeField] private Transform _container;
    [SerializeField] private float _containerRotation;
    [SerializeField] private FirstParking _firstParking;
    [SerializeField] private Transform _parkingBorderPoint;
    [SerializeField] private BiomPainter _biomPainter;

    private float _currentPoints = 0;
    private Queue<Car> _queueForGenerate;
    int _tryIndex = 0;
    int _counter = 0;
    private CarList _carList;
    private bool _isActive;

    private void OnValidate()
    {
        if (_carPrefabs == null || _carPrefabs.Count == 0)
            throw new NullReferenceException(nameof(_carPrefabs));

        if (_carCololrsList == null)
            throw new NullReferenceException(nameof(_carCololrsList));

        if (_carMatrix == null)
            throw new NullReferenceException(nameof(_carMatrix));

        if (_container == null)
            throw new NullReferenceException(nameof(_container));

    }

    public void StartLevel(LevelInfo levelInfo, CarList carList)
    {
        Debug.Log("START puzzle");
        while (_isActive)
        {}


        _container.rotation = Quaternion.identity;

        Debug.Log("START 2 puzzle");
        _currentPoints = levelInfo.Points;
        _carList = carList;
        _queueForGenerate = new Queue<Car>();
        _carMatrix.StartLevel();
        _firstParking.StartLevel(carList);
        RepaintCarPrefabs(levelInfo.Biom);

        GenerateCar(GetRandomPrefab(), GetRandomRotationWithoutLimit(), Vector3.zero);

        while (_currentPoints > 0)
        {
            GenerateCarsAroundBase();
        }

        _firstParking.UpdateGrayMode();

        _container.Rotate(Vector3.up, _containerRotation);
        Debug.Log("START end puzzle");
        _isActive = true;
    }

    public void FinishLevel()
    {
        Debug.Log("FINISH puzzle");
        _currentPoints = 0;
        _queueForGenerate = null;
        _carMatrix.FinishLevel();
        _carList.Clear();

        int childIndex = 0;

        while (childIndex < _container.childCount)
        {
            Destroy(_container.GetChild(childIndex).gameObject);
            childIndex++;
        }

        //_container.Rotate(Vector3.up, -_containerRotation);
        Debug.Log("FINISH end puzzle");
        _isActive = false;
    }

    private void RepaintCarPrefabs(BiomType biomType)
    {
        if(_carPrefabs.Count == 0) 
            return;

        Texture biomTexture = _biomPainter.GetTexture(biomType);
        _carPrefabs[0].RepaintSharedMaterial(biomTexture);
    }
    
    private void GenerateCarsAroundBase()
    {
        if (_queueForGenerate.Count == 0)
        {
            Debug.Log("Нет машин");
            return;
        }

        Car lastCar = _queueForGenerate.Dequeue();

        for (int i = 0; i < 4; i++)
        {
            if (_currentPoints <= 0)
            {
                return;
            }

            Vector3 lastCarOffset;

            if ((Side)i == Side.Forward)
            {
                lastCarOffset = lastCar.transform.localPosition + lastCar.transform.forward * lastCar.Type.Length;
                TryGenerateCar(lastCarOffset, lastCar.transform.forward);
            }
            else if ((Side)i == Side.Back)
            {
                lastCarOffset = lastCar.transform.localPosition - lastCar.transform.forward;
                TryGenerateCar(lastCarOffset, -lastCar.transform.forward);
            }
            else
            {
                Vector3 sideOffset = (Side)i == Side.Right ? lastCar.transform.right : -lastCar.transform.right;

                for (int l = 1; l <= lastCar.Type.Length; l++)
                {
                    lastCarOffset = lastCar.transform.localPosition + lastCar.transform.forward * (lastCar.Type.Length - l) + sideOffset;
                    TryGenerateCar(lastCarOffset, sideOffset);
                }
            }
        }
    }

    private bool TryGenerateCar(Vector3 startPosition, Vector3 lastCarForward)
    {
        if (_tryIndex > 10)
        {
            Debug.Log($"10 try! {startPosition}");
            return true;
        }

        if (_carMatrix.MarixCellIsEmpty(startPosition.x, startPosition.z) == false)
        {
            return false;
        }

        if (_currentPoints <= 0)
        {
            return true;
        }

        RotationType rotationType = GetRandomRotationWithoutLimit();
        ArrowCar prefab = GetRandomPrefab();

        if (_carMatrix.CanGenerateCarByPosition(prefab.Type.Length, ConvertRotationToDirection(rotationType), startPosition.x, startPosition.z))
        {
            _tryIndex = 0;
            GenerateCar(prefab, rotationType, startPosition);
            return true;
        }
        else
        {
            _tryIndex++;
            bool carWasCreated = false;

            while (carWasCreated == false)
            {
                carWasCreated = TryGenerateCar(startPosition, lastCarForward);
            }

            return true;
        }
    }

    private ArrowCar GetRandomPrefab()
    {
        List<ArrowCar> cars = _carPrefabs.Where(car => car.Type.SeatsCount <= _currentPoints).ToList();
        return cars[UnityEngine.Random.Range(0, cars.Count)];
    }

    private Car GenerateCar(ArrowCar prefab, RotationType rotationType, Vector3 lastCarOffset)
    {
        int colorIndex = ColorPallet.GetRandomColorIndex();
        Vector3 newPosition = transform.position + lastCarOffset;

        if (_carMatrix.CanGenerateCarByPosition(prefab.Type.Length, ConvertRotationToDirection(rotationType), newPosition.x, newPosition.z) == false)
        {
            return null;
        }

        _counter++;
        CarModel carModel = _carList.AddCar(colorIndex, prefab.Type.SeatsCount);
        ArrowCar car = Instantiate(prefab, newPosition, Quaternion.AngleAxis(ConvertRotationTypeToDegrees(rotationType), Vector3.up), _container);
        car.gameObject.name = $"{_counter}_car_type{prefab.Type.SeatsCount}_";
        car.SetModel(carModel);
        car.Initialize(_carMatrix, colorIndex, _parkingBorderPoint.position);
        _firstParking.AddCar(car);

        _carMatrix.FillMatrix(colorIndex);
        _currentPoints -= car.Type.SeatsCount;
        _queueForGenerate.Enqueue(car);

        return car;
    }
    
    private enum Side
    {
        Forward,
        Right,
        Back,
        Left
    }
}
