using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class DefaultButton : MonoBehaviour
{
    protected Button Button;

    protected void Awake()
    {
        Button = GetComponent<Button>();
        Button.onClick.AddListener(OnClick);
    }

    protected virtual void OnClick()
    {
        //Button.onClick.RemoveListener(OnStartClick);
    }
}
