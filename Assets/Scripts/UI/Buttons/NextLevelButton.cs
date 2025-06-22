using System;
using Scripts.Progress;
using Scripts.UI.Panels;
using UnityEngine;
using YG;

namespace Scripts.UI.Buttons
{
    public class NextLevelButton : DefaultButton
    {
        [SerializeField] private UIPanel _panel;

        public event Action<LevelInfo> Click;

        protected override void OnClick()
        {
            base.OnClick();

            Click?.Invoke(YandexGame.savesData.CurrentLevel);
            _panel.Hide();
            Time.timeScale = 1;
        }
    }
}