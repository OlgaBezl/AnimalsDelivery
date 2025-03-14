using UnityEngine;

[CreateAssetMenu(fileName = "Result", menuName = "ScriptableObjects/Results", order = 2)]
public class ResultType : ScriptableObject
{
    [field: SerializeField] public int MinPercent { get; private set; }
    [field: SerializeField] public int StarsCount { get; private set; }

    //public int FreePoints;
}
