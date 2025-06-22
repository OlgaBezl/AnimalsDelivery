using System;
using Scripts.Enviroment;
using Scripts.UI.Panels;
using UnityEngine;

namespace Scripts.Cars.CarPlaces
{
    public class CarPlace : MonoBehaviour
    {
        [SerializeField] private ObjectPainter _objectPainter;

        private ShowRewardPanel _showRewardPanel;

        public event Action Unlocked;

        [field: SerializeField] public Transform EntryPoint { get; private set; }
        [field: SerializeField] public Transform ExitPoint { get; private set; }

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

        private void Unlock(int id)
        {
            _showRewardPanel.IsShown -= Unlock;
            IsLocked = false;

            Unlocked?.Invoke();
            _objectPainter.MoveTextureByOffset(0);
        }

    }
}