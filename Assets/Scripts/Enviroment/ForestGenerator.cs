using System.Linq;
using UnityEngine;

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

    private int _xTreeCount;
    private int _zTreeCount;

    private float _xBorderOffset;
    private float _zBorderOffset;

    private void Awake()
    {
        Vector3 size = _meshRenderer.bounds.size;

        _xIntervalCount = Mathf.FloorToInt(size.x / _offset);
        _zIntervalCount = Mathf.FloorToInt(size.z / _offset);
        _xTreeCount = _xIntervalCount + 1;
        _zTreeCount = _zIntervalCount + 1;

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
                GameObject tree = Instantiate(_trees[treeIndex], transform.position + new Vector3(x, 0, z), Quaternion.identity, transform);
                tree.transform.localScale = tree.transform.localScale * scaleFactor;
            }

            even = !even;
        }
    }
    //public void Generate2()
    //{
    //    for (float x = -_halfWidth; x < _halfWidth; x += _xOffset)
    //    {
    //        for (float z = -_halfDepth; z < _halfDepth; z += _zOffset)
    //        {
    //            int treeIndex = Random.Range(0, _trees.Count());
    //            float scaleFactor = Random.Range(_minScaleFactor, _maxScaleFactor);
    //            GameObject tree = Instantiate(_trees[treeIndex], transform.position + new Vector3(x, 0, z), Quaternion.identity, transform);
    //            tree.transform.localScale = tree.transform.localScale * scaleFactor;
    //        }
    //    }
    //}
}
