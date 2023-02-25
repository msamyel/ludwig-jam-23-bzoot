using System;
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
        [Header("GameOver")]
        [SerializeField] GameObject _gameOverCardRoot;
        [SerializeField] Button _restartGameButton;
        [SerializeField] Button _backToMenuButton;
        [Header("PlayerWon")]
        [SerializeField] GameObject _playerWonCardRoot;

        public Action OnRestart { set; private get; }
        public Action OnBackToMenu { set; private get; }

        void Awake()
        {
            _gameOverCardRoot.SetActive(false);
            _playerWonCardRoot.SetActive(false);
        }

        public void Init()
        {

            _restartGameButton.onClick.AddListener(() => OnRestart());
            _backToMenuButton.onClick.AddListener(() => OnBackToMenu());
        }

        public void SetIrritation(float irritation)
        {
            float paddingFromRight = (1f - irritation) * _irritationBarMask.rectTransform.sizeDelta.x;
            //Padding to be applied to the masking X = Left Y = Bottom Z = Right W = Top
            _irritationBarMask.padding = new Vector4(x: 0, y: 0, z: paddingFromRight, w: 0);
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
            _gameOverCardRoot.SetActive(true);
        }

        public void DisplayPlayerWon()
        {
            _playerWonCardRoot.SetActive(true);
        }
    }
}