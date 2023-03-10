using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Bzoot
{
    public class CootsLeavingAnimation : MonoBehaviour
    {
        [SerializeField] Transform _speechBubble;
        [SerializeField] CootsView _cootsRoot;
        [SerializeField] Transform _leavingCootsRoot;

        void Awake()
        {
            _speechBubble.gameObject.SetActive(false);
            _leavingCootsRoot.gameObject.SetActive(false);
        }
        
        public void PlayAnimation(Action onComplete)
        {
            _speechBubble.gameObject.SetActive(true);
            
            DOTween.Sequence()
                .SetLink(gameObject)
                .Append(
                    _speechBubble.DOLocalMoveY(.72f, 2.2f)
                        .From(6.5f)
                        .SetEase(Ease.InBack))
                .AppendCallback(() => StartCoroutine(ContinuousMeowing()))
                .AppendInterval(1f)
                .AppendCallback(() =>
                {
                    _cootsRoot.EarOnTheRight.CloseEar();
                    _cootsRoot.EarOnTheLeft.CloseEar();
                })
                .AppendInterval(1f)
                .AppendCallback(
                    () =>
                    {
                        _speechBubble.gameObject.SetActive(false);
                        _cootsRoot.gameObject.SetActive(false);
                        _leavingCootsRoot.gameObject.SetActive(true);
                    })
                .AppendCallback(() =>
                {
                    _leavingCootsRoot
                        .DOLocalMoveY(-.1f, 2f)
                        .SetLoops(-1, LoopType.Yoyo)
                        .SetLink(_leavingCootsRoot.gameObject);
                    _leavingCootsRoot.DOLocalMoveX(-5f, 10f)
                        .SetLink(_leavingCootsRoot.gameObject);
                })
                .AppendInterval(3f)
                .AppendCallback(() => onComplete.Invoke());
        }

        IEnumerator ContinuousMeowing()
        {
            for (int i = 0; i < 6; i++)
            {
                SoundPlayer.Instance.PlayRandomSound(GameSceneEnvironment.Instance.CootsMeowAudioClips);
                yield return new WaitForSeconds(.33f);
            }
        }
    }
}
