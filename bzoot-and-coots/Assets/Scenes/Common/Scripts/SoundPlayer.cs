using UnityEngine;
namespace Bzoot
{
    public class SoundPlayer : MonoBehaviour
    {
        public static SoundPlayer Instance => _instance;
        static SoundPlayer _instance;

        AudioSource _audioSource;
        void Awake()
        {
            if (_instance)
            {
                Destroy(this);
                return;
            }
            _instance = this;

            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.volume = .5f;
            DontDestroyOnLoad(this);
        }

        public void PlayRandomSound(AudioClip[] clips)
        {
            var effect = clips[Random.Range(0, clips.Length)];
            PlaySound(effect);
        }
        
        public void PlaySound(AudioClip clip)
        {
            if (clip is null)
            {
                return;
            }
            if (!SessionPersistentData.Instance.IsSoundEffectEnabled)
            {
                return;
            }
            _audioSource.PlayOneShot(clip);
        }
    }
}