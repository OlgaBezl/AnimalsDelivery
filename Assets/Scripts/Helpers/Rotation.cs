using UnityEngine;

namespace Scripts.Helpers
{
    public class Rotation
    {
        public static float ConvertRotationTypeToDegrees(RotationType rotationType)
        {
            return 90f * (int)rotationType;
        }

        public static Vector3Int ConvertRotationToDirection(RotationType rotationType)
        {
            if (rotationType == RotationType.Zero)
                return Vector3Int.forward;
            
            if (rotationType == RotationType.Half)
                return Vector3Int.back;
            
            if (rotationType == RotationType.Quarter)
                return Vector3Int.right;
            
            return Vector3Int.left;
        }

        public static RotationType GetRandomRotationWithoutLimit()
        {
            return (RotationType)Random.Range(0, 4);
        }
    }
}