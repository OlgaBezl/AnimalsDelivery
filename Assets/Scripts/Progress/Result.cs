using Agava.YandexGames;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YG;

public class Result : MonoBehaviour
{
    [SerializeField] private SecondParking _parking;

    private int _currentPercent;
    private int _totalSeatsCount;
    private int _occupiedSeatsCount;
    private int _lastSeatsCount;
    private int _maxPercent = 100;
    private CarList _carList;

    public event Action<int> CurrentPercentChanged;
    public event Action<int> LevelFinished;

    private void OnValidate()
    {
        if (_parking == null)
            throw new NullReferenceException(nameof(_parking));
    }

    private void OnEnable()
    {
        _parking.CarLeft += CarLeftParking;
    }

    private void OnDisable()
    {
        _parking.CarLeft -= CarLeftParking;
    }

    public void StartLevel(int points, CarList carList)
    {
        _lastSeatsCount = points;
        _totalSeatsCount = points;
        _occupiedSeatsCount = points;
        _currentPercent = _maxPercent;
        _carList = carList;
        _carList.LevelFinished += UpdateLevelResult;

        CurrentPercentChanged?.Invoke(_currentPercent);
    }

    public void AddTotalPoints()
    {
        _lastSeatsCount++;
        _totalSeatsCount++;
        _occupiedSeatsCount++;
    }

    public void FinishLevel()
    {
        _lastSeatsCount = 0;
        _totalSeatsCount = 0;
        _occupiedSeatsCount = 0;
        _currentPercent = 0;
        _carList.LevelFinished -= UpdateLevelResult;
    }

    private void CarLeftParking(CarWithSeats car)
    {
        _lastSeatsCount -= car.Type.SeatsCount;
        _occupiedSeatsCount -= car.FreeSeatsCount;
        _currentPercent = (int)Mathf.Round((float)_occupiedSeatsCount * _maxPercent / (float)_totalSeatsCount);

        CurrentPercentChanged?.Invoke(_currentPercent);

        if(_lastSeatsCount <= 0)
        {
            UpdateLevelResult();
        }
    }

    private void UpdateLevelResult()
    {
        YandexGame.savesData.CurrentLevel.TryUpdateResult(_occupiedSeatsCount);
        int starsCount = YandexGame.savesData.CurrentLevel.GetStarCount(_occupiedSeatsCount);

        if (YandexGame.savesData.CurrentLevel.StarsCount > 1)
        {
            YandexGame.savesData.MoveToNextLevel();
        }

        LevelFinished?.Invoke(starsCount);
    }
}