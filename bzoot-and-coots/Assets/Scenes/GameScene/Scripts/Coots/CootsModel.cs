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

        float _currentIrritation;
        float _chanceForAttack;

        bool _isAttackOnCooldown;

        CombatPhase _currentCombatPhase = CombatPhase.FirstPhase;

        public Action<float> OnUpdateCurrentIrritance { set; private get; }
        
        public Action OnAttackPlayer { set; private get; }
        public Action OnStartCrazyAttack { set; private get; }
        
        public Action OnIrritationMax { set; private get; }

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
            float newIrritation = _currentIrritation + GameSceneEnvironment.Instance.Coots.IncreaseIrritancePerSecondOfCollision * Time.deltaTime;
            SetIrritation(newIrritation);
            _chanceForAttack += GameSceneEnvironment.Instance.Coots.AttackChancePerSecondOfSoundHittingEars * Time.deltaTime;
        }

        public void IncreaseChanceForAttackByGeneralSound()
        {
            _chanceForAttack += GameSceneEnvironment.Instance.Coots.AttackChancePerSecondOfSound * Time.deltaTime;
        }

        public void StartAttackIfAvailable()
        {
            if (_isAttackOnCooldown)
            {
                return;
            }
            
            if (_currentCombatPhase >= CombatPhase.LastPhase)
            {
                StartOnePawAttack();
                return;
            }
            
            if (_chanceForAttack < GameSceneEnvironment.Instance.Coots.MinimumAttackChanceToTriggerAttack)
            {
                return;
            }
            
            float randomChance = Random.Range(0f, 1f);
            
            if (randomChance >= _chanceForAttack)
            {
                return;
            }
            
            if (_currentCombatPhase >= CombatPhase.CrazyAttackPhase)
            {
                float crazyAttackRandomChance = Random.Range(0f, 1f);
                if (crazyAttackRandomChance <= GameSceneEnvironment.Instance.Coots.CrazyAttackDuringSecondPhaseChance)
                {
                    StartCrazyAttack();
                    return;
                }
                StartOnePawAttack();
                return;
            }
            if (_currentCombatPhase >= CombatPhase.FirstPhase)
            {
                StartOnePawAttack();
            }
        }

        void StartCooldown()
        {
            _isAttackOnCooldown = true;
            GameSceneEnvironment.Instance.StartDelayedFunction(
                GameSceneEnvironment.Instance.Coots.DelayBetweenIndividualAttacks,
                AttackCooldown);
        }
        
        void AttackCooldown()
        {
            _isAttackOnCooldown = false;
        }

        void StartOnePawAttack()
        {
            Debug.Log("StartOnePawAttack");
            foreach (var paw in Paws)
            {
                if (paw.IsInUse)
                {
                    continue;
                }
                paw.UsePaw();
                StartCooldown();
                
                OnAttackPlayer.Invoke();
                _chanceForAttack *= .5f;
                return;
            }
        }

        void StartCrazyAttack()
        {
            _chanceForAttack = 0;
            StartCooldown();
            OnStartCrazyAttack.Invoke();
        }

        public void ResetValuesOnPlayerRespawn()
        {
            _chanceForAttack = 0;
            _isAttackOnCooldown = false;
            EarOnTheLeft.ResetEar();
            EarOnTheRight.ResetEar();
            float newIrritance = _currentIrritation - GameSceneEnvironment.Instance.Coots.DecreaseIrritanceRatioOnHitPlayer;
            SetIrritation(newIrritance);
        }

        void SetIrritation(float newValue)
        {
            if (newValue < 0)
            {
                _currentIrritation = 0;
            }
            else
            {
                _currentIrritation = newValue;
            }
            OnUpdateCurrentIrritance.Invoke(_currentIrritation);
            UpdateCombatPhase();
        }

        void UpdateCombatPhase()
        {
            if (_currentIrritation >= 1f)
            {
                OnIrritationMax.Invoke();
                return;
            }
            _currentCombatPhase = _currentIrritation switch
            {
                < .33f => CombatPhase.FirstPhase,
                < .66f => CombatPhase.CrazyAttackPhase,
                _ => CombatPhase.LastPhase
                };
        }

        enum CombatPhase
        {
            FirstPhase,
            CrazyAttackPhase,
            LastPhase
        }
    }
}