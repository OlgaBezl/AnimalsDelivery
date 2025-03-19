using System;
using UnityEngine;
using YG;
using Scripts.UI.Panels;
using Scripts.Progress;

namespace Scripts.UI.Buttons
{
    public class CommonStartButton : DefaultButton
    {
        [field: SerializeField] public UIPanel Panel { get; private set; }

        public event Action<LevelInfo> Click;

        protected override void OnClick()
        {
            Click?.Invoke(YandexGame.savesData.CurrentLevel);
        }
    }
}