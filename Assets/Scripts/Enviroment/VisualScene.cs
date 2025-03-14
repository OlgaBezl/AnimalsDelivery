using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualScene : MonoBehaviour
{
    [SerializeField] private GroupPainter _forest;
    [SerializeField] private GroupPainter _town;
    [SerializeField] private ObjectPainter _background;
    [SerializeField] private BiomPainter _biomPainter;

    private BiomType _currentBiomType = BiomType.Autumn;

    public void StartLevel(BiomType biomType)
    {
        _background.SetTone(_biomPainter.GetTone(biomType));

        bool changeBiom = biomType != _currentBiomType;
        _currentBiomType = biomType;

        if (changeBiom)
        {
            _forest.gameObject.SetActive(!_forest.gameObject.activeSelf);
            _town.gameObject.SetActive(!_town.gameObject.activeSelf);
        }

        if(_forest.gameObject.activeSelf)
        {
            _forest.Paint(biomType);
        }
        else
        {
            _town.Paint(biomType);
        }
    }
}
