using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LevelPanel;

public class ShowRewardPanel : UIPanel
{
    [SerializeField] private ShowRewardButton _showRewardButton;
    public event Action<int> IsShown;

    private void OnValidate()
    {
        if (_showRewardButton == null)
            throw new NullReferenceException(nameof(_showRewardButton));
    }

    public override void Open()
    {
        _showRewardButton.IsShown += ShowAndHide;
        base.Open();
    }

    public override void Hide()
    {
        _showRewardButton.IsShown -= ShowAndHide;
        base.Hide();
    }

    private void ShowAndHide(int id)
    {
        IsShown?.Invoke(id);
        Hide();
    }
}
