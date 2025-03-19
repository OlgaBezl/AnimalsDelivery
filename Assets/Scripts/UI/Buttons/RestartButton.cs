using UnityEngine;

namespace Scripts.UI.Buttons
{
    public class RestartButton : CommonStartButton
    {
        [SerializeField] private Level _level;

        protected override void OnClick()
        {
            Time.timeScale = 0;
            _level.Unload();

            base.OnClick();

            Panel.Hide();
            Time.timeScale = 1;
        }
    }
}