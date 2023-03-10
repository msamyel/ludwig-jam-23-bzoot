using System;
using UnityEngine;
namespace Bzoot
{
    public class PlayerModel
    {
        public Vector2 Pos { get; private set; } = new(0, 0);
        public Vector2 SpeedPerSecond { get; private set; }

        public int LivesCount { get; private set; } = 3;

        public bool _isCanCreateSound = true;
        
        public Action<Vector2> OnUpdatePosition {set; private get;}
        public Action<float> OnUpdateHorizontalSpeedPerSecond { set; private get; }
        public Action<int> OnUpdateLivesCount { set; private get; }
        public Action OnCreateSound { set; private get; }

        //call after events have been bound
        public void PostInit()
        {
            OnUpdateLivesCount.Invoke(LivesCount);
        }
        
        public void Move()
        {
            var bounds = GameSceneEnvironment.Instance.PlayArea;
            var newPosition = Pos + SpeedPerSecond * Time.deltaTime;
            //todo: smoothstep to slow down player
            if (!bounds.Contains(newPosition))
            {
                SlowDownWhenReachedBounds(newPosition, bounds);
                return;
            }

            Pos = newPosition;
            OnUpdatePosition.Invoke(Pos);
        }
        
        void SlowDownWhenReachedBounds(Vector2 newPos, Bounds bounds)
        {
            //slow down vertically
            if (!bounds.Contains(new Vector2(0, newPos.y)))
            {
                SpeedPerSecond *= new Vector2(1, .5f * Time.deltaTime);
                Pos = new Vector2(newPos.x, Pos.y);
            }
            //slow down horizontally
            if (!bounds.Contains(new Vector2(newPos.x, 0)))
            {
                SpeedPerSecond *= new Vector2(.5f * Time.deltaTime, 1);
                Pos = new Vector2(Pos.x, newPos.y);
            }
            OnUpdatePosition.Invoke(Pos);
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
            
            OnUpdateHorizontalSpeedPerSecond.Invoke(SpeedPerSecond.x);
        }

        public void ApplyVerticalAcceleration()
        {
            float AccPerSecond = GameSceneEnvironment.Instance.Physics.VerticalAccPerSecond;
            float MaxVerticalSpeedPerSecond = GameSceneEnvironment.Instance.Physics.MaxVerticalSpeedPerSecond;

            float acceleration = AccPerSecond * Time.deltaTime;

            float speed = Math.Min(SpeedPerSecond.y + acceleration, MaxVerticalSpeedPerSecond);
            SpeedPerSecond = new Vector2(SpeedPerSecond.x, speed);
        }

        public void CreateSound()
        {
            if (!_isCanCreateSound)
            {
                return;
            }
            OnCreateSound.Invoke();
            _isCanCreateSound = false;
            
            GameSceneEnvironment.Instance.StartDelayedFunction(
                GameSceneEnvironment.Instance.Physics.CreateSoundCooldownSecs,
                ReenableSound);
        }

        void ReenableSound()
        {
            _isCanCreateSound = true;
        }

        public void RemoveLife()
        {
            LivesCount--;
            OnUpdateLivesCount.Invoke(LivesCount);
        }

        public void RestartOnResume()
        {
            SpeedPerSecond = new Vector2(0, 0);
            Pos = GameSceneEnvironment.Instance.GameCycle.BzootRespawnPosition;
            _isCanCreateSound = true;
        }
    }
}