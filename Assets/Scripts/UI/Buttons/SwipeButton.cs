using UnityEngine;
using Scripts.UI.Panels;

namespace Scripts.UI.Buttons
{
    public class SwipeButton : DefaultButton
    {
        [SerializeField] private LevelPanel _levelPanel;
        [SerializeField] private LoadPageType _loadPageType;

        private void OnValidate()
        {
            if (_levelPanel == null)
                throw new System.NullReferenceException(nameof(_levelPanel));
        }

        protected override void OnClick()
        {
            base.OnClick();

            _levelPanel.Clear();
            _levelPanel.SelectPage(_loadPageType);
            _levelPanel.Fill();
        }
    }
}