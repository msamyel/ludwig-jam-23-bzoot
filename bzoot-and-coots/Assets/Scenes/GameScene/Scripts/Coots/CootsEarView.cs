using System;
using UnityEngine;

namespace Bzoot
{
    public class CootsEarView : MonoBehaviour
    {
        public Action OnCollideWithSound { set; private get; }

        SpriteRenderer _renderer;
        [SerializeField] Sprite _spriteOpen;
        [SerializeField] Sprite _spriteHalfOpen;
        [SerializeField] Sprite _spriteClosed;
        
        void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Sound"))
            {
                OnCollideWithSound.Invoke();
            }
        }

        public void CloseEar()
        {
            //todo: implement animation
            _renderer.enabled = false;
        }

        public void OpenEar()
        {
            //todo: implement animation
            _renderer.enabled = true;
        }
    }
}