using System;
using UnityEngine;

public class Trace : MonoBehaviour
{
    //[SerializeField] protected GameObject _cloudsPrefab;
    //[SerializeField] protected GameObject _traces;
    //public bool IsActive;

    //private void OnValidate()
    //{
    //    if (_cloudsPrefab == null)
    //        throw new NullReferenceException(nameof(_cloudsPrefab));

    //    if (_traces == null)
    //        throw new NullReferenceException(nameof(_traces));
    //}

    public void Show()
    {
        gameObject.SetActive(true);
        //GameObject clouds = Instantiate(_cloudsPrefab, transform.position, transform.rotation);
        //clouds.transform.parent = transform;
        //_traces.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        //_traces.SetActive(false);
    }
}
