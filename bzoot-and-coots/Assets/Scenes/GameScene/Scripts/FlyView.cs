using UnityEngine;

namespace Bzoot
{
    public class FlyView : MonoBehaviour
    {
        public void UpdatePosition(Vector2 position)
            => transform.position = new Vector3(position.x, position.y, 0);
    }
}