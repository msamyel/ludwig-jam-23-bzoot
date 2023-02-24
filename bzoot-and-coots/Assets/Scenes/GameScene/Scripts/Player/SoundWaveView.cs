using DG.Tweening;
using UnityEngine;
namespace Bzoot
{
    public class SoundWaveView : MonoBehaviour
    {
        void Awake()
        {
            Rotate();
        }

        void Rotate()
        {
            if (!transform)
            {
                return;
            }
            float rotateSpeed = Random.Range(-1f, 1f);
            transform.DOLocalRotate(new Vector3(0, 0, 360) * rotateSpeed, 1f)
                .SetLink(gameObject)
                .SetRelative(true)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    Rotate();
                });
        }
    }
}