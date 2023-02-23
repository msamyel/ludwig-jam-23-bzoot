using System;
using UnityEngine;
namespace Bzoot
{
    public class CootsEarModel
    {
        float _irritance;

        bool _isEarOpen = true;

        public Action OnCloseEar { set; private get; }
        public Action OnOpenEar { set; private get; }
        
        public Action OnConfirmIncreaseIrritation { set; private get; }

        public void IncreaseIrritanceOnSound()
        {
            if (!_isEarOpen)
            {
                return;
            }
            _irritance += GameSceneEnvironment.Instance.Coots.IncreaseIrritancePerSecondOfCollision * Time.deltaTime;
            OnConfirmIncreaseIrritation.Invoke();

            if (_irritance > GameSceneEnvironment.Instance.Coots.MaxEarSoundTolerance)
            {
                CloseEar();
            }
        }

        void CloseEar()
        {
            _isEarOpen = false;
            OnCloseEar.Invoke();
            GameSceneEnvironment.Instance.StartDelayedFunction(
                GameSceneEnvironment.Instance.Coots.EarClosedIntervalSecs,
                ResetEar
            );
        }

        void ResetEar()
        {
            _irritance = 0f;
            _isEarOpen = true;
            OnOpenEar.Invoke();
        }
    }
}