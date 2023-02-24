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

        [Header("Vfx")]
        [SerializeField] Transform _bloodstain;

        public void UpdatePosition(Vector2 position)
            => transform.position = new Vector3(position.x, position.y, transform.position.z);

        public Action OnGotHit { set; private get; }

        void Awake()
        {
            _bloodstain.gameObject.SetActive(false);
        }

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

        public void DrawBloodstain()
        {
            var bzootPos = transform.position;
            _bloodstain.localScale = new Vector3(.65f, 0, 1);
            _bloodstain.position = new Vector3(bzootPos.x, bzootPos.y, -.5f);
            _bloodstain.gameObject.SetActive(true);

            DOTween.Sequence()
                .SetLink(gameObject)
                .Append(
                    _bloodstain.DOScale(
                        new Vector3(.65f, .75f, 1), 5f))
                .AppendInterval(1f)
                .AppendCallback(() =>
                {
                    if (_bloodstain)
                    {
                        _bloodstain.gameObject.SetActive(false);
                    }
                });
        }
    }
}