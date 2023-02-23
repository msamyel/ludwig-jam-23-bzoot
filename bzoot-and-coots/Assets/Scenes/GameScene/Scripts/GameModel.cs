using System;
using UnityEngine;
namespace Bzoot
{
    public class GameModel : MonoBehaviour
    {
        public readonly FlyModel Bzoot = new();
        public readonly CootsModel Coots = new();
        
        public Action<Vector2> OnUpdateBzootPos { set; private get; }
        public Action OnCloseCootsEarOnTheRight { set; private get; }
        public Action OnCloseCootsEarOnTheLeft { set; private get; }
        public Action OnOpenCootsEarOnTheRight { set; private get; }
        public Action OnOpenCootsEarOnTheLeft { set; private get; }
        
        public Action<float> OnUpdateCootsIrritation { set; private get; }

        bool _isInitFinished;
        
        public void Init()
        {
            Bzoot.OnUpdatePosition = (pos) => OnUpdateBzootPos(pos);

            Coots.Init();
            Coots.OnIncreaseCurrentIrritance = (v) => OnUpdateCootsIrritation(v);
            
            Coots.EarOnTheRight.OnCloseEar = () => OnCloseCootsEarOnTheRight();
            Coots.EarOnTheLeft.OnCloseEar = () => OnCloseCootsEarOnTheLeft();
            Coots.EarOnTheRight.OnOpenEar = () => OnOpenCootsEarOnTheRight();
            Coots.EarOnTheLeft.OnOpenEar= () => OnOpenCootsEarOnTheLeft();

            _isInitFinished = true;
        }

        void Update()
        {
            if (!_isInitFinished)
            {
                return;
            }
            
            HandleInput();
            
            Bzoot.ApplyGravity();
            Bzoot.ApplyHorizontalDrag();
            Bzoot.Move();
        }

        void HandleInput()
        {
            if (Input.GetKey(KeyCode.W))
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
        }
        
    }
}