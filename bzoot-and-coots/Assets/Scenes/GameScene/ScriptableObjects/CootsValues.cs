using UnityEngine;
namespace Bzoot
{
    [CreateAssetMenu(menuName = "ScriptableObjects/CootsValues")]
    public class CootsValues : ScriptableObject
    {
        [Header("General")]
        public float IncreaseIrritancePerSecondOfCollision = 2f; 
        public float MaxCootsTolerance = 16f;
        [Header("Ears")]
        public float MaxEarSoundTolerance = 8f;
        public float EarClosedIntervalSecs = 10f;
    }
}