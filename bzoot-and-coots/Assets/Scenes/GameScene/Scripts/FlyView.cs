using System;
using UnityEngine;

namespace Bzoot
{
    public class FlyView : MonoBehaviour
    {
        [SerializeField] Transform _sound;
        public void UpdatePosition(Vector2 position)
            => transform.position = new Vector3(position.x, position.y, transform.position.z);

        public Action OnGotHit { set; private get; }
        
        void Update()
        {
            var position = transform.position;
            _sound.position = new Vector3(position.x, position.y, _sound.transform.position.z);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Deadly"))
            {
                OnGotHit.Invoke();
            }
        }
    }
}