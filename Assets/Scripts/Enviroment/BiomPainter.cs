using UnityEngine;

namespace Scripts.Enviroment
{
    public class BiomPainter : MonoBehaviour
    {
        [field: SerializeField] private Texture _autumnTexture;
        [field: SerializeField] private Texture _winterTexture;
        [field: SerializeField] private Texture _springTexture;
        [field: SerializeField] private Texture _summerTexture;

        [field: SerializeField] private Texture _autumnPaleTexture;
        [field: SerializeField] private Texture _winterPaleTexture;
        [field: SerializeField] private Texture _springPaleTexture;
        [field: SerializeField] private Texture _summerPaleTexture;

        [field: SerializeField] private Color _autumnTone;
        [field: SerializeField] private Color _winterTone;
        [field: SerializeField] private Color _springTone;
        [field: SerializeField] private Color _summerTone;

        public Texture GetTexture(BiomType biomType)
        {
            if (biomType == BiomType.Autumn)
                return _autumnTexture;
            else if (biomType == BiomType.Winter)
                return _winterTexture;
            if (biomType == BiomType.Spring)
                return _springTexture;
            return _summerTexture;
        }

        public Texture GetPaleTexture(BiomType biomType)
        {
            if (biomType == BiomType.Autumn)
                return _autumnPaleTexture;
            else if (biomType == BiomType.Winter)
                return _winterPaleTexture;
            if (biomType == BiomType.Spring)
                return _springPaleTexture;
            return _summerPaleTexture;
        }

        public Color GetTone(BiomType biomType)
        {
            if (biomType == BiomType.Autumn)
                return _autumnTone;
            else if (biomType == BiomType.Winter)
                return _winterTone;
            if (biomType == BiomType.Spring)
                return _springTone;
            return _summerTone;
        }
    }
}