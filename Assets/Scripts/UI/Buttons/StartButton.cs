using UnityEngine;

namespace Scripts.UI.Buttons
{
    public class StartButton : CommonStartButton
    {
        protected override void OnClick()
        {
            base.OnClick();
            Panel.Hide();
            Time.timeScale = 1;
        }
    }
}