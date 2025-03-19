using UnityEngine;

namespace Scripts.Helpers
{
    public class Rotation
    {
        public static float ConvertRotationTypeToDegrees(RotationType rotationType)
        {
            return 90f * (int)rotationType;
        }

        public static Vector3 ConvertRotationToDirection(RotationType rotationType)
        {
            if (rotationType == RotationType.Zero)
                return Vector3.forward;
            else if (rotationType == RotationType.Half)
                return Vector3.back;
            else if (rotationType == RotationType.Quarter)
                return Vector3.right;
            else
                return Vector3.left;
        }

        public static RotationType GetRandomRotationWithoutLimit()
        {
            return (RotationType)Random.Range(0, 4);
        }
    }
}