using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Card.CardActions
{
    public class AttackByDrawPileCountAction : CardActionBase
    {
        public override CardActionType ActionType => CardActionType.AttackWithDP;
        public override void DoAction(CardActionParameters actionParameters)
        {
            
            var targetCharacter = actionParameters.TargetCharacter;
            var selfCharacter = actionParameters.SelfCharacter;

            var value = (int)(CollectionManager.Instance.DrawPile.Count * GameManager.Instance.AttackBonus) + selfCharacter.CharacterStats.StatusDict[StatusType.Strength].StatusValue; 

            if (actionParameters.TargetCharacter.CharacterStats.IsVulnerable)
            {
                value *= 2;
            }

            targetCharacter.CharacterStats.Damage(Mathf.RoundToInt(value));

            if (FxManager != null)
            {
                FxManager.PlayFx(actionParameters.TargetCharacter.transform,FxType.Attack);
                FxManager.SpawnFloatingText(actionParameters.TargetCharacter.TextSpawnRoot,value.ToString());
            }
           
            if (AudioManager != null) 
                AudioManager.PlayOneShot(actionParameters.CardData.AudioType);
        }
    }
}