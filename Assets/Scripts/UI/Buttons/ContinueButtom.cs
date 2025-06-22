using System;
using Scripts.UI.Panels;
using UnityEngine;
using YG;

namespace Scripts.UI.Buttons
{
    public class ContinueButtom : DefaultButton
    {
        [SerializeField] private UIPanel _panel;

        private void OnValidate()
        {
            if (_panel == null)
                throw new NullReferenceException(nameof(_panel));
        }

        protected override void OnClick()
        {
            base.OnClick();

            YandexGame.GameplayStart();
            _panel.Hide();
            Time.timeScale = 1;
        }
    }
}