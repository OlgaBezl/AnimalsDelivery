using System;
using Scripts.Progress;
using Scripts.UI.Panels;
using UnityEngine;
using YG;

namespace Scripts.UI.Buttons
{
    public class CommonStartButton : DefaultButton
    {
        public event Action<LevelInfo> Click;

        [field: SerializeField] public UIPanel Panel { get; private set; }

        protected override void OnClick()
        {
            Click?.Invoke(YandexGame.savesData.CurrentLevel);
        }
    }
}