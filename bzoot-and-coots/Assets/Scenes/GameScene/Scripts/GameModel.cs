using System;
using UnityEngine;
namespace Bzoot
{
    public class GameModel : MonoBehaviour
    {
        public readonly FlyModel Bzoot = new();
        
        public Action<Vector2> OnUpdateBzootPos { set; private get; }
        public void Init()
        {
            Bzoot.OnUpdatePosition = (pos) => OnUpdateBzootPos(pos);
        }

        void Update()
        {
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