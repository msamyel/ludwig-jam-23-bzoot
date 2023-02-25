using DG.Tweening;
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
        [Header("Animation")]
        [SerializeField] Image _blackCurtain;
        [SerializeField] Transform _title;

        void Awake()
        {
            _controlsDialog.SetActive(false);
            
            _onePlayerGameStartButton.onClick.AddListener(DisplayControlsDialog);
            _controlDialogsOkBtn.onClick.AddListener(StartGame);
            
            _soundSettingsToggle.onValueChanged.AddListener((SetSoundEffectsEnabled));
            
            PlayAnimation();
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

        void PlayAnimation()
        {
            var seq = DOTween.Sequence()
                .SetLink(gameObject)
                .Append(_title.DOLocalMoveY(9f, 2f).SetEase(Ease.InBack))
                .Append(_blackCurtain.DOFade(0, .3f))
                .AppendCallback(() => _blackCurtain.gameObject.SetActive(false))
                .Append(
                    _onePlayerGameStartButton.transform.DOLocalMoveY(-289, 2f)
                        .SetEase(Ease.InBack));

            if (SessionPersistentData.Instance.IsInitialAnimationCompletedOnce)
            {
                seq.Kill(true);
                _blackCurtain.gameObject.SetActive(false);
            }
            SessionPersistentData.Instance.IsInitialAnimationCompletedOnce = true;
        }
    }
}
