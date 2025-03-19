using System.Collections.Generic;
using System.Linq;
using Scripts.Progress;
using Scripts.UI.Buttons;
using UnityEngine;
using YG;

namespace Scripts.UI.Panels
{
    public class LevelPanel : UIPanel
    {
        [SerializeField] private LevelButton _levelButtonPrefab;
        [SerializeField] private int _buttonCountPerPage = 3;
        [SerializeField] private Transform _container;
        [SerializeField] private LevelStart _levelStart;
        [SerializeField] private SwipeButton _leftSwipeButton;
        [SerializeField] private SwipeButton _rightSwipeButton;

        private int _startValue = 1;
        private int _currentPage = 1;
        private int _minPage = 1;
        private int _maxPage = 1;
        private List<LevelButton> _buttons;

        private void OnValidate()
        {
            if (_levelButtonPrefab == null)
                throw new System.NullReferenceException(nameof(_levelButtonPrefab));

            if (_container == null)
                throw new System.NullReferenceException(nameof(_container));

            if (_buttonCountPerPage <= 0)
                throw new System.ArgumentOutOfRangeException(nameof(_buttonCountPerPage));

            if (_levelStart == null)
                throw new System.NullReferenceException(nameof(_levelStart));
        }

        public override void Open()
        {
            SelectPage(LoadPageType.SelectedLevel);
            IsOpened += Fill;
            base.Open();
        }

        public override void Hide()
        {
            IsHidden += Clear;
            base.Hide();
        }

        public void SelectPage(LoadPageType loadType)
        {
            if (loadType == LoadPageType.SelectedLevel)
            {
                _maxPage = (int)Mathf.Ceil((float)SavesYG.MaxLevel / (float)_buttonCountPerPage);
                float levelNumber = (float)YandexGame.savesData.CurrentLevel.LevelNumber;
                _currentPage = (int)Mathf.Ceil(levelNumber / (float)_buttonCountPerPage);
            }
            else if (loadType == LoadPageType.NextPage)
            {
                _currentPage++;
            }
            else if (loadType == LoadPageType.PreviousPage)
            {
                _currentPage--;
            }

            _startValue = (_currentPage - 1) * _buttonCountPerPage + 1;

            _leftSwipeButton.gameObject.SetActive(_currentPage > _minPage);
            _rightSwipeButton.gameObject.SetActive(_currentPage < _maxPage);
        }

        public void Fill()
        {
            IsOpened -= Fill;

            List<LevelInfo> levels = YandexGame.savesData.Levels.ToList();
            int endValue = Mathf.Min(_startValue + _buttonCountPerPage - 1, levels.Count);
            int selectedLevel = YandexGame.savesData.CurrentLevel.LevelNumber;
            _buttons = new List<LevelButton>();

            for (int number = _startValue; number <= endValue; number++)
            {
                LevelInfo levelInfo = levels.FirstOrDefault(level => level.LevelNumber == number);
                LevelButton levelButton = Instantiate(_levelButtonPrefab, _container);
                levelButton.Initialize(this, levelInfo);
                levelButton.Show(selectedLevel == number);
                _buttons.Add(levelButton);
            }

            _levelStart.UpdateAdditionalStartLevelButtons(_buttons);
        }

        public void Clear()
        {
            IsHidden -= Clear;

            foreach (LevelButton button in _buttons)
            {
                Destroy(button.gameObject);
            }
        }
    }
}