using System;
using DG.Tweening;
using UnityEngine;

namespace Bzoot
{
    public class PlayerView : MonoBehaviour
    {
        [Header("Create Sound")]
        [SerializeField] Transform _soundWaveRoot;
        [SerializeField] SpriteRenderer _soundWavePrefab;
        public void UpdatePosition(Vector2 position)
            => transform.position = new Vector3(position.x, position.y, transform.position.z);

        public Action OnGotHit { set; private get; }
        
        public void CreateSound()
        {
            float finalScale = GameSceneEnvironment.Instance.Physics.SoundObjectScaleFinal;
            float scaleupDurationSecs = GameSceneEnvironment.Instance.Physics.SoundObjectScaleUpDurationSecs;
            var ease = GameSceneEnvironment.Instance.Physics.SoundObjectScaleupEase;
            var soundSprite = Instantiate(_soundWavePrefab, _soundWaveRoot);
            soundSprite.transform.position = transform.position;
            soundSprite.transform
                .DOScale(new Vector3(finalScale, finalScale, 1), scaleupDurationSecs)
                .From(Vector3.zero)
                .SetEase(ease)
                .SetLink(soundSprite.gameObject)
                .OnComplete(() => Destroy(soundSprite.gameObject));
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log($"OnTriggerEnter2D: {other.gameObject.name}");
            if (other.gameObject.CompareTag("Deadly"))
            {
                other.enabled = false;
                OnGotHit.Invoke();
            }
        }
    }
}