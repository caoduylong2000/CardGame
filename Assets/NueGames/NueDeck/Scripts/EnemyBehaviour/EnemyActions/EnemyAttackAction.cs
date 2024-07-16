﻿using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.EnemyBehaviour.EnemyActions
{
    public class EnemyAttackAction: EnemyActionBase
    {
        public override EnemyActionType ActionType => EnemyActionType.Attack;
        
        public override void DoAction(EnemyActionParameters actionParameters)
        {
            if (!actionParameters.TargetCharacter) return;
            var value = Mathf.RoundToInt(actionParameters.Value);

            if (actionParameters.TargetCharacter.CharacterStats.IsVulnerable)
            {
                value *= 2;
            }

            if (actionParameters.SelfCharacter.CharacterStats.IsWeak)
            {
                value /= 2;
            }

            actionParameters.TargetCharacter.CharacterStats.Damage(value);


            if (FxManager != null)
            {
                FxManager.PlayFx(actionParameters.TargetCharacter.transform,FxType.Attack);
                FxManager.SpawnFloatingText(actionParameters.TargetCharacter.TextSpawnRoot,value.ToString());
            }

            if (AudioManager != null)
                AudioManager.PlayOneShot(AudioActionType.Attack);
           
        }
    }
}