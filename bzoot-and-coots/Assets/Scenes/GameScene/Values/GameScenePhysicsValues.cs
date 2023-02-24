using DG.Tweening;
using UnityEngine;

namespace Bzoot
{
    [CreateAssetMenu(menuName = "ScriptableObjects/PhysicsValues")]
    public class GameScenePhysicsValues : ScriptableObject
    {
        [Header("Gravity")]
        public float GravityAccPerSecond = .6f;
        public float MaxFallingSpeedPerSecond = 1.8f;

        [Header("Vertical Acceleration")]
        public float VerticalAccPerSecond = 2.6f;
        public float MaxVerticalSpeedPerSecond = 1.3f;

        [Header("Horizontal Movement")]
        public float HorizontalAccPerSecond = 2f;
        public float MaxHorizontalSpeedPerSecond = 1f;
        public float HorizontalDragPerSecond = .9f;

        [Header("Sound Creating")]
        public float CreateSoundCooldownSecs = 1f;
        public float SoundObjectScaleFinal = .66f;
        public float SoundObjectScaleUpDurationSecs = .25f;
        public Ease SoundObjectScaleupEase = Ease.OutQuad;
    }
}