using Scripts.Enviroment;
using UnityEngine;

public class Biom : MonoBehaviour
{
    [field: SerializeField] public BiomType Type { get; private set; }
    [field: SerializeField] public Texture MainTexture { get; private set; }
    [field: SerializeField] public Texture PaleTexture { get; private set; }
    [field: SerializeField] public Color Tone { get; private set; }
}
