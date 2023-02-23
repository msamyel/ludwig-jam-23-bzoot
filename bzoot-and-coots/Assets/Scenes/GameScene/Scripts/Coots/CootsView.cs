using System.Collections;
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

        public void AttackPlayer(Vector2 position)
        {
            CreatePawPrint(position);
            //todo: add random rotation
        }

        void CreatePawPrint(Vector2 position)
        {
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
            //todo: add animation (dotween?)
            const float animationDurationSecs = .2f;
            yield return new WaitForSeconds(animationDurationSecs);
            paw.transform.position = new Vector3(playerPosition.x, playerPosition.y, -2);
            paw.EnableCollider();
            //wait one frame
            yield return new WaitForSeconds(.1f);
            Destroy(paw.gameObject);
        }
    }
}