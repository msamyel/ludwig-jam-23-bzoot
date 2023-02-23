using UnityEngine;
namespace Bzoot
{
    [CreateAssetMenu(menuName = "ScriptableObjects/GameCycleValues")]
    public class GameCycleValues : ScriptableObject
    {
        [Header("Respawn")]
        public Vector3 BzootRespawnPosition = new Vector3(-8f,0f,-2f);
    }
}