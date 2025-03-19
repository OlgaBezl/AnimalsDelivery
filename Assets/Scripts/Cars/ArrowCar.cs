using System;
using UnityEngine;
using DG.Tweening;
using Scripts.Cars.Matrix;

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

        public event Action<ArrowCar> LeaveParkingStart;
        public event Action<ArrowCar> LeaveParkingEnd;

        public int X { get; private set; }
        public int Z { get; private set; }
        public int ForwardX { get; private set; }
        public int ForwardZ { get; private set; }
        public int DirectionX { get; private set; }
        public int DirectionZ { get; private set; }

        public void Initialize(CarMatrix carMatrix, int colorIndex, Vector3 parkingBorder)
        {
            Initialize(colorIndex);

            _transform = transform;
            _carMatrix = carMatrix;
            _parkingBorder = parkingBorder;
            _maxDistanceToParkingBorder = 2 * Math.Max(Math.Abs(parkingBorder.x), Math.Abs(parkingBorder.z));

            var direction = transform.forward;
            Vector3 startPosition = transform.position;
            Vector3 forwardPosition = transform.position + Type.Length * direction;

            X = (int)Mathf.Round(startPosition.x) + _carMatrix.MatrixCenter;
            Z = (int)Mathf.Round(startPosition.z) + _carMatrix.MatrixCenter;

            ForwardX = (int)Mathf.Round(forwardPosition.x) + _carMatrix.MatrixCenter;
            ForwardZ = (int)Mathf.Round(forwardPosition.z) + _carMatrix.MatrixCenter;

            DirectionX = (int)direction.normalized.x;
            DirectionZ = (int)direction.normalized.z;
        }

        private void OnMouseUp()
        {
            if (CanLeftParking)
            {
                LeaveParkingStart?.Invoke(this);
                Trace.Show();
                MoveToParkingEdge(_transform.position + _transform.forward * _maxDistanceToParkingBorder);
            }
        }

        public override void GrayModeOff()
        {
            base.GrayModeOff();
            Model.GrayModeOff();
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
                onComplete += () => LeaveParkingEnd?.Invoke(this);
        }
    }
}