using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Biom", menuName = "ScriptableObjects/Results", order = 2)]
//public class BiomType : ScriptableObject
//{
//    [field: SerializeField] public int ID { get; private set; }
//    [field: SerializeField] public int Name { get; private set; }
//}
public enum BiomType
{
    Autumn,
    Winter,
    Spring,
    Summer
}