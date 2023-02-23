using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Bzoot
{
    public class CootsModel
    {
        public readonly CootsEarModel EarOnTheRight = new();
        public readonly CootsEarModel EarOnTheLeft = new();

        public readonly CootsPawModel[] Paws = new[]
        {
            new CootsPawModel(),
            new CootsPawModel()
        };

        float _currentIrritance;
        float _chanceForAttack;

        public Action<float> OnIncreaseCurrentIrritance { set; private get; }
        
        public Action OnAttackPlayer { set; private get; }

        public void Init()
        {
            EarOnTheLeft.OnConfirmIncreaseIrritation = () => IrritateCootsBySoundHittingEars();
            EarOnTheRight.OnConfirmIncreaseIrritation = () => IrritateCootsBySoundHittingEars();
        }
        public void IrritateEarOnTheRight()
        {
            EarOnTheRight.IncreaseIrritanceOnSound();
        }

        public void IrritateEarOnTheLeft()
        {
            EarOnTheLeft.IncreaseIrritanceOnSound();
        }

        void IrritateCootsBySoundHittingEars()
        {
            _currentIrritance += GameSceneEnvironment.Instance.Coots.IncreaseIrritancePerSecondOfCollision * Time.deltaTime;
            OnIncreaseCurrentIrritance.Invoke(_currentIrritance);

            _chanceForAttack += GameSceneEnvironment.Instance.Coots.AttackChancePerSecondOfSoundHittingEars * Time.deltaTime;
        }

        public void IncreaseChanceForAttackByGeneralSound()
        {
            _chanceForAttack += GameSceneEnvironment.Instance.Coots.AttackChancePerSecondOfSound * Time.deltaTime;
        }

        public void CheckIfPawAttackAvailable()
        {
            if (_chanceForAttack < GameSceneEnvironment.Instance.Coots.MinimumAttackChanceToTriggerAttack)
            {
                return;
            }
            
            int randomChance = Random.Range(0, 100);
            
            if (randomChance >= _chanceForAttack*100)
            {
                return;
            }
            
            Debug.Log($"{randomChance} / {_chanceForAttack} is smaller: {randomChance <= _chanceForAttack}");
            
            foreach (var paw in Paws)
            {
                if (paw.IsInUse)
                {
                    continue;
                }
                paw.UsePaw();
                OnAttackPlayer.Invoke();
                _chanceForAttack *= .5f;
                return;
            }
        }
    }
}