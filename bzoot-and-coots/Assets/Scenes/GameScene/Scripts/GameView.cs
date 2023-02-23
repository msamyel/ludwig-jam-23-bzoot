using System;
using DG.Tweening;
using UnityEngine;
namespace Bzoot
{
    [RequireComponent(typeof(GameModel))]
    public class GameView : MonoBehaviour
    {
        [Header("Object Views")]
        [SerializeField] PlayerView Bzoot;
        [SerializeField] CootsView Coots;

        [Header("UI")]
        [SerializeField] GameSceneUi Ui;

        GameModel _model;

        void Start()
        {
            _model = GetComponent<GameModel>();
            _model.Init();
            BindModelToView();
            BindViewToModel();
            _model.PostInit();

            Ui.Init();
        }

        void BindModelToView()
        {
            // bzoot 
            _model.OnUpdateBzootPos = (pos) => Bzoot.UpdatePosition(pos);

            _model.OnCloseCootsEarOnTheLeft = () => Coots.EarOnTheLeft.CloseEar();
            _model.OnCloseCootsEarOnTheRight = () => Coots.EarOnTheRight.CloseEar();
            _model.OnOpenCootsEarOnTheLeft = () => Coots.EarOnTheLeft.OpenEar();
            _model.OnOpenCootsEarOnTheRight = () => Coots.EarOnTheRight.OpenEar();

            _model.OnAttackPlayer = (v) => Coots.AttackPlayer(v);

            //game cycle
            _model.OnPlayPlayerDeadAnimation = (onComplete) => PlayPlayerDeadAnimation(onComplete);
            _model.OnGameOver = Ui.DisplayGameOver;

            // bzoot -> ui
            _model.Bzoot.OnUpdateLivesCount = (v) => Ui.SetLives(v);

            // coots -> ui
            _model.OnUpdateCootsIrritation = (v) => Ui.SetIrritation(v);
        }
        void BindViewToModel()
        {
            //bzoot
            Bzoot.OnGotHit = () => _model.OnPlayerGotHit();

            // coots
            Coots.EarOnTheLeft.OnCollideWithSound = () => _model.Coots.IrritateEarOnTheLeft();
            Coots.EarOnTheRight.OnCollideWithSound = () => _model.Coots.IrritateEarOnTheRight();
            
            //ui events
            Ui.OnRestart = () => _model.RestartGame();
            Ui.OnBackToMenu = () => _model.ReturnToMenu();
        }

        void PlayPlayerDeadAnimation(PlayPlayerDeadAnimationArgs args)
        {
            float playerZ = Bzoot.transform.position.z;

            var values = GameSceneEnvironment.Instance.GameCycle;
            
            var seq = DOTween.Sequence()
                .SetLink(gameObject)
                // get bigger
                .Append(Bzoot.transform.DOScale(2f, .5f))
                .AppendInterval(1f)
                // slide down
                .Append(Bzoot.transform.DOLocalMoveY(-8f, 5f).SetRelative());

            if (args.IsRespawn)
            {
                // teleport left of screen
                seq.Append(Bzoot.transform.DOScale(1f, 0))
                    .Append(Bzoot.transform.DOLocalMove(new Vector3(-8, values.BzootRespawnPosition.y, playerZ), 0))
                    //fly back to scene
                    .Append(Bzoot.transform.DOLocalMoveX(values.BzootRespawnPosition.x, 2f));
            }

            seq.AppendCallback(() => args.OnComplete?.Invoke());
        }
    }
}