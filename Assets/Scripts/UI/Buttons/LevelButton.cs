using System;
using System.Linq;
using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Scripts.UI.Panels;
using Scripts.Progress;

namespace Scripts.UI.Buttons
{
    public class LevelButton : DefaultButton
    {
        [SerializeField] private StarInLevelButton[] _stars;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _image;
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _selectColor;

        private UIPanel _panel;
        private LevelInfo _levelInfo;

        public event Action<LevelInfo> Click;

        protected override void OnClick()
        {
            base.OnClick();

            Click?.Invoke(_levelInfo);
            _panel.Hide();
            Time.timeScale = 1;
        }

        private void OnValidate()
        {
            if (_stars == null || _stars.Count() == 0)
                throw new ArgumentNullException(nameof(_stars));

            if (_text == null)
                throw new ArgumentNullException(nameof(_text));
        }

        public void Initialize(UIPanel panel, LevelInfo levelInfo)
        {
            _panel = panel;
            _levelInfo = levelInfo;
            _text.text = levelInfo.LevelNumber.ToString();
            Button.interactable = levelInfo.IsEnabled;

            foreach (StarInLevelButton star in _stars)
            {
                star.Show(levelInfo.StarsCount, levelInfo.IsEnabled);
            }
        }

        public void Show(bool isSelected)
        {
            gameObject.SetActive(true);
            _image.color = isSelected ? _selectColor : _defaultColor;

            DOTween.Sequence().
                Append(transform.DOScale(Vector3.zero, 0)).
                Append(transform.DOScale(Vector3.one * 1.25f, 0.4f)).
                Append(transform.DOScale(Vector3.one, 0.2f)).
                SetUpdate(true);
        }
    }
}