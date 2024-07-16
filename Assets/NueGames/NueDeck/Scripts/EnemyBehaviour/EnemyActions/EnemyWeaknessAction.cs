using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.EnemyBehaviour.EnemyActions
{
    public class EnemyWeaknessAction : EnemyActionBase
    {
        public override EnemyActionType ActionType => EnemyActionType.Weak;
        public override void DoAction(EnemyActionParameters actionParameters)
        {
            if (!actionParameters.TargetCharacter) return;

            actionParameters.TargetCharacter.CharacterStats.ApplyStatus(StatusType.Weak,Mathf.RoundToInt(actionParameters.Value));
            
            if (FxManager != null) 
                FxManager.PlayFx(actionParameters.TargetCharacter.transform, FxType.Weak);
            
            if (AudioManager != null) 
                AudioManager.PlayOneShot(AudioActionType.Poison);
        }
    }
}