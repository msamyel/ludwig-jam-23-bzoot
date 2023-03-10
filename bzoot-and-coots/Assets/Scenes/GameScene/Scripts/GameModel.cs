using System;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Bzoot
{
    public class GameModel : MonoBehaviour
    {
        public readonly PlayerModel Bzoot = new();
        public readonly CootsModel Coots = new();

        public Action<Vector2> OnUpdateBzootPos { set; private get; }
        public Action OnCloseCootsEarOnTheRight { set; private get; }
        public Action OnCloseCootsEarOnTheLeft { set; private get; }
        public Action OnOpenCootsEarOnTheRight { set; private get; }
        public Action OnOpenCootsEarOnTheLeft { set; private get; }

        public Action<float> OnUpdateCootsIrritation { set; private get; }

        public Action<bool> OnUpdateIsVerticalAcceleration { set; private get; }

        public Action<Vector2> OnAttackPlayer { set; private get; }
        public Action<Vector2> OnAttackPlayerCrazy { set; private get; }
        public Action OnPlayerWon { set; private get; }
        public Action OnGameOver { set; private get; }

        public Action<PlayPlayerDeadAnimationArgs> OnPlayPlayerDeadAnimation { set; private get; }

        bool _isInitFinished;

        bool _isGameSuspended;

        public void Init()
        {
            Bzoot.OnUpdatePosition = (pos) => OnUpdateBzootPos(pos);

            Coots.Init();
            Coots.OnUpdateCurrentIrritance = (v) => OnUpdateCootsIrritation(v);
            Coots.OnAttackPlayer = AttackPlayer;
            Coots.OnStartCrazyAttack = AttackPlayerCrazy;

            Coots.OnIrritationMax = PlayerWon;

            Coots.EarOnTheRight.OnCloseEar = () => OnCloseCootsEarOnTheRight();
            Coots.EarOnTheLeft.OnCloseEar = () => OnCloseCootsEarOnTheLeft();
            Coots.EarOnTheRight.OnOpenEar = () => OnOpenCootsEarOnTheRight();
            Coots.EarOnTheLeft.OnOpenEar = () => OnOpenCootsEarOnTheLeft();

            _isInitFinished = true;
        }

        public void PostInit()
        {
            Bzoot.PostInit();
            PlayGameReadySound();
        }

        void Update()
        {
            if (!_isInitFinished)
            {
                return;
            }

            if (_isGameSuspended)
            {
                return;
            }

            HandleInput();

            Bzoot.ApplyGravity();
            Bzoot.ApplyHorizontalDrag();
            Bzoot.Move();

            Coots.StartAttackIfAvailable();
        }

        void HandleInput()
        {
            bool isAcceleration = Input.GetKey(KeyCode.W);
            OnUpdateIsVerticalAcceleration.Invoke(isAcceleration);
            if (isAcceleration)
            {
                Bzoot.ApplyVerticalAcceleration();
            }
            if (Input.GetKey(KeyCode.A))
            {
                Bzoot.ApplyHorizontalAcceleration(-1);
            }
            if (Input.GetKey(KeyCode.D))
            {
                Bzoot.ApplyHorizontalAcceleration(1);
            }
            if (Input.GetKey(KeyCode.Space))
            {
                //todo: create sound from Bzoot
                Bzoot.CreateSound();
                Coots.IncreaseChanceForAttackByGeneralSound();
            }
            // if (Input.GetKey(KeyCode.Q))
            // {
            //     PlayerWon();
            // }
        }

        public void OnPlayerGotHit()
        {
            if (_isGameSuspended)
            {
                return;
            }
            Debug.Log("YOU ARE DEAD");
            Bzoot.RemoveLife();
            Coots.ResetValuesOnPlayerRespawn();
            _isGameSuspended = true;

            if (Bzoot.LivesCount <= 0)
            {
                OnPlayPlayerDeadAnimation.Invoke(
                    new PlayPlayerDeadAnimationArgs(
                        IsRespawn: false,
                        OnComplete: OnGameOver));
                return;
            }
            OnPlayPlayerDeadAnimation.Invoke(new PlayPlayerDeadAnimationArgs(
                IsRespawn: true,
                OnComplete: ResumeGameOnPlayerRespawn));
        }

        void AttackPlayer()
        {
            OnAttackPlayer.Invoke(Bzoot.Pos);
        }

        void AttackPlayerCrazy()
        {
            OnAttackPlayerCrazy.Invoke(Bzoot.Pos);
        }

        void ResumeGameOnPlayerRespawn()
        {
            Debug.Log("Resume game");
            _isGameSuspended = false;
            PlayGameReadySound();
            Bzoot.RestartOnResume();
        }

        void PlayerWon()
        {
            _isGameSuspended = true;
            OnPlayerWon.Invoke();
        }

        //for now just play single meows sound to signalize that the match has started
        void PlayGameReadySound()
        {
            SoundPlayer.Instance.PlayRandomSound(GameSceneEnvironment.Instance.CootsMeowAudioClips);
        }

        public void RestartGame()
            => SceneManager.LoadScene(Constants.Scene.GameScene);

        public void ReturnToMenu()
            => SceneManager.LoadScene(Constants.Scene.MenuScene);
    }
}