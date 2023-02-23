using UnityEngine;
namespace Bzoot
{
    [CreateAssetMenu(menuName = "ScriptableObjects/CootsValues")]
    public class CootsValues : ScriptableObject
    {
        [Header("General")]
        public float IncreaseIrritancePerSecondOfCollision = 2f; 
        public float MaxCootsTolerance = 16f;
        [Header("Attack chance vars")]
        public float AttackChancePerSecondOfSound = .01f;
        public float AttackChancePerSecondOfSoundHittingEars = .1f;
        public float MinimumAttackChanceToTriggerAttack = .15f;
        [Header("Ears")]
        public float MaxEarSoundTolerance = 8f;
        public float EarClosedIntervalSecs = 10f;
        [Header("Paws")]
        public float PawShadowIntervalSecs = .5f;
        public float PawCoolDownSecs = 2f;
    }
}