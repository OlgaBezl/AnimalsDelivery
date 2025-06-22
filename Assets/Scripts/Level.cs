using System;
using Agava.YandexGames;
using Scripts.Cars.Containers;
using Scripts.Cars.Generators;
using Scripts.Cars.Model;
using Scripts.Enviroment;
using Scripts.Helpers;
using Scripts.Progress;
using Scripts.Queues;
using UnityEngine;
using YG;

namespace Scripts
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private PuzzleGenerator _carGenerator;
        [SerializeField] private CarQueue _carQueue;
        [SerializeField] private Landing _landing;
        [SerializeField] private Result _result;
        [SerializeField] private CarCololrsList _carCololrsList;
        [SerializeField] private VisualScene _visualScene;

        private bool _isFirstLoad;
        private bool _isActive;
        private CarList _carList;

        private void OnValidate()
        {
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
            _carGenerator.Unload();
            _carQueue.Unload();
            _landing.Unload();
            _isActive = false;
        }
    }
}