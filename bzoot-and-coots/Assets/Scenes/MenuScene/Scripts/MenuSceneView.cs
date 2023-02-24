using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Bzoot
{
    public class MenuSceneView : MonoBehaviour
    {
        [Header("Main Controls")]
        [SerializeField] Button _onePlayerGameStartButton;
        [SerializeField] Toggle _soundSettingsToggle;
        [Header("Display Controls")]
        [SerializeField] GameObject _controlsDialog;
        [SerializeField] Button _controlDialogsOkBtn;

        void Awake()
        {
            _controlsDialog.SetActive(false);
            
            _onePlayerGameStartButton.onClick.AddListener(DisplayControlsDialog);
            _controlDialogsOkBtn.onClick.AddListener(StartGame);
            
            _soundSettingsToggle.onValueChanged.AddListener((SetSoundEffectsEnabled));
        }

        void DisplayControlsDialog()
        {
            _controlsDialog.SetActive(true);
        }

        void SetSoundEffectsEnabled(bool isEnabled)
        {
            SessionPersistentData.Instance.IsSoundEffectEnabled = isEnabled;
        }

        void StartGame()
        {
            SceneManager.LoadScene(Constants.Scene.GameScene);
        }
    }
}
