using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YG;

[Serializable]
public class LevelInfo
{
    public int LevelNumber{get;private set;}
    public BiomType Biom { get; private set; }
    public int Points { get; private set; }
    public int StarsCount { get; private set; }
    public bool IsEnabled { get; private set; }
    public int UserPoints { get; private set; }

    private int _biomStep = 3;
    private string _leaderboardName = "AnimalDeliveryLeaderboard";

    public LevelInfo(int levelNumber, int points, int userPoints)
    {
        LevelNumber = levelNumber;
        Points = points;
        UserPoints = userPoints;

        Biom = GetBiom();
        StarsCount = GetStarCount(UserPoints);
        IsEnabled = GetEnabled();
    }

    public void TryUpdateResult(int userPoints)
    {
        if (UserPoints > userPoints)
            return;

        UserPoints = userPoints;
        StarsCount = GetStarCount(UserPoints);

        int totalScore = YandexGame.savesData.UserPoints.Sum();
        Save();
        SaveLeaderboardScores();
        YandexGame.savesData.UpdateCurrentLevel(this);
    }

    public void ActivateCurrentLevel()
    {
        IsEnabled = true;
        YandexGame.savesData.UpdateCurrentLevel(this);
    }

    private void Save()
    {
        if (LevelNumber < 1)
            return;

        YandexGame.savesData.UserPoints[LevelNumber - 1] = UserPoints;
        YandexGame.SaveProgress();
    }

    private void SaveLeaderboardScores()
    {
        int totalScore = YandexGame.savesData.UserPoints.Sum();
        Debug.Log($"SAVE totalScore {totalScore}");
        YandexGame.NewLeaderboardScores(_leaderboardName, totalScore);
    }

    private BiomType GetBiom()
    {
       int groupNumber = (LevelNumber - 1) / _biomStep;

        var biomCount = Enum.GetNames(typeof(BiomType)).Length;

        int biomNumber = groupNumber % biomCount;
        return (BiomType)biomNumber;        
    }

    private bool GetEnabled()
    {
        if (LevelNumber < 1)
            return false;

        if (LevelNumber == 1)
            return true;

        LevelInfo lastLevel = YandexGame.savesData.Levels.FirstOrDefault(level => level.LevelNumber == LevelNumber - 1);

        return lastLevel.StarsCount > 1;
    }

    public int GetStarCount(int userPoints)
    {
        int maxPercent = 100;
        float currentPercent = Mathf.Round((float)userPoints * maxPercent / (float)Points);

        if (currentPercent < 50)
            return 0;
        else if (currentPercent < 70)
            return 1;
        else if (currentPercent < 90)
            return 2;
        else
            return 3;
    }
}