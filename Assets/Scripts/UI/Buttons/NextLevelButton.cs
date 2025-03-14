using System;
using UnityEngine;
using YG;

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
