using UnityEngine;

namespace Scripts.Cars
{
    [CreateAssetMenu(fileName = "Car", menuName = "ScriptableObjects/Cars", order = 1)]
    public class CarSpecification : ScriptableObject
    {
        [field: SerializeField] public int SeatsCount { get; private set; } = 4;
        [field: SerializeField] public int Length { get; private set; } = 1;
        [field: SerializeField] public int Width { get; private set; } = 1;
    }
}