using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Card.CardActions
{
    public class LoseHealthAction : CardActionBase
    {
        public override CardActionType ActionType => CardActionType.LoseHealth;

        public override void DoAction(CardActionParameters actionParameters)
        {
            var newTarget = actionParameters.TargetCharacter
                ? actionParameters.TargetCharacter
                : actionParameters.SelfCharacter;

            if (!newTarget) return;
            
            newTarget.CharacterStats.LoseHealth(Mathf.RoundToInt(actionParameters.Value));

            if (FxManager != null) 
                FxManager.PlayFx(newTarget.transform, FxType.Attack);
            
            if (AudioManager != null) 
                AudioManager.PlayOneShot(actionParameters.CardData.AudioType);
        }
    }
}