using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Bzoot
{
    public class CootsView : MonoBehaviour
    {
        [Header("Ears")]
        public CootsEarView EarOnTheRight;
        public CootsEarView EarOnTheLeft;

        [Header("Paws")]
        public Transform PawRoot;
        public GameObject PawPrintPrefab;
        public CootsPawView PawPrefab;

        [Header("Mouth")]
        public SpriteRenderer Mouth;
        public Sprite MouthSpriteNormal;
        public Sprite MouthSpriteAngry;

        [Header("Eyes")]
        public Transform IrisRoot;
        public Transform Irises;
        
        Sequence _irisAnimation;

        public void Init()
        {
            AnimateEyes();
        }

        public void AttackPlayer(Vector2 position)
        {
            CreatePawPrint(position);
        }

        public void StopCoroutinesAndCleanObjects()
        {
            StopAllCoroutines();
            foreach (Transform t in PawRoot)
            {
                Destroy(t.gameObject);
            }
        }

        public void CrazyAttack(Vector2 playerPosition)
        {
            StartCoroutine(CrazyAttackCoroutine(playerPosition));
        }

        IEnumerator CrazyAttackCoroutine(Vector2 playerPosition)
        {
            int attackCount = (int)Random.Range(
                GameSceneEnvironment.Instance.Coots.CrazyAttackAttackCountMin,
                GameSceneEnvironment.Instance.Coots.CrazyAttackAttackCountMax + 1);

            float attackRadius = GameSceneEnvironment.Instance.Coots.CrazyAttackRadius;

            for (int i = 0; i < attackCount; i++)
            {
                Vector2 randomPosition = playerPosition
                                         + new Vector2(
                                             Random.Range(-attackRadius, attackRadius),
                                             Random.Range(-attackRadius, attackRadius));
                CreatePawPrint(randomPosition);
                yield return new WaitForSecondsRealtime(GameSceneEnvironment.Instance.Coots.CrazyAttackIntervalBetweenAttackSecs);
            }
        }

        void CreatePawPrint(Vector2 position)
        {
            var pawPrint = Instantiate(PawPrintPrefab, PawRoot);
            pawPrint.transform.position = new Vector3(position.x, position.y, -2);
            var rotation = Quaternion.Euler(0, 0, Random.Range(-30, 31));
            pawPrint.transform.rotation = rotation;
            StartCoroutine(DeletePawPrint(pawPrint));
            StartCoroutine(CreatePaw(position,rotation));
        }

        IEnumerator DeletePawPrint(GameObject pawPrint)
        {
            yield return new WaitForSeconds(GameSceneEnvironment.Instance.Coots.PawShadowIntervalSecs);
            Destroy(pawPrint);
        }

        IEnumerator CreatePaw(Vector2 playerPosition, Quaternion rotation)
        {
            //wait for the paw print to disappear
            yield return new WaitForSeconds(GameSceneEnvironment.Instance.Coots.PawShadowIntervalSecs);
            //create paw
            var paw = Instantiate(PawPrefab, PawRoot);
            paw.transform.position = new Vector3(Random.Range(-5,5), Random.Range(-5,0), -2f);
            paw.transform.rotation = rotation;
            float animationDurationSecs = GameSceneEnvironment.Instance.Coots.PawAnimationDuration;
            paw.transform
                .DOLocalMove(new Vector3(playerPosition.x, playerPosition.y, -2), animationDurationSecs)
                .SetLink(paw.gameObject);
            yield return new WaitForSeconds(animationDurationSecs);
            SoundPlayer.Instance.PlayRandomSound(paw.SoundEffects);
            paw.EnableCollider();
            //wait one frame
            yield return new WaitForSeconds(.1f);
            Destroy(paw.gameObject);
        }

        void AnimateEyes()
        {
            _irisAnimation?.Kill(complete: false);

            _irisAnimation = DOTween.Sequence()
                .SetLink(gameObject)
                .Append(
                    Irises.DOLocalMove(new Vector3(0, 0, -1), .1f))
                .Append(
                    Irises.DOShakePosition(
                        duration: 2f,
                        strength: Vector2.one * 0.02f,
                        vibrato: 1,
                        fadeOut: false))
                .OnComplete(() => AnimateEyes());
        }

        void FocusIrisesOnPlayer(Vector2 playerPos)
        {
            _irisAnimation?.Kill(complete: false);
            //todo: finish feature
        }
    }
}