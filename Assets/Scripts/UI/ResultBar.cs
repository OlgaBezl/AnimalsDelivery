using UnityEngine;
using UnityEngine.UI;
using Scripts.Progress;

namespace Scripts.UI
{
    public class ResultBar : MonoBehaviour
    {
        [SerializeField] private Result _result;
        [SerializeField] private TMPro.TextMeshProUGUI _text;
        [SerializeField] private Slider _slider;

        private void OnValidate()
        {
            if (_result == null)
                throw new System.NullReferenceException(nameof(_result));

            if (_text == null)
                throw new System.NullReferenceException(nameof(_text));

            if (_slider == null)
                throw new System.NullReferenceException(nameof(_slider));
        }

        private void OnEnable()
        {
            _result.CurrentPercentChanged += ResultChanged;
        }

        private void OnDisable()
        {
            _result.CurrentPercentChanged -= ResultChanged;
        }

        private void ResultChanged(int percent)
        {
            _slider.value = percent;
            _text.text = $"{percent}%";
        }
    }
}