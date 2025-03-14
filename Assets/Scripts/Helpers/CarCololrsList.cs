using System;
using System.Collections.Generic;
using UnityEngine;

public class CarCololrsList : MonoBehaviour
{
    [SerializeField] private FirstParking _firstParking;
    [SerializeField] private CarQueue _carQueue;
    [SerializeField] private SecondParking _secondParking;

    private CarList _carList;

    private void OnValidate()
    {
        if (_firstParking == null)
            throw new NullReferenceException(nameof(_firstParking));

        if (_carQueue == null)
            throw new NullReferenceException(nameof(_carQueue));

        if (_secondParking == null)
            throw new NullReferenceException(nameof(_secondParking));
    }

    public void StartLevel(CarList list)
    {
        _carList = list;
    }

    public int GetFreeRandomColorIndex(List<Tuple<int, int>> exceptColorTuples)
    {
        return _carList.GetRandomIndex();
    }

}
