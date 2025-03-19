using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class Star : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private float _smoothDescreaseDuration = 0.5f;

        private void OnValidate()
        {
            if (_image == null)
                throw new NullReferenceException(nameof(_image));
        }

        public void Clear()
        {
            _image.fillAmount = 0;
        }

        public void Fill()
        {
            StartCoroutine(FillSmoothly());
        }

        private IEnumerator FillSmoothly()
        {
            float elapsedTime = 0f;
            float previousValue = 0f;
            float target = 1f;

            while (elapsedTime < _smoothDescreaseDuration)
            {
                elapsedTime += Time.deltaTime;
                float normalizedPosition = elapsedTime / _smoothDescreaseDuration;
                float intermediateValue = Mathf.Lerp(previousValue, target, normalizedPosition);
                _image.fillAmount = intermediateValue;
                yield return null;
            }
        }
    }
}