using UnityEngine;
namespace Bzoot
{
    [RequireComponent(typeof(GameModel))]
    public class GameView : MonoBehaviour
    {
        [Header("Object Views")]
        [SerializeField] FlyView Bzoot;
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
        }
    }
}