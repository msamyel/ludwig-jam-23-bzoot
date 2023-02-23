using UnityEngine;
using UnityEngine.UI;
namespace Bzoot
{
    public class GameSceneUi : MonoBehaviour
    {
        [Header("Progress bar")]
        [SerializeField] Image _irritationBar;
        [Header("Lives Display")]
        [SerializeField] Transform _livesRoot;
        [SerializeField] Image _livesPrefab;

        public void SetIrritation(float absoluteIrritation)
        {
            float percentage = absoluteIrritation / GameSceneEnvironment.Instance.Coots.MaxCootsTolerance;
            _irritationBar.fillAmount = percentage;
        }

        public void SetLives(int livesCount)
        {
            foreach (Transform t in _livesRoot)
            {
                Destroy(t.gameObject);
            }
            for (int i = 0; i < livesCount; i++)
            {
                Instantiate(_livesPrefab, _livesRoot);
            }
        }
    }
}