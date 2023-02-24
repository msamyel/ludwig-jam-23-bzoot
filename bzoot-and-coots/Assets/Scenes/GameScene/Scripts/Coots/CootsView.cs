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

        public void AttackPlayer(Vector2 position)
        {
            CreatePawPrint(position);
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
            //todo: add random rotation (ma
            var pawPrint = Instantiate(PawPrintPrefab, PawRoot);
            pawPrint.transform.position = new Vector3(position.x, position.y, -2);
            StartCoroutine(DeletePawPrint(pawPrint));
            StartCoroutine(CreatePaw(position));
        }

        IEnumerator DeletePawPrint(GameObject pawPrint)
        {
            yield return new WaitForSeconds(GameSceneEnvironment.Instance.Coots.PawShadowIntervalSecs);
            Destroy(pawPrint);
        }

        IEnumerator CreatePaw(Vector2 playerPosition)
        {
            //wait for the paw print to disappear
            yield return new WaitForSeconds(GameSceneEnvironment.Instance.Coots.PawShadowIntervalSecs);
            //create paw
            var paw = Instantiate(PawPrefab, PawRoot);
            paw.transform.position = new Vector3(0, 0, -2f);
            //todo: maybe add rotation
            float animationDurationSecs = GameSceneEnvironment.Instance.Coots.PawAnimationDuration;
            paw.transform.DOLocalMove(new Vector3(playerPosition.x, playerPosition.y, -2), animationDurationSecs);
            yield return new WaitForSeconds(animationDurationSecs);
            paw.EnableCollider();
            //wait one frame
            yield return new WaitForSeconds(.1f);
            Destroy(paw.gameObject);
        }
    }
}