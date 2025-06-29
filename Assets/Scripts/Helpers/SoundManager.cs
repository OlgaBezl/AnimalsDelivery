using System.Linq;
using Scripts.UI.Buttons;
using UnityEngine;
using YG;

namespace Scripts.Helpers
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private SoundButton[] buttons;
        [SerializeField] private AudioSource _audioSource;

        private bool _isPlaying;

        private void OnValidate()
        {
            if (_audioSource == null)
                throw new System.NullReferenceException(nameof(_audioSource));

            if (buttons == null || buttons.Count() == 0)
                throw new System.NullReferenceException(nameof(buttons));
        }

        private void OnEnable()
        {
            _isPlaying = _audioSource.isPlaying;
            YandexGame.onVisibilityWindowGame += OnVisibilityWindowGame;

            foreach (SoundButton button in buttons)
            {
                button.Click += SwitchPlayingAndUpdateFlag;
            }
        }

        private void OnDisable()
        {
            YandexGame.onVisibilityWindowGame -= OnVisibilityWindowGame;

            foreach (SoundButton button in buttons)
            {
                button.Click -= SwitchPlayingAndUpdateFlag;
            }
        }

        public void SwitchPlayingAndUpdateFlag()
        {
            SwitchPlaying(_audioSource.isPlaying);
            _isPlaying = _audioSource.isPlaying;
        }

        public void SwitchPlaying(bool isPlaying)
        {
            if (isPlaying)
            {
                TurnOffSound();
            }
            else
            {
                TurnOnSound();
            }
        }

        private void OnVisibilityWindowGame(bool visible)
        {
            if (_isPlaying)
            {
                SwitchPlaying(!visible);
            }
        }

        private void TurnOnSound()
        {
            _audioSource.UnPause();

            foreach (SoundButton button in buttons)
            {
                button.TurnOnSound();
            }
        }

        private void TurnOffSound()
        {
            _audioSource.Pause();

            foreach (SoundButton button in buttons)
            {
                button.TurnOffSound();
            }
        }
    }
}