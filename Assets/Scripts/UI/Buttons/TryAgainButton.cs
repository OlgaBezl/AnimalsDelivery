using System;
using UnityEngine;
using YG;
using Scripts.Helpers;
using Scripts.UI.Panels;

namespace Scripts.UI.Buttons
{
    public class TryAgainButton : DefaultButton
    {
        [SerializeField] private UIPanel _panel;

        private int _points;

        public event Action<int> Click;

        private void OnValidate()
        {
            if (_panel == null)
                throw new NullReferenceException(nameof(_panel));
        }

        public void Load(int points)
        {
            _points = points;
        }

        protected override void OnClick()
        {
            YandexGame.RewVideoShow((int)RewardType.ContinueLevel);
            YandexGame.RewardVideoEvent += TryAgain;
        }

        private void TryAgain(int id)
        {
            if (id != (int)RewardType.ContinueLevel)
                return;

            YandexGame.RewardVideoEvent -= TryAgain;

            base.OnClick();
            Click?.Invoke(_points);
            _panel.Hide();
            Time.timeScale = 1;
        }
    }
}