using System;
using DG.Tweening;
using Scripts.Cars.Matrix;
using Scripts.Cars.Model;
using UnityEngine;

namespace Scripts.Cars
{
    public class ArrowCar : Car
    {
        [SerializeField] private GameObject _arrow;
        [SerializeField] private Transform _rotation;
        [SerializeField] private float _distanceToMapBorder = 10f;

        private Transform _transform;
        private CarMatrix _carMatrix;
        private Vector3 _parkingBorder;
        private Vector3 _startPoint;
        private float _maxDistanceToParkingBorder;

        public event Action<ArrowCar> ParkingStartLeaving;
        public event Action<ArrowCar> ParkingFinalLeaved;

        public Vector2Int Location { get; private set; }
        public Vector2Int Forward { get; private set; }
        public Vector2Int Direction { get; private set; }

        private void OnMouseUp()
        {
            if (CanLeftParking)
            {
                ParkingStartLeaving?.Invoke(this);
                Trace.Show();
                MoveToParkingEdge(_transform.position + _transform.forward * _maxDistanceToParkingBorder);
            }
        }

        public void Initialize(CarMatrix carMatrix, int colorIndex, Vector3 parkingBorder)
        {
            Initialize(colorIndex);

            _transform = transform;
            _carMatrix = carMatrix;
            _parkingBorder = parkingBorder;
            _maxDistanceToParkingBorder = 2 * Math.Max(Math.Abs(parkingBorder.x), Math.Abs(parkingBorder.z));

            var direction = transform.forward;
            Vector3 startPosition = transform.position;
            Vector3 forwardPosition = transform.position + Specification.Length * direction;

            int x = (int)Mathf.Round(startPosition.x) + _carMatrix.MatrixCenter;
            int z = (int)Mathf.Round(startPosition.z) + _carMatrix.MatrixCenter;
            Location = new Vector2Int(x, z);

            int forwardX = (int)Mathf.Round(forwardPosition.x) + _carMatrix.MatrixCenter;
            int forwardZ = (int)Mathf.Round(forwardPosition.z) + _carMatrix.MatrixCenter;
            Forward = new Vector2Int(forwardX, forwardZ);

            int directionX = (int)direction.normalized.x;
            int directionZ = (int)direction.normalized.z;
            Direction = new Vector2Int(directionX, directionZ);
        }

        public override void GrayModeOff()
        {
            base.GrayModeOff();
            Model.ChangeStatus(CarModelStatus.FirstParkingColor);
            _arrow.SetActive(true);
        }

        private void MoveToParkingEdge(Vector3 target)
        {
            float moveDuration = Vector3.Magnitude(target - _transform.position) / Speed;
            _startPoint = _transform.position;
            var sequence = DOTween.Sequence();

            sequence.Append(_transform.DOMove(target, moveDuration)).
                SetEase(Ease.Linear).
                OnUpdate(() =>
                {
                    if (Mathf.Abs(_transform.position.x) >= _parkingBorder.x ||
                        Mathf.Abs(_transform.position.z) >= _parkingBorder.z)
                    {
                        sequence.Kill();
                        TurnAndLeaveParking();
                    }
                });
        }

        private void TurnAndLeaveParking()
        {
            Vector3 turnPoint = transform.position;
            Vector3 endPoint = transform.position;
            Quaternion rotation = Quaternion.Euler(0, 0, 0);

            if (Mathf.Abs(turnPoint.x) >= _parkingBorder.x)
            {
                if (turnPoint.z < _startPoint.z)
                {
                    endPoint = new Vector3(turnPoint.x, turnPoint.y, turnPoint.z - _distanceToMapBorder);
                    rotation = Quaternion.Euler(0, turnPoint.y - 180, 0);
                }
                else
                {
                    endPoint = new Vector3(turnPoint.x, turnPoint.y, turnPoint.z + _distanceToMapBorder);
                    rotation = Quaternion.Euler(0, turnPoint.y, 0);
                }
            }

            if (Mathf.Abs(turnPoint.z) >= _parkingBorder.z)
            {
                if (turnPoint.x < _startPoint.x)
                {
                    endPoint = new Vector3(turnPoint.x - _distanceToMapBorder, -turnPoint.y, turnPoint.z);
                    rotation = Quaternion.Euler(0, turnPoint.y - 90, 0);
                }
                else
                {
                    endPoint = new Vector3(turnPoint.x + _distanceToMapBorder, turnPoint.y, turnPoint.z);
                    rotation = Quaternion.Euler(0, turnPoint.y + 90, 0);
                }
            }

            MoveAlongEdge(endPoint, rotation);
        }

        private void MoveAlongEdge(Vector3 target, Quaternion rotation)
        {
            float moveDuration = Vector3.Magnitude(target - _transform.position) / Speed;
            _startPoint = _transform.position;

            DOTween.Sequence().
                Append(_transform.DORotateQuaternion(rotation, RotateDuration)).
                Append(_transform.DOMove(target, moveDuration)).
                SetEase(Ease.Linear).
                onComplete += () => ParkingFinalLeaved?.Invoke(this);
        }
    }
}