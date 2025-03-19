using System;
using UnityEngine;
using Scripts.Enviroment;
using Scripts.UI.Panels;

namespace Scripts.Cars.CarPlaces
{
    public class CarPlace : MonoBehaviour
    {
        [SerializeField] private ObjectPainter _objectPainter;
        [field: SerializeField] public Transform EntryPoint { get; private set; }
        [field: SerializeField] public Transform ExitPoint { get; private set; }

        private ShowRewardPanel _showRewardPanel;

        public event Action Unlocked;

        public bool IsFree { get; private set; } = true;
        public bool IsLocked { get; private set; } = false;
        public int OrderNumber { get; private set; }

        private void OnValidate()
        {
            if (_objectPainter == null)
                throw new NullReferenceException(nameof(_objectPainter));

            if (EntryPoint == null)
                throw new NullReferenceException(nameof(EntryPoint));

            if (ExitPoint == null)
                throw new NullReferenceException(nameof(ExitPoint));
        }

        private void OnMouseUp()
        {
            if (IsLocked)
            {
                _showRewardPanel.Open();
                _showRewardPanel.IsShown += Unlock;
            }
        }

        private void Unlock(int id)
        {
            _showRewardPanel.IsShown -= Unlock;
            IsLocked = false;

            Unlocked?.Invoke();
            _objectPainter.MoveTextureByOffset(0);
        }

        public void Initialize(bool isLocked, ShowRewardPanel showRewardPanel, int orderNumber)
        {
            IsLocked = isLocked;
            OrderNumber = orderNumber;
            _showRewardPanel = showRewardPanel;

            if (isLocked)
            {
                _objectPainter.MoveTextureByOffset(0.666f);
            }
        }

        public void Take()
        {
            IsFree = false;
        }

        public void FreeUp()
        {
            IsFree = true;
        }
    }
}