using UnityEngine;
namespace Bzoot
{
    public class SessionPersistentData : MonoBehaviour
    {
        public static SessionPersistentData Instance => _instance;
        static SessionPersistentData _instance;

        public bool IsSoundEffectEnabled { get; set; } = true;
        
        void Awake()
        {
            if (_instance)
            {
                Destroy(this);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(this);
        }
    }
}