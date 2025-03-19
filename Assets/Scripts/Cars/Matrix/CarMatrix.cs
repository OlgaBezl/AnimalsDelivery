using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Cars.Matrix
{
    public class CarMatrix : MonoBehaviour
    {
        [SerializeField] private GameObject _testCube;
        [SerializeField] private Transform _matrixContainer;

        private MatrixCell[,] _matrix;
        private int _matrixSize = 50;
        private List<Vector3> _barrierCheckedList;

        public int MatrixCenter { get; private set; }

        public void StartLevel()
        {
            _matrix = new MatrixCell[_matrixSize, _matrixSize];
            _barrierCheckedList = new List<Vector3>();

            for (int x = 0; x < _matrixSize; x++)
            {
                for (int z = 0; z < _matrixSize; z++)
                {
                    _matrix[x, z] = new MatrixCell();
                }
            }

            MatrixCenter = _matrixSize / 2;
        }

        public void FinishLevel()
        {
            _matrix = null;

            for (int i = 0; i < _matrixContainer.childCount; i++)
            {
                Destroy(_matrixContainer.GetChild(i).gameObject);
            }
        }

        public bool CanGenerateCarByPosition(int length, Vector3 forward, float startX, float startZ)
        {
            int directionX = (int)forward.normalized.x;
            int directionZ = (int)forward.normalized.z;

            int x = MatrixCenter + (int)Mathf.Round(startX);
            int z = MatrixCenter + (int)Mathf.Round(startZ);

            for (int offset = 0; offset < length; offset++)
            {
                if (_matrix[x, z].TryMark(forward) == false)
                {
                    ClearTempFields();
                    return false;
                }

                x += directionX;
                z += directionZ;
            }

            _barrierCheckedList.Clear();
            x -= directionX;
            z -= directionZ;

            if (HasBarrier(forward, x, z))
            {
                ClearTempFields();
                return false;
            }

            return true;
        }

        public bool HasBarrier(Vector3 forward, int startX, int startZ)
        {
            int directionX = (int)forward.normalized.x;
            int directionZ = (int)forward.normalized.z;

            int x = startX;
            int z = startZ;
            _barrierCheckedList.Add(new Vector3(x, 0, z));

            while (x < _matrixSize - 1 && z < _matrixSize - 1 && x > 0 && z > 0)
            {
                x += directionX;
                z += directionZ;

                MatrixCell checkedCell = _matrix[x, z];

                if (checkedCell.State == MatrixCellStates.Free)
                    continue;

                if (checkedCell.Direction == -forward)
                    return true;

                if (_barrierCheckedList.Any(coord => coord.x == x && coord.z == z))
                    return true;

                if (checkedCell.Direction == forward)
                    continue;

                if (HasBarrier(checkedCell.Direction, x, z))
                    return true;
            }

            return false;
        }

        public bool MarixCellIsEmpty(float startX, float startZ)
        {
            int x = (int)Mathf.Round(startX) + MatrixCenter;
            int z = (int)Mathf.Round(startZ) + MatrixCenter;

            return _matrix[x, z].State != MatrixCellStates.Taken;
        }

        public void FillMatrix()
        {
            for (int x = 0; x < _matrixSize; x++)
            {
                for (int z = 0; z < _matrixSize; z++)
                {
                    _matrix[x, z].TryTake();
                }
            }
        }

        public void ClearTempFields()
        {
            for (int x = 0; x < _matrixSize; x++)
            {
                for (int z = 0; z < _matrixSize; z++)
                {
                    _matrix[x, z].ClearMarked();
                }
            }
        }

        public bool CanLeaveParking(ArrowCar car)
        {
            int x = car.ForwardX;
            int z = car.ForwardZ;

            while (x < _matrixSize && z < _matrixSize && x >= 0 && z >= 0)
            {
                if (_matrix[x, z].State != MatrixCellStates.Free)
                {
                    return false;
                }

                x += car.DirectionX;
                z += car.DirectionZ;
            }

            return true;
        }

        public void ClearCarPlace(ArrowCar car)
        {
            int x = car.X;
            int z = car.Z;

            for (int i = 0; i < car.Type.Length; i++)
            {
                _matrix[x, z].Clear();

                x += car.DirectionX;
                z += car.DirectionZ;
            }
        }
    }
}