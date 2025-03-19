using UnityEngine;

namespace Scripts.Cars.Matrix
{
    public class MatrixCell
    {
        public MatrixCellStates State { get; private set; }
        public Vector3 Direction { get; private set; }

        public bool TryMark(Vector3 direction)
        {
            if (State == MatrixCellStates.Taken)
            {
                return false;
            }

            State = MatrixCellStates.Marked;
            Direction = direction;
            return true;
        }

        public bool TryTake()
        {
            if (State == MatrixCellStates.Marked)
            {
                State = MatrixCellStates.Taken;
                return true;
            }

            return false;
        }

        public void ClearMarked()
        {
            if (State == MatrixCellStates.Marked)
                Clear();
        }

        public void Clear()
        {
            State = MatrixCellStates.Free;
            Direction = Vector3.zero;
        }
    }
}