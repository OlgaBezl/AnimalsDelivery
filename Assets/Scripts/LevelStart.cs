using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YG;
using Scripts.UI;
using Scripts.UI.Buttons;
using Scripts.Progress;

namespace Scripts
{
    public class LevelStart : MonoBehaviour
    {
        [SerializeField] private CommonStartButton[] _startButtons;
        [SerializeField] private Level _level;
        [SerializeField] private GameCanvas _gameCanvas;

        private LevelButton[] _additionalStartButtons;

        private void OnValidate()
        {
            if (_startButtons == null)
                throw new NullReferenceException(nameof(_startButtons));

            if (_level == null)
                throw new NullReferenceException(nameof(_level));

            if (_gameCanvas == null)
                throw new NullReferenceException(nameof(_gameCanvas));
        }

        private void Start()
        {
            YandexGame.savesData.Load();

            foreach (CommonStartButton buttom in _startButtons)
            {
                buttom.Click += OnStartClick;
            }
        }

        public void UpdateAdditionalStartLevelButtons(IReadOnlyList<LevelButton> buttons)
        {
            if (_additionalStartButtons != null)
            {
                foreach (LevelButton button in _additionalStartButtons)
                {
                    button.Click -= OnStartClick;
                }

                _additionalStartButtons = null;
            }

            if (buttons == null)
            {
                return;
            }

            _additionalStartButtons = buttons.ToArray();

            foreach (LevelButton button in _additionalStartButtons)
            {
                button.Click += OnStartClick;
            }
        }

        private void OnStartClick(LevelInfo levelInfo)
        {
            YandexGame.savesData.MoveToAnotherLevel(levelInfo);
            _level.Load(levelInfo, true);
        }
    }
}