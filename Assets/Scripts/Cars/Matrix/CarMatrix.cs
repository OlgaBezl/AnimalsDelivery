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
        private List<Vector3Int> _barrierCheckedList;
        private Vector3Int _matrixCenterOffset;

        public int MatrixCenter { get; private set; }
        
        public void Load()
        {
            _matrix = new MatrixCell[_matrixSize, _matrixSize];
            _barrierCheckedList = new List<Vector3Int>();

            for (int x = 0; x < _matrixSize; x++)
            {
                for (int z = 0; z < _matrixSize; z++)
                {
                    _matrix[x, z] = new MatrixCell();
                }
            }

            MatrixCenter = _matrixSize / 2;
            _matrixCenterOffset = new Vector3Int(MatrixCenter, MatrixCenter, MatrixCenter);
        }

        public void Unload()
        {
            _matrix = null;

            for (int i = 0; i < _matrixContainer.childCount; i++)
            {
                Destroy(_matrixContainer.GetChild(i).gameObject);
            }
        }

        public bool CanGenerateCarByPosition(int length, Vector3Int forward, Vector3Int startPosition)
        {
            Vector3Int position = startPosition + _matrixCenterOffset;

            for (int offset = 0; offset < length; offset++)
            {
                if (_matrix[position.x, position.z].TryMark(forward) == false)
                {
                    ClearTempFields();
                    return false;
                }

                position += forward;
            }

            _barrierCheckedList.Clear();
            position -= forward;

            if (HasBarrier(forward, new Vector3Int(position.x, 0, position.z)))
            {
                ClearTempFields();
                return false;
            }

            return true;
        }

        public bool HasBarrier(Vector3Int forward, Vector3Int startPosition)
        {
            Vector3Int position = startPosition;
            _barrierCheckedList.Add(position);

            while (position.x < _matrixSize - 1 &&
                   position.z < _matrixSize - 1 &&
                   position.x > 0 &&
                   position.z > 0)
            {
                position += forward;
                MatrixCell checkedCell = _matrix[position.x, position.z];

                if (checkedCell.State == MatrixCellStates.Free)
                    continue;

                if (checkedCell.Direction == -forward)
                    return true;

                if (_barrierCheckedList.Any(coord => coord.x == position.x && coord.z == position.z))
                    return true;

                if (checkedCell.Direction == forward)
                    continue;

                if (HasBarrier(checkedCell.Direction, position))
                    return true;
            }

            return false;
        }

        public bool CheckIfMarixCellIsEmpty(Vector3Int position)
        {
            int x = position.x + MatrixCenter;
            int z = position.z + MatrixCenter;

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

        public bool CkeckIfCanLeaveParking(ArrowCar car)
        {
            Vector2Int forward = car.Forward;

            while (forward.x < _matrixSize &&
                   forward.y < _matrixSize &&
                   forward.x >= 0 &&
                   forward.y >= 0)
            {
                if (_matrix[forward.x, forward.y].State != MatrixCellStates.Free)
                {
                    return false;
                }

                forward += car.Direction;
            }

            return true;
        }

        public void ClearCarPlace(ArrowCar car)
        {
            Vector2Int location = car.Location;

            for (int i = 0; i < car.Specification.Length; i++)
            {
                _matrix[location.x, location.y].Clear();
                location += car.Direction;
            }
        }
    }
}