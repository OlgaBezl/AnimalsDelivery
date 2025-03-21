using System;
using UnityEngine;
using TMPro;
using YG;
using Scripts.Cars.Generators;
using Scripts.Helpers;

namespace Scripts.UI.Buttons
{
    public class AddCarButton : DefaultButton
    {
        [SerializeField] private int _startTryCount = 1;
        [SerializeField] private AdditinalCarSpawner _additinalCarSpawner;
        [SerializeField] private GameObject _playIcon;
        [SerializeField] private TextMeshProUGUI _text;

        private int _tryCount;

        private void OnValidate()
        {
            if (_additinalCarSpawner == null)
                throw new NullReferenceException(nameof(_additinalCarSpawner));

            if (_playIcon == null)
                throw new NullReferenceException(nameof(_playIcon));

            if (_text == null)
                throw new NullReferenceException(nameof(_text));
        }

        protected override void Awake()
        {
            base.Awake();
            _tryCount = _startTryCount;
            RefreshDisplay();
        }

        protected override void OnClick()
        {
            if (_tryCount > 0)
            {
                _tryCount--;
                TryAgain();
            }
            else
            {
                YandexGame.RewVideoShow((int)RewardType.AddCar);
                YandexGame.RewardVideoEvent += TryAgainAfterReward;
            }
        }

        private void TryAgainAfterReward(int id)
        {
            if (id != (int)RewardType.AddCar)
                return;

            YandexGame.RewardVideoEvent -= TryAgainAfterReward;
            TryAgain();
        }

        private void TryAgain()
        {
            _additinalCarSpawner.SpawnCar();

            RefreshDisplay();
            base.OnClick();
        }

        private void RefreshDisplay()
        {
            if (_tryCount > 0)
            {
                _text.text = _tryCount.ToString();
                _text.gameObject.SetActive(true);
                _playIcon.SetActive(false);
            }
            else
            {
                _text.gameObject.SetActive(false);
                _playIcon.SetActive(true);
            }
        }
    }
}