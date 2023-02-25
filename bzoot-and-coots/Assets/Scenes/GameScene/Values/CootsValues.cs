using UnityEngine;
namespace Bzoot
{
    [CreateAssetMenu(menuName = "ScriptableObjects/CootsValues")]
    public class CootsValues : ScriptableObject
    {
        [Header("General")]
        public float IncreaseIrritancePerSecondOfCollision = .002f; 
        public float DecreaseIrritanceRatioOnHitPlayer = .33f;
        [Header("Attack chance vars")]
        public float AttackChancePerSecondOfSound = .01f;
        public float AttackChancePerSecondOfSoundHittingEars = .1f;
        public float MinimumAttackChanceToTriggerAttack = .15f;
        [Header("Crazy Attack")]
        public float CrazyAttackDuringSecondPhaseChance = .1f;
        public float CrazyAttackAttackCountMin = 3;
        public float CrazyAttackAttackCountMax = 9;
        public float CrazyAttackRadius = 1f;
        public float CrazyAttackIntervalBetweenAttackSecs = .1f;
        [Header("Ears")]
        public float MaxEarSoundTolerance = .008f;
        public float EarClosedIntervalSecs = 10f;
        [Header("Paws")]
        public float DelayBetweenIndividualAttacks = .5f;
        public float PawShadowIntervalSecs = .5f;
        public float PawCoolDownSecs = 2f;
        public float ColliderEnabledDuration = .05f;
        public float PawAnimationDuration = .1f;
    }
}