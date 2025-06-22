using System.Linq;
using UnityEngine;

namespace Scripts.Enviroment
{
    public class BiomPainter : MonoBehaviour
    {
        private Biom[] _bioms;

        private void Awake()
        {
            _bioms = gameObject.GetComponentsInChildren<Biom>();
        }

        public Biom GetBiom(BiomType biomType)
        {
            return _bioms.FirstOrDefault(biom => biom.Type == biomType);
        }
    }
}