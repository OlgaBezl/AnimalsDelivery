using UnityEngine;

namespace Scripts.Helpers
{
    public class ColorPallet
    {
        public const int ColorsCountWithoutGray = 5;
        public const int GrayIndex = 0;

        public static int GetRandomColorIndex()
        {
            int index = Random.Range(0, ColorsCountWithoutGray);

            if (index >= GrayIndex)
                index++;

            return index;
        }
    }
}