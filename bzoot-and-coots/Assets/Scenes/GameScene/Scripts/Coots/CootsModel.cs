using System;
using UnityEngine;
namespace Bzoot
{
    public class CootsModel
    {
        public readonly CootsEarModel EarOnTheRight = new();
        public readonly CootsEarModel EarOnTheLeft = new();

        float _currentIrritance;

        public Action<float> OnIncreaseCurrentIrritance { set; private get; }

        public void Init()
        {
            EarOnTheLeft.OnConfirmIncreaseIrritation = () => IrritateCoots();
            EarOnTheRight.OnConfirmIncreaseIrritation = () => IrritateCoots();
        }
        public void IrritateEarOnTheRight()
        {
            EarOnTheRight.IncreaseIrritanceOnSound();
        }

        public void IrritateEarOnTheLeft()
        {
            EarOnTheLeft.IncreaseIrritanceOnSound();
        }

        void IrritateCoots()
        {
            _currentIrritance += GameSceneEnvironment.Instance.Coots.IncreaseIrritancePerSecondOfCollision * Time.deltaTime;
            OnIncreaseCurrentIrritance.Invoke(_currentIrritance);
        }
    }
}
