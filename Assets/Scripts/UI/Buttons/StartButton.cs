using System;
using UnityEngine;
using YG;

public class StartButton : CommonStartButton
{
    protected override void OnClick()
    {
        base.OnClick();
        Panel.Hide();
        Time.timeScale = 1;
    }
}
