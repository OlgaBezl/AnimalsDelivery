using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YG;

public class LevelsInfoGenerator : MonoBehaviour
{
    [SerializeField] private int _stepPoints = 3;
    [SerializeField] private int _minPoints = 10;
    [SerializeField] private int _maxPoints = 100;
    [SerializeField] private int _maxlevel = 30;
    [SerializeField] private Result _result;

    private List<LevelInfo> _levels;

    public int LevelCount => _levels == null ? 0 : _levels.Count;

    private void OnValidate()
    {
        if (_result == null)
            throw new System.NullReferenceException(nameof(_result));
    }

    //private void Awake()
    //{
    //    if(YandexGame.savesData.Levels?.Count > 0)
    //    {
    //        _levels = YandexGame.savesData.Levels;
    //    }
    //    else
    //    {
    //        GenereatLevelList();
    //    }


    //    LevelInfo currentLevel = _levels.Where(level => level.IsEnabled).OrderBy(level => level.LevelNumber).Last();
    //    CurrentProgress.MoveToAnotherLevel(currentLevel);

    //}

    //public void GenereatLevelList()
    //{
    //    _levels = new List<LevelInfo>();
    //    int points = _minPoints;

    //    for (int number = 1; number <= MaxLevel; number++)
    //    {
    //        _levels.Add(new LevelInfo(number, BiomType.Autumn, points));
    //        points += _stepPoints;
    //    }
    //}


}
