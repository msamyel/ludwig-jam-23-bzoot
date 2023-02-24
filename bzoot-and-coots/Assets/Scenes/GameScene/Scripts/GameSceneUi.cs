using System;
using UnityEngine;
using UnityEngine.UI;
namespace Bzoot
{
    public class GameSceneUi : MonoBehaviour
    {
        [Header("Progress bar")]
        [SerializeField] Image _irritationBar;
        [Header("Lives Display")]
        [SerializeField] Transform _livesRoot;
        [SerializeField] Image _livesPrefab;
        [Header("GameOver")]
        [SerializeField] GameObject _gameOverCardRoot;
        [SerializeField] Button _restartGameButton;
        [SerializeField] Button _backToMenuButton;

        public Action OnRestart { set; private get; }
        public Action OnBackToMenu { set; private get; }

        void Awake()
        {
            _gameOverCardRoot.SetActive(false);
        }

        public void Init()
        {
            
            _restartGameButton.onClick.AddListener(() => OnRestart());
            _backToMenuButton.onClick.AddListener(() => OnBackToMenu());
        }
        
        public void SetIrritation(float irritation)
        {
            _irritationBar.fillAmount = irritation;
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
            
        }
    }
}