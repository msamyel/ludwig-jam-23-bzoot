using UnityEngine;
namespace Bzoot
{
    public class CootsPawModel
    {
        public bool IsInUse { get; private set; }

        public void UsePaw()
        {
            Debug.Assert(!IsInUse, "This paw was already in use!");

            IsInUse = true;

            GameSceneEnvironment.Instance.StartDelayedFunction(
                GameSceneEnvironment.Instance.Coots.PawCoolDownSecs,
                SetNotInUse
            );
        }

        void SetNotInUse()
        {
            IsInUse = false;
        }
    }
}