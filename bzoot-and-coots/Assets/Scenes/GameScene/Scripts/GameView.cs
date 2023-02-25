using DG.Tweening;
using UnityEngine;
namespace Bzoot
{
    [RequireComponent(typeof(GameModel))]
    public class GameView : MonoBehaviour
    {
        [Header("Object Views")]
        [SerializeField] PlayerView _bzoot;
        [SerializeField] CootsView _coots;

        [Header("Animations")]
        [SerializeField] CootsLeavingAnimation _cootsLeavingAnimation;

        [Header("UI")]
        [SerializeField] GameSceneUi Ui;

        GameModel _model;

        void Start()
        {
            _model = GetComponent<GameModel>();
            _model.Init();
            _coots.Init();
            BindModelToView();
            BindViewToModel();
            _model.PostInit();

            
            Ui.Init();
        }

        void BindModelToView()
        {
            // bzoot 
            _model.OnUpdateBzootPos = (pos) => _bzoot.UpdatePosition(pos);
            _model.Bzoot.OnCreateSound = () => _bzoot.CreateSound();
            _model.Bzoot.OnUpdateHorizontalSpeedPerSecond = (v) => _bzoot.AnimateHorizontalSpeed(v);
            _model.OnUpdateIsVerticalAcceleration = (v) => _bzoot.AnimateVerticalAcceleration(v);

            _model.OnCloseCootsEarOnTheLeft = () => _coots.EarOnTheLeft.CloseEar();
            _model.OnCloseCootsEarOnTheRight = () => _coots.EarOnTheRight.CloseEar();
            _model.OnOpenCootsEarOnTheLeft = () => _coots.EarOnTheLeft.OpenEar();
            _model.OnOpenCootsEarOnTheRight = () => _coots.EarOnTheRight.OpenEar();

            _model.OnAttackPlayer = (v) => _coots.AttackPlayer(v);
            _model.OnAttackPlayerCrazy = (v) => _coots.CrazyAttack(v);

            //game cycle
            _model.OnPlayPlayerDeadAnimation = (onComplete) => PlayPlayerDeadAnimation(onComplete);
            _model.OnGameOver = Ui.DisplayGameOver;

            _model.OnPlayerWon = HandlePlayerWon;

            // bzoot -> ui
            _model.Bzoot.OnUpdateLivesCount = (v) => Ui.SetLives(v);

            // coots -> ui
            _model.OnUpdateCootsIrritation = (v) => Ui.SetIrritation(v);
        }
        void BindViewToModel()
        {
            //bzoot
            _bzoot.OnGotHit = () => _model.OnPlayerGotHit();

            // coots
            _coots.EarOnTheLeft.OnCollideWithSound = () => _model.Coots.IrritateEarOnTheLeft();
            _coots.EarOnTheRight.OnCollideWithSound = () => _model.Coots.IrritateEarOnTheRight();
            
            //ui events
            Ui.OnRestart = () => _model.RestartGame();
            Ui.OnBackToMenu = () => _model.ReturnToMenu();
        }


        void HandlePlayerWon()
        {
            UnbindOnGameEnd();
            _coots.StopCoroutinesAndCleanObjects();
            _bzoot.DisableCollider();
            _cootsLeavingAnimation.PlayAnimation(onComplete: () =>
            {
                if (Ui)
                {
                    Ui.DisplayPlayerWon();
                }
            });
        }

        void UnbindOnGameEnd()
        {
            _model.Coots.EarOnTheLeft.OnOpenEar = () => { };
            _model.Coots.EarOnTheRight.OnOpenEar = () => { };
        }
        
        //todo: should be it's separate class
        void PlayPlayerDeadAnimation(PlayPlayerDeadAnimationArgs args)
        {
            const float scaleOnDead = 1f;
            const float scaleRegular = .25f;
            
            float playerZ = _bzoot.transform.position.z;

            var values = GameSceneEnvironment.Instance.GameCycle;
            
            var seq = DOTween.Sequence()
                .SetLink(gameObject)
                // get bigger
                .AppendCallback(() => SoundPlayer.Instance.PlaySound(GameSceneEnvironment.Instance.PlayerSmashedAudioClip))
                .Append(_bzoot.transform.DOScale(scaleOnDead, .5f))
                .AppendInterval(1f)
                // slide down
                .AppendCallback(_bzoot.DrawBloodstain)
                .Append(_bzoot.transform.DOLocalMoveY(-8f, 3f).SetRelative());

            if (args.IsRespawn)
            {
                
                // teleport left of screen
                seq.AppendCallback(() => _bzoot.AnimateHorizontalSpeed(.3f))
                    .Append(_bzoot.transform.DOScale(scaleRegular, 0))
                    .Append(_bzoot.transform.DOLocalMove(new Vector3(-8, values.BzootRespawnPosition.y, playerZ), 0))
                    //fly back to scene
                    .Append(_bzoot.transform.DOLocalMoveX(values.BzootRespawnPosition.x, 2f));
            }

            seq.AppendCallback(() => args.OnComplete?.Invoke());
        }
    }
}