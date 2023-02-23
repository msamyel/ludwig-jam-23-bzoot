using System.Collections;
using UnityEngine;

namespace Bzoot
{
    public class CootsPawView : MonoBehaviour
    {
        [SerializeField] Collider2D _collider;
        void Awake()
        {
            //disable collider before animation
            _collider.enabled = false;
        }

        public void EnableCollider()
        {
            _collider.enabled = true;
            StartCoroutine(DisableCollider());
        }

        IEnumerator DisableCollider()
        {
            //wait one frame, disable on next frame
            yield return null;
            _collider.enabled = false;
        }
    }
}
