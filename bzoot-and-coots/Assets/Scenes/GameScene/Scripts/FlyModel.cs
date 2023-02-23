using System;
using UnityEngine;
namespace Bzoot
{
    public class FlyModel 
    {
        public Vector2 Pos { get; private set; }
        public Vector2 SpeedPerSecond { get; private set; }
        
        public Action<Vector2> OnUpdatePosition {set; private get;}

        public void Move()
        {
            var bounds = GameSceneEnvironment.Instance.PlayArea;
            var newPosition = Pos + SpeedPerSecond * Time.deltaTime;
            //todo: smoothstep to slow down player
            if (!bounds.Contains(newPosition))
            {
                //todo: determine if slow down verticaly or horizontaly
                SlowDownVertically();
                return;
            }

            Pos = newPosition;
            OnUpdatePosition.Invoke(Pos);
        }

        void SlowDownVertically()
        {
            SpeedPerSecond *= new Vector2(1, .5f);
        }

        public void ApplyGravity()
        {
            float GravityAccPerSecond = GameSceneEnvironment.Instance.Physics.GravityAccPerSecond;
            float MaxFallingSpeedPerSecond = GameSceneEnvironment.Instance.Physics.MaxFallingSpeedPerSecond;

            float gravityAcc = GravityAccPerSecond * Time.deltaTime;

            float speed = -Mathf.Min(-(SpeedPerSecond.y - gravityAcc), MaxFallingSpeedPerSecond);
            SpeedPerSecond = new Vector2(SpeedPerSecond.x, speed);
        }

        public void ApplyHorizontalAcceleration(int direction)
        {
            Debug.Assert(direction is -1 or 1 or 0, "Direction must be -1, 1 or 0");
            float horizontalAccPerSecond = GameSceneEnvironment.Instance.Physics.HorizontalAccPerSecond;
            float maxHorizontalSpeedPerSecond = GameSceneEnvironment.Instance.Physics.MaxHorizontalSpeedPerSecond;

            float acc = horizontalAccPerSecond * Time.deltaTime * direction;

            float speed = Math.Min(Math.Max(SpeedPerSecond.x + acc, -maxHorizontalSpeedPerSecond), maxHorizontalSpeedPerSecond);
            SpeedPerSecond = new Vector2(speed, SpeedPerSecond.y);
        }

        public void ApplyHorizontalDrag()
        {
            float drag = Mathf.Lerp(
                1,
                GameSceneEnvironment.Instance.Physics.HorizontalDragPerSecond,
                Time.deltaTime);
            SpeedPerSecond *= new Vector2(drag, 1f);
        }

        public void ApplyVerticalAcceleration()
        {
            float AccPerSecond = GameSceneEnvironment.Instance.Physics.VerticalAccPerSecond;
            float MaxVerticalSpeedPerSecond = GameSceneEnvironment.Instance.Physics.MaxVerticalSpeedPerSecond;

            float acceleration = AccPerSecond * Time.deltaTime;

            float speed = Math.Min(SpeedPerSecond.y + acceleration, MaxVerticalSpeedPerSecond);
            SpeedPerSecond = new Vector2(SpeedPerSecond.x, speed);
        }
    }
}