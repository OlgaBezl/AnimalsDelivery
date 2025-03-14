
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace YG
{
    [Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        // Тестовые сохранения для демо сцены
        // Можно удалить этот код, но тогда удалите и демо (папка Example)
        public int money = 1;                       // Можно задать полям значения по умолчанию
        public string newPlayerName = "Hello!";
        public bool[] openLevels = new bool[3];

        public LevelInfo CurrentLevel { get; private set; }

        public IReadOnlyList<LevelInfo> Levels => _levels;
        private List<LevelInfo> _levels = new List<LevelInfo>();

        public int[] UserPoints = new int[MaxLevel];

        // Ваши сохранения

        // ...

        // Поля (сохранения) можно удалять и создавать новые. При обновлении игры сохранения ломаться не должны
        private int _stepPoints = 2;
        private int _minPoints = 10;
        public const int MaxLevel = 45;

        // Вы можете выполнить какие то действия при загрузке сохранений
        public SavesYG()
        {
        }

        public void MoveToAnotherLevel(LevelInfo levelInfo)
        {
            if (levelInfo == null)
                throw new ArgumentNullException(nameof(levelInfo));

            levelInfo.ActivateCurrentLevel();
        }

        public void MoveToNextLevel()
        {
            LevelInfo nextLevel = YandexGame.savesData.Levels.FirstOrDefault(level => level.LevelNumber == CurrentLevel.LevelNumber + 1);

            if (nextLevel != null)
            {
                nextLevel.ActivateCurrentLevel();
            }
        }

        public void Load()
        {
            if (UserPoints.Count() < MaxLevel)
            {
                int[] userPointsTemp = new int[MaxLevel];

                for (int i = 0; i < UserPoints.Count(); i++)
                {
                    userPointsTemp[i] = UserPoints[i];
                }

                UserPoints = userPointsTemp;
            }

            _levels = new List<LevelInfo>();
            int points = _minPoints;

            for (int number = 1; number <= MaxLevel; number++)
            {
                _levels.Add(new LevelInfo(number, points, YandexGame.savesData.UserPoints[number - 1]));
                points += _stepPoints;
            }

            YandexGame.savesData.CurrentLevel = _levels.Where(level => level.IsEnabled).OrderBy(level => level.LevelNumber).Last();
        }

        public void UpdateCurrentLevel(LevelInfo levelInfo)
        {
            var level = _levels.FirstOrDefault(lvl => lvl.LevelNumber == levelInfo.LevelNumber);
            level = levelInfo;
            CurrentLevel = levelInfo;
        }
    }
}
