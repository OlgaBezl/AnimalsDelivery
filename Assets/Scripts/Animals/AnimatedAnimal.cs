using UnityEngine;
using Scripts.Helpers;
using Scripts.Effects;

namespace Scripts.Animals
{
    public class AnimatedAnimal : MonoBehaviour
    {
        [SerializeField] private Animal[] _animals;
        [SerializeField] private Firework[] _fireworks;
        [SerializeField] private SadFaceSpawner[] _sadFaceSpawners;

        private Animal _animal;

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Show(bool goodResult)
        {
            _animal = Instantiate(_animals[Random.Range(0, _animals.Length)], transform);
            _animal.Initialize(ColorPallet.GetRandomColorIndex(), false);

            if (goodResult)
            {
                _animal.Dance();

                foreach (Firework firework in _fireworks)
                {
                    firework.Activate(ColorPallet.GetRandomColorIndex());
                }
            }
            else
            {
                _animal.Despond();

                foreach (SadFaceSpawner sadFaceSpawner in _sadFaceSpawners)
                {
                    sadFaceSpawner.Activate(ColorPallet.GetRandomColorIndex());
                }
            }
        }

        public void Hide()
        {
            Destroy(_animal.gameObject);

            foreach (Firework firework in _fireworks)
            {
                firework.Clear();
            }

            foreach (SadFaceSpawner sadFaceSpawner in _sadFaceSpawners)
            {
                sadFaceSpawner.Clear();
            }
        }
    }
}