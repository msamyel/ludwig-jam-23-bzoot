using UnityEngine;
using UnityEngine.UI;
namespace Bzoot
{
    public class GameSceneUi : MonoBehaviour
    {
        [SerializeField] Image _irritationBar;

        public void SetIrritation(float absoluteIrritation)
        {
            float percentage = absoluteIrritation / GameSceneEnvironment.Instance.Coots.MaxCootsTolerance;
            _irritationBar.fillAmount = percentage;
        }
    }
}