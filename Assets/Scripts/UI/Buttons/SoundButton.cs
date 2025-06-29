using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Buttons
{
    public class SoundButton : DefaultButton
    {
        [SerializeField] private Image _image;
        [SerializeField] private Sprite _withoutSoundSptite;
        [SerializeField] private Sprite _soundSptite;

        public event Action Click;

        private void OnValidate()
        {
            if (_image == null)
                throw new NullReferenceException(nameof(_image));

            if (_withoutSoundSptite == null)
                throw new NullReferenceException(nameof(_withoutSoundSptite));

            if (_soundSptite == null)
                throw new NullReferenceException(nameof(_soundSptite));
        }

        protected override void OnClick()
        {
            base.OnClick();
            Click?.Invoke();
        }

        public void TurnOnSound()
        {
            _image.sprite = _soundSptite;
        }

        public void TurnOffSound()
        {
            _image.sprite = _withoutSoundSptite;
        }
    }
}