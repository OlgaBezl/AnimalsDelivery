using System.Collections.Generic;
using UnityEngine;

public class AnimatedAnimal : MonoBehaviour
{
    [SerializeField] private Animal[] _animals;
    //[SerializeField] private Transform[] _fireworksSpawnPoints;
    [SerializeField] private Firework[] _fireworks;
    [SerializeField] private SadFaceSpawner[] _sadFaceSpawners;
    //[SerializeField] private Firework _firework;
    //[SerializeField] private Animal _animal;
    private Animal _animal;
    private BiomType _biomType = BiomType.Autumn;
    //private List<Firework> _fireworks;
    //private void Start()
    //{
    //    Show();
    //    _animal.Initialize(ColorPallet.GetRandomColorIndex());
    //}

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Show(bool goodResult)
    {
        _animal = Instantiate(_animals[Random.Range(0, _animals.Length)], transform);
        //_animal = Instantiate(_animals[0], transform);

        _animal.Initialize(ColorPallet.GetRandomColorIndex(), false);
        

        if (goodResult)
        {
            _animal.Dance();

            foreach(Firework firework in _fireworks)
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

        foreach(Firework firework in _fireworks)
        {
            firework.Clear();
        }

        foreach (SadFaceSpawner sadFaceSpawner in _sadFaceSpawners)
        {
            sadFaceSpawner.Clear();
        }
    }
}
