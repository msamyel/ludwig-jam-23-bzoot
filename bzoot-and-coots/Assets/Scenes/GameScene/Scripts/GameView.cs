using UnityEngine;
namespace Bzoot
{
    [RequireComponent(typeof(GameModel))]
    public class GameView : MonoBehaviour
    {
        [Header("Object Views")]
        [SerializeField] FlyView Bzoot;
        
        GameModel _model;

        void Awake()
        {
            _model = GetComponent<GameModel>();
            _model.Init();
            _model.OnUpdateBzootPos = (pos) => Bzoot.UpdatePosition(pos);
        }
    }
}