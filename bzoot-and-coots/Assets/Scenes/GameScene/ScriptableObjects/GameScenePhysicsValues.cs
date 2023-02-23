using UnityEngine;

namespace Bzoot
{
    [CreateAssetMenu(menuName = "ScriptableObjects/PhysicsValues")]
    public class GameScenePhysicsValues : ScriptableObject
    {
        public float GravityAccPerSecond = .6f;
        public float MaxFallingSpeedPerSecond = 1.8f;

        public float VerticalAccPerSecond = 2.6f;
        public float MaxVerticalSpeedPerSecond = 1.3f;

        public float HorizontalAccPerSecond = 2f;
        public float MaxHorizontalSpeedPerSecond = 1f;
        public float HorizontalDragPerSecond = .9f;
    }
}