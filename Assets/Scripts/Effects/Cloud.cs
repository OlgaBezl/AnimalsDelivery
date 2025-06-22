using UnityEngine;

namespace Scripts.Effects
{
    public class Cloud : MonoBehaviour
    {
        [SerializeField] private float seconds = 5f;

        private void Awake()
        {
            Destroy(gameObject, seconds);
        }
    }
}