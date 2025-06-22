using System;
using System.Collections.Generic;
using System.Linq;
using Scripts.Cars.Containers;
using Scripts.Cars.Matrix;
using Scripts.Cars.Model;
using Scripts.Enviroment;
using Scripts.Helpers;
using Scripts.Progress;
using UnityEngine;

namespace Scripts.Cars.Generators
{
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
        private int _counter = 0;
        private CarList _carList;

        private enum Side
        {
            Forward = 0,
            Right = 1,
            Back = 2,
            Left = 3
        }

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
            _container.rotation = Quaternion.identity;
            _currentPoints = levelInfo.Points;
            _carList = carList;
            _queueForGenerate = new Queue<Car>();
            _carMatrix.StartLevel();
            _firstParking.StartLevel(carList);
            RepaintCarPrefabs(levelInfo.Biom);

            GenerateCar(GetRandomPrefab(), Rotation.GetRandomRotationWithoutLimit(), Vector3Int.zero);

            while (_currentPoints > 0)
            {
                GenerateCarsAroundBase();
            }

            _firstParking.UpdateGrayMode();

            _container.Rotate(Vector3.up, _containerRotation);
        }

        public void Unload()
        {
            _currentPoints = 0;
            _queueForGenerate = null;
            _carMatrix.Unload();
            _carList.Unload();

            int childIndex = 0;

            while (childIndex < _container.childCount)
            {
                Destroy(_container.GetChild(childIndex).gameObject);
                childIndex++;
            }
        }

        private void RepaintCarPrefabs(BiomType biomType)
        {
            if (_carPrefabs.Count == 0)
                return;

            Texture biomTexture = _biomPainter.GetBiom(biomType).MainTexture;
            _carPrefabs[0].RepaintSharedMaterial(biomTexture);
        }

        private void GenerateCarsAroundBase()
        {
            if (_queueForGenerate.Count == 0)
            {
                return;
            }

            Car lastCar = _queueForGenerate.Dequeue();
            int sideCount = 4;

            for (int i = 0; i < sideCount; i++)
            {
                if (_currentPoints <= 0)
                {
                    return;
                }

                Vector3 lastCarOffset;

                if ((Side)i == Side.Forward)
                {
                    lastCarOffset = lastCar.transform.localPosition + lastCar.transform.forward * lastCar.Specification.Length;
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

                    for (int l = 1; l <= lastCar.Specification.Length; l++)
                    {
                        lastCarOffset = lastCar.transform.localPosition +
                            lastCar.transform.forward * (lastCar.Specification.Length - l) + sideOffset;
                        TryGenerateCar(lastCarOffset, sideOffset);
                    }
                }
            }
        }

        private bool TryGenerateCar(Vector3 startPosition, Vector3 lastCarForward)
        {
            Vector3Int roundStartPosition = RoundVector(startPosition);

            if (_carMatrix.MarixCellIsEmpty(roundStartPosition) == false)
            {
                return false;
            }

            if (_currentPoints <= 0)
            {
                return true;
            }

            RotationType rotationType = Rotation.GetRandomRotationWithoutLimit();
            ArrowCar prefab = GetRandomPrefab();
            Vector3Int forward = Rotation.ConvertRotationToDirection(rotationType);

            if (_carMatrix.CanGenerateCarByPosition(prefab.Specification.Length, forward, roundStartPosition))
            {
                GenerateCar(prefab, rotationType, roundStartPosition);
                return true;
            }
            else
            {
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
            List<ArrowCar> cars = _carPrefabs.Where(car => car.Specification.SeatsCount <= _currentPoints).ToList();
            return cars[UnityEngine.Random.Range(0, cars.Count)];
        }

        private Car GenerateCar(ArrowCar prefab, RotationType rotationType, Vector3Int lastCarOffset)
        {
            int colorIndex = ColorPallet.GetRandomColorIndex();
            Vector3 newPosition = transform.position + lastCarOffset;
            Vector3Int forward = Rotation.ConvertRotationToDirection(rotationType);

            if (!_carMatrix.CanGenerateCarByPosition(prefab.Specification.Length, forward, RoundVector(newPosition)))
            {
                return null;
            }

            _counter++;
            CarModel carModel = _carList.AddCar(colorIndex, prefab.Specification.SeatsCount);

            float angleRotation = Rotation.ConvertRotationTypeToDegrees(rotationType);
            Quaternion rotation = Quaternion.AngleAxis(angleRotation, Vector3.up);
            ArrowCar car = Instantiate(prefab, newPosition, rotation, _container);
            car.gameObject.name = $"{_counter}_car_type{prefab.Specification.SeatsCount}_";
            car.SetModel(carModel);
            car.Initialize(_carMatrix, colorIndex, _parkingBorderPoint.position);
            _firstParking.AddCar(car);

            _carMatrix.FillMatrix();
            _currentPoints -= car.Specification.SeatsCount;
            _queueForGenerate.Enqueue(car);

            return car;
        }

        private Vector3Int RoundVector(Vector3 vector3)
        {
            return new Vector3Int((int)Mathf.Round(vector3.x), (int)Mathf.Round(vector3.y), (int)Mathf.Round(vector3.z));
        }
    }
}