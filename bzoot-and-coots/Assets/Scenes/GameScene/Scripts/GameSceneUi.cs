using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
namespace Bzoot
{
    public class GameSceneUi : MonoBehaviour
    {
        [Header("Progress bar")]
        [SerializeField] RectMask2D _irritationBarMask;
        [Header("Lives Display")]
        [SerializeField] Transform _livesRoot;
        [SerializeField] Image _livesPrefab;
        [Header("GameEnd")]
        [SerializeField] GameObject _gameEndRoot;
        [SerializeField] GameObject _gameWonText;
        [SerializeField] GameObject _gameLostText;
        [SerializeField] Button _restartGameButton;
        [SerializeField] Button _backToMenuButton;
        [Header("PlayerWon")]

        Tween _irritationBarAnimation;
        
        public Action OnRestart { set; private get; }
        public Action OnBackToMenu { set; private get; }

        void Awake()
        {
            _gameEndRoot.SetActive(false);
            ResetIrritationBar();
        }

        public void Init()
        {
            _restartGameButton.onClick.AddListener(() => OnRestart());
            _backToMenuButton.onClick.AddListener(() => OnBackToMenu());
        }

        public void SetIrritation(float irritation)
        {
            irritation = Mathf.Clamp01(irritation);
            
            float paddingFromRight = (1f - irritation) * _irritationBarMask.rectTransform.sizeDelta.x;
            
            //Padding to be applied to the masking X = Left Y = Bottom Z = Right W = Top
            _irritationBarMask.padding = new Vector4(x: 0, y: 0, z: paddingFromRight, w: 0);
            
            _irritationBarAnimation?.Kill(false);

            _irritationBarAnimation = DOTween.To(
                getter: () => _irritationBarMask.padding.z,
                setter: (v) => { _irritationBarMask.padding = new Vector4(0, 0, v, 0); },
                endValue: paddingFromRight,
                duration: 1f
            );
        }

        void ResetIrritationBar()
        {
            _irritationBarMask.padding = new Vector4(0, 0, _irritationBarMask.rectTransform.sizeDelta.x, 0);
        }

        public void SetLives(int livesCount)
        {
            foreach (Transform t in _livesRoot)
            {
                Destroy(t.gameObject);
            }
            for (int i = 0; i < livesCount; i++)
            {
                Instantiate(_livesPrefab, _livesRoot);
            }
        }

        public void DisplayGameOver()
        {
            _gameLostText.SetActive(true);
            _gameEndRoot.SetActive(true);
        }

        public void DisplayPlayerWon()
        {
            _gameWonText.SetActive(true);
            _gameEndRoot.SetActive(true);
        }
    }
}