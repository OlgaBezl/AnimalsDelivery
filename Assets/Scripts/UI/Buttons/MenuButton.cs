using Scripts.UI.Panels;
using UnityEngine;
using YG;

namespace Scripts.UI.Buttons
{
    public class MenuButton : DefaultButton
    {
        [SerializeField] private UIPanel _menuPanel;

        private void OnValidate()
        {
            if (_menuPanel == null)
                throw new System.NullReferenceException(nameof(_menuPanel));
        }

        protected override void OnClick()
        {
            base.OnClick();

            YandexGame.GameplayStop();
            _menuPanel.Open();
            Time.timeScale = 0;
        }
    }
}