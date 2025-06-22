using System;
using Scripts.Cars.Model;
using Scripts.Effects;
using Scripts.Enviroment;
using Scripts.Helpers;
using UnityEngine;

namespace Scripts.Cars
{
    public abstract class Car : MonoBehaviour
    {
        [SerializeField] private ObjectPainter _objectPainter;

        [field: SerializeField] public float RotateDuration = 1f;
        [field: SerializeField] public float Speed { get; private set; } = 10f;
        [field: SerializeField] public CarSpecification Specification { get; private set; }
        [field: SerializeField] protected Trace Trace { get; set; }

        public CarModel Model { get; private set; }
        public bool CanLeftParking { get; private set; }
        public int ColorIndex { get; private set; }
        public int CurrentColorIndex { get; private set; } = -1;

        private void OnValidate()
        {
            if (_objectPainter == null)
                throw new NullReferenceException(nameof(_objectPainter));

            if (Specification == null)
                throw new NullReferenceException(nameof(Specification));

            if (Speed <= 0)
                throw new ArgumentOutOfRangeException(nameof(Speed));

            if (Trace == null)
                throw new NullReferenceException(nameof(Trace));
        }

        public void SetModel(CarModel carModel)
        {
            Model = carModel;
        }

        public virtual void Initialize(int colorIndex)
        {
            ColorIndex = colorIndex;
            _objectPainter.PaintByIndex(ColorIndex, CurrentColorIndex);
        }

        public void RepaintSharedMaterial(Texture texture)
        {
            _objectPainter.SetShaderMaterialTexture(texture);
        }

        public void GrayModeOn()
        {
            CanLeftParking = false;
            _objectPainter.PaintByIndex(ColorPallet.GrayIndex, CurrentColorIndex);
        }

        public virtual void GrayModeOff()
        {
            CanLeftParking = true;
            _objectPainter.PaintByIndex(ColorIndex, CurrentColorIndex);
        }
    }
}