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

        public void CreatePawPrint(Vector2 position)
        {
            var pawPrint = Instantiate(PawPrintPrefab, PawRoot);
            pawPrint.transform.position = new Vector3(position.x, position.y, -2);
            StartCoroutine(DeletePawPrint(pawPrint));
            //todo: add random rotation
        }

        IEnumerator DeletePawPrint(GameObject pawPrint)
        {
            yield return new WaitForSeconds(GameSceneEnvironment.Instance.Coots.PawShadowIntervalSecs);
            Destroy(pawPrint);
        }
    }
}