using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Scripts.Helpers;
using Scripts.Enviroment;

namespace Scripts.Effects
{
    public class SadFaceSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _sadFace;
        [SerializeField] private int _count = 3;
        [SerializeField] private float _moveDuration = 1.5f;
        [SerializeField] private float _height = 2f;

        private List<GameObject> _faces;
        private List<ObjectPainter> _objectPainters;
        private int _currentCount;

        public void Activate(int colorIndex)
        {
            _faces = new List<GameObject>();
            _objectPainters = new List<ObjectPainter>();
            _currentCount = _count;

            CreateSadFace(colorIndex);
        }

        public void Clear()
        {
            if (_faces == null)
                return;

            _currentCount = 0;

            for (int i = 0; i < _faces.Count; i++)
            {
                Destroy(_faces[i].gameObject);
            }

            _faces.Clear();
            _objectPainters.Clear();
        }

        private void CreateSadFace(int colorIndex)
        {
            if (_currentCount <= 0)
                return;

            _currentCount--;
            GameObject sadFace = Instantiate(_sadFace, transform);
            _faces.Add(sadFace);

            ObjectPainter objectPainter = sadFace.GetComponent<ObjectPainter>();
            objectPainter.PaintByIndex(colorIndex, ColorPallet.GrayIndex);
            _objectPainters.Add(objectPainter);

            Vector3 newPosition = transform.position + new Vector3(0, -_height, 0);

            DOTween.Sequence().
                Append(sadFace.transform.DOMove(newPosition, _moveDuration)).
                Append(sadFace.transform.DOScale(sadFace.transform.localScale * 1.2f, _moveDuration / 3)).
                Append(sadFace.transform.DOScale(sadFace.transform.localScale / 10f, _moveDuration / 3)).
                onComplete += () =>
                {
                    Destroy(sadFace.gameObject);
                    CreateSadFace(colorIndex);
                };
        }
    }
}