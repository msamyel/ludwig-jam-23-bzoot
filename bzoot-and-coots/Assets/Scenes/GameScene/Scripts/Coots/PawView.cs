using DG.Tweening;
using UnityEngine;
namespace Bzoot
{
    public class PawView : MonoBehaviour
    {
        SpriteRenderer _spriteRenderer;
        void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            DOTween.Sequence()
                .SetLink(gameObject)
                .Append(transform.DOScale(1f, .2f).From(.5f))
                .Join(_spriteRenderer.DOColor(Vector4.one, .4f)
                    .From(new Vector4(1, 1, 1,.5f)));
        }
    }
}