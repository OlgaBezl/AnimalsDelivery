using Agava.YandexGames;
using System;
using UnityEngine;
using YG;

public class Level : MonoBehaviour
{
    //[SerializeField] private int points;
    [SerializeField] private PuzzleGenerator _carGenerator;
    [SerializeField] private CarQueue _carQueue;
    [SerializeField] private Landing _landing;
    [SerializeField] private Result _result;
    [SerializeField] private CarCololrsList _carCololrsList;
    [SerializeField] private VisualScene _visualScene;

    bool _isFirstLoad;
    bool _isActive;
    private CarList _carList;

    private void OnValidate()
    {
        //if (points <= 0)
        //    throw new ArgumentOutOfRangeException(nameof(points));

        if (_carGenerator == null)
            throw new NullReferenceException(nameof(_carGenerator));

        if (_carQueue == null)
            throw new NullReferenceException(nameof(_carQueue));

        if (_landing == null)
            throw new NullReferenceException(nameof(_landing));

        if (_result == null)
            throw new NullReferenceException(nameof(_result));

        if (_carCololrsList == null)
            throw new NullReferenceException(nameof(_carCololrsList));
    }

    public void Load(LevelInfo levelInfo, bool withAdd)
    {
        if (_isFirstLoad)
            YandexGamesSdk.Initialize();

        if (_isActive)
            Unload();

        if (!_isFirstLoad && withAdd)
            YandexGame.FullscreenShow();

        _carList = new CarList();
        _carList.Initialize();

        _carCololrsList.StartLevel(_carList);
        _carGenerator.StartLevel(levelInfo, _carList);
        _carQueue.StartLevel();
        _landing.StartLevel(_carList);
        _result.StartLevel(levelInfo.Points, _carList);
        _visualScene.StartLevel(levelInfo.Biom);

        YandexGame.GameplayStart();
        _isActive = true;
    }

    public void Unload()
    {
        YandexGame.GameplayStop();
        _carGenerator.FinishLevel();
        _carQueue.FinishLevel();
        _landing.FinishLevel();
        _isActive = false;
    }
}
