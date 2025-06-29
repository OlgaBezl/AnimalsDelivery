using Scripts.UI.Panels;
using UnityEngine;

namespace Scripts.Levels
{
    public class GameRoot : MonoBehaviour
    {
        [SerializeField] private UIPanel _mainMenu;

        private void OnValidate()
        {
            if (_mainMenu == null)
                throw new System.NullReferenceException(nameof(_mainMenu));
        }

        private void Start()
        {
            _mainMenu.Open();
        }
    }
}