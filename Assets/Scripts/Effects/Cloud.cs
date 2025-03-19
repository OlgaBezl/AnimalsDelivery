using System.Collections;
using UnityEngine;

namespace Scripts.Effects
{
    public class Cloud : MonoBehaviour
    {
        [SerializeField] private float seconds = 5f;

        private void Awake()
        {
            StartCoroutine(DestroyAfterSomeSecond());
        }

        private IEnumerator DestroyAfterSomeSecond()
        {
            yield return new WaitForSeconds(seconds);
            Destroy(gameObject);
        }
    }
}