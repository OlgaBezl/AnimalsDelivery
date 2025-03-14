using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Firework : MonoBehaviour
{
    [SerializeField] private GameObject _part;
    [SerializeField] private int _rayCount = 8;
    [SerializeField] private float _radius = 10f;
    [SerializeField] private float _moveDuration = 2f;
    [SerializeField] private float _height = 15f;

    private List<GameObject> _parts;
    private List<ObjectPainter> _objectPainters;

    public void Activate(int colorIndex)
    {
        _parts = new List<GameObject>();
        _objectPainters = new List<ObjectPainter>();

        for (int i = 0; i < _rayCount; i++)
        {
            GameObject part = Instantiate(_part, transform);
            _parts.Add(part);

            ObjectPainter objectPainter = part.GetComponent<ObjectPainter>();
            objectPainter.PaintByIndex(colorIndex, ColorPallet.GrayIndex);
            _objectPainters.Add(objectPainter);
        }

        float angleStep = 360 / _rayCount * Mathf.Deg2Rad;

        for (int i = 0; i < _rayCount; i++)
        {
            float angle = angleStep * i;
            Vector2 localPosition = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * _radius;
            Vector3 newPosition = _parts[i].transform.position + new Vector3(localPosition.x, localPosition.y + _height, 0);

            DOTween.Sequence().
                Append(_parts[i].transform.DOMove(newPosition, _moveDuration)).
                Append(_parts[i].transform.DOScale(_parts[i].transform.localScale * 3, _moveDuration / 2)).
                Append(_parts[i].transform.DOScale(_parts[i].transform.localScale / 10, _moveDuration)).
                //SetEase(Ease.).
                onComplete += () => Destroy(_parts[i].gameObject);
        }
    }

    public void Clear()
    {
        if (_parts == null) 
            return;

        for(int i=0; i < _parts.Count; i++)
        {
            Destroy(_parts[i].gameObject);
        }

        _parts.Clear();
        _objectPainters.Clear();
    }
}
