using System;
using UnityEngine;

namespace Scripts.Enviroment
{
    public class GroupPainter : MonoBehaviour
    {
        [SerializeField] private BiomPainter _biomPainter;

        private void OnValidate()
        {
            if (_biomPainter == null)
                throw new NullReferenceException(nameof(_biomPainter));
        }

        public void Paint(BiomType biomType)
        {
            Texture paleTexture = _biomPainter.GetPaleTexture(biomType);
            Texture mainTexture = _biomPainter.GetTexture(biomType);

            ObjectPainter[] objects = transform.GetComponentsInChildren<ObjectPainter>();
            int? previousIndex = null;

            foreach (ObjectPainter objectPainter in objects)
            {
                if (objectPainter.IsMainMaterial)
                {
                    objectPainter.RandomPaint(mainTexture, previousIndex);
                }
                else
                {
                    objectPainter.RandomPaint(paleTexture, previousIndex);
                }

                previousIndex = objectPainter.ColorIndex;
            }
        }
    }
}