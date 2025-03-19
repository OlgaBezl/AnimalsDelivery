using System.Linq;
using UnityEngine;

namespace Scripts.Enviroment
{
    public class ForestGenerator : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private float _offset = 4f;
        [SerializeField] private float _minScaleFactor = 0.8f;
        [SerializeField] private float _maxScaleFactor = 1.2f;
        [SerializeField] private bool _chessOrder;
        [SerializeField] private GameObject[] _trees;

        private int _xIntervalCount;
        private int _zIntervalCount;

        private float _xBorderOffset;
        private float _zBorderOffset;

        private void Awake()
        {
            Vector3 size = _meshRenderer.bounds.size;

            _xIntervalCount = Mathf.FloorToInt(size.x / _offset);
            _zIntervalCount = Mathf.FloorToInt(size.z / _offset);

            _xBorderOffset = Mathf.Abs((size.x - _xIntervalCount * _offset) / 2f);
            _zBorderOffset = Mathf.Abs((size.z - _zIntervalCount * _offset) / 2f);

            Generate();
        }

        public void Generate()
        {
            bool even = false;

            float halfWidth = _xIntervalCount * _offset / 2f - _xBorderOffset;

            for (float x = -halfWidth; x <= halfWidth; x += _offset)
            {
                int zIntervalCount = _chessOrder && even ? _zIntervalCount - 1 : _zIntervalCount;
                float halfDepth = zIntervalCount * _offset / 2f - _zBorderOffset;

                for (float z = -halfDepth; z <= halfDepth; z += _offset)
                {
                    int treeIndex = Random.Range(0, _trees.Count());
                    float scaleFactor = Random.Range(_minScaleFactor, _maxScaleFactor);
                    Vector3 position = transform.position + new Vector3(x, 0, z);
                    GameObject tree = Instantiate(_trees[treeIndex], position, Quaternion.identity, transform);
                    tree.transform.localScale = tree.transform.localScale * scaleFactor;
                }

                even = !even;
            }
        }
    }
}