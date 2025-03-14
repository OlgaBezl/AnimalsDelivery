using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class CommonStartButton : DefaultButton
{
    [field: SerializeField] public UIPanel Panel { get;private set; }

    public event Action<LevelInfo> Click;

    protected override void OnClick()
    {
        Click?.Invoke(YandexGame.savesData.CurrentLevel);
    }
}