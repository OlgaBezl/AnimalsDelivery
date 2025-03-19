using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Buttons
{
    [RequireComponent(typeof(Button))]
    public abstract class DefaultButton : MonoBehaviour
    {
        protected Button Button;

        protected virtual void Awake()
        {
            Button = GetComponent<Button>();
            Button.onClick.AddListener(OnClick);
        }

        protected virtual void OnClick()
        {
        }

        private void OnDestroy()
        {
            Button.onClick.RemoveListener(OnClick);
        }
    }
}