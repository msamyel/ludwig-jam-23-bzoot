using System;
using DG.Tweening;
using UnityEngine;

namespace Bzoot
{
    public class PlayerView : MonoBehaviour
    {
        [Header("Bzoot")]
        [SerializeField] SpriteRenderer _spriteRenderer;
        [SerializeField] Animator _spriteAnimator;

        [Header("Create Sound")]
        [SerializeField] Transform _soundWaveRoot;
        [SerializeField] SpriteRenderer _soundWavePrefab;
        [SerializeField] AudioClip[] _bzzClips;

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
            SoundPlayer.Instance.PlayRandomSound(_bzzClips);
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

        public void AnimateHorizontalSpeed(float horizontalSpeedPerSecond)
        {
            bool isFacingFront = Mathf.Abs(horizontalSpeedPerSecond) <= .2f;
            //sprite is usually facing right
            _spriteRenderer.flipX = horizontalSpeedPerSecond > -.2f;
            _spriteAnimator.SetBool("IsFacingFront", isFacingFront);

            transform.rotation = Quaternion.Euler(0, 0, - horizontalSpeedPerSecond * 45f / 3f);
        }

        public void AnimateVerticalAcceleration(bool isVerticalAcceleration)
        {
            _spriteAnimator.speed = isVerticalAcceleration ? 1 : 0;
        }

        public void DrawBloodstain()
        {
            AnimateVerticalAcceleration(false);
            var bzootPos = transform.position;
            _bloodstain.localScale = new Vector3(.65f, 0, 1);
            _bloodstain.position = new Vector3(bzootPos.x, bzootPos.y, -.5f);
            _bloodstain.gameObject.SetActive(true);

            DOTween.Sequence()
                .SetLink(gameObject)
                .Append(
                    _bloodstain.DOScale(
                        new Vector3(.65f, .75f, 1), 3f))
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