using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Card.CardActions
{
    public class WeaknessAction : CardActionBase
    {
        public override CardActionType ActionType => CardActionType.Weak;
        public override void DoAction(CardActionParameters actionParameters)
        {
            if (!actionParameters.TargetCharacter) return;

            actionParameters.TargetCharacter.CharacterStats.ApplyStatus(StatusType.Weak,Mathf.RoundToInt(actionParameters.Value));

            if (FxManager != null)
                FxManager.PlayFx(actionParameters.TargetCharacter.transform,FxType.Weak);
           
            if (AudioManager != null) 
                AudioManager.PlayOneShot(actionParameters.CardData.AudioType);
        }
    }
}