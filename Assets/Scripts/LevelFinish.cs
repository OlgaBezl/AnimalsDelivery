using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class LevelFinish : MonoBehaviour
{
    [SerializeField] private Result _result;
    [SerializeField] private Level _level;
    [SerializeField] private FinishPanel _goodFinishPanel;
    [SerializeField] private FinishPanel _badFinishPanel;
    [SerializeField] private GameCanvas _gameCanvas;

    private void OnValidate()
    {
        if (_result == null)
            throw new NullReferenceException(nameof(_result));

        if (_level == null)
            throw new NullReferenceException(nameof(_level));

        if (_goodFinishPanel == null)
            throw new NullReferenceException(nameof(_goodFinishPanel));

        if (_badFinishPanel == null)
            throw new NullReferenceException(nameof(_badFinishPanel));

        if (_gameCanvas == null)
            throw new NullReferenceException(nameof(_gameCanvas));
    }

    private void OnEnable()
    {
        _result.LevelFinished += Finish;
    }

    private void OnDisable()
    {
        _result.LevelFinished -= Finish;
    }

    private void Finish(int fillStarsCount)
    {
        //_gameCanvas.gameObject.SetActive(false);
        _level.Unload();

        if(fillStarsCount > 1 )
        {
            _goodFinishPanel.Open(fillStarsCount);
        }
        else
        {
            _badFinishPanel.Open(fillStarsCount);
        }
    }
}
