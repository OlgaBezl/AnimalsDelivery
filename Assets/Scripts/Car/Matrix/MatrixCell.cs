using UnityEngine;

public class MatrixCell
{
    public MatrixCellStates State { get; private set; }
    public Vector3 Direction { get; private set; }
    //public int X;
    //public int Y;
    public enum MatrixCellStates
    {
        Free = 0,
        Marked = 1,
        Taken = 2
    }

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
