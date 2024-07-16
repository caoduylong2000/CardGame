using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Card.CardActions
{
    public class BlockAction : CardActionBase
    {
        public override CardActionType ActionType => CardActionType.Block;
        public override void DoAction(CardActionParameters actionParameters)
        {
            if (!actionParameters.TargetCharacter) return;

            float blockValue = actionParameters.Value;

            if (actionParameters.TargetCharacter.CharacterStats.IsFrail)
            {
                blockValue = actionParameters.Value / 2;
            }

            actionParameters.TargetCharacter.CharacterStats.ApplyStatus(StatusType.Block,Mathf.RoundToInt(blockValue));

            if (FxManager != null) 
                FxManager.PlayFx(actionParameters.TargetCharacter.transform, FxType.Block);
            
            if (AudioManager != null) 
                AudioManager.PlayOneShot(actionParameters.CardData.AudioType);
        }
    }
}