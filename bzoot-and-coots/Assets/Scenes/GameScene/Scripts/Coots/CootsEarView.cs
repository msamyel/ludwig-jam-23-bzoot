using System;
using System.Collections;
using UnityEngine;

namespace Bzoot
{
    public class CootsEarView : MonoBehaviour
    {
        public Action OnCollideWithSound { set; private get; }

        SpriteRenderer _renderer;
        Collider2D _collider;
        [SerializeField] Sprite _spriteOpen;
        [SerializeField] Sprite _spriteHalfOpen;
        [SerializeField] Sprite _spriteClosed;
        
        const float AnimFrameDurationSecs = .05f;
            
        void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<Collider2D>();
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
            _collider.enabled = false;
            StartCoroutine(CloseEarAnimation());
        }

        IEnumerator CloseEarAnimation()
        {
            _renderer.sprite = _spriteHalfOpen;
            yield return new WaitForSeconds(AnimFrameDurationSecs);
            _renderer.sprite = _spriteClosed;
        }

        public void OpenEar()
        {
            _collider.enabled = true;
            StartCoroutine(OpenEarAnimation());
        }
        
        IEnumerator OpenEarAnimation()
        {
            _renderer.sprite = _spriteHalfOpen;
            yield return new WaitForSeconds(AnimFrameDurationSecs);
            _renderer.sprite = _spriteOpen;
        }
    }
}