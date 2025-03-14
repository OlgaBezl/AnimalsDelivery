using System;
using UnityEngine;

public class ChangePanelButtom : DefaultButton
{
    [SerializeField] private UIPanel _currentPanel;
    [SerializeField] private UIPanel _secondPanel;

    private void OnValidate()
    {
        if (_currentPanel == null)
            throw new NullReferenceException(nameof(_currentPanel));
    }

    public void ChangeSecondPanel(UIPanel secondPanel)
    {
        if (secondPanel == null)
            return;

        _secondPanel = secondPanel;
    }

    protected override void OnClick()
    {
        base.OnClick();

        _currentPanel.IsHidden += OpenMainMenuPanel;
        _currentPanel.Hide();
    }

    private void OpenMainMenuPanel()
    {
        _currentPanel.IsHidden -= OpenMainMenuPanel;

        _secondPanel.SetReturnButton(_currentPanel);

        if (_secondPanel == null)
            return;

        _secondPanel.Open();
    }
}
