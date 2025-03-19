using System;
using UnityEngine;

namespace Scripts.UI
{
    public class StarInLevelButton : MonoBehaviour
    {
        [SerializeField] private int _number;
        [SerializeField] private GameObject _fill;
        [SerializeField] private GameObject _border;
        [SerializeField] private GameObject _point;

        private void OnValidate()
        {
            if (_fill == null)
                throw new ArgumentNullException(nameof(_fill));

            if (_border == null)
                throw new ArgumentNullException(nameof(_border));

            if (_point == null)
                throw new ArgumentNullException(nameof(_point));
        }

        public void Show(int starCount, bool enable)
        {
            if (enable)
            {
                _fill.SetActive(starCount >= _number);
                _border.SetActive(true);
                _point.SetActive(false);
            }
            else
            {
                _fill.SetActive(false);
                _border.SetActive(false);
                _point.SetActive(true);
            }
        }
    }
}