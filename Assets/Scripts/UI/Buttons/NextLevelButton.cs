using System;
using UnityEngine;
using YG;
using Scripts.UI.Panels;
using Scripts.Progress;

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