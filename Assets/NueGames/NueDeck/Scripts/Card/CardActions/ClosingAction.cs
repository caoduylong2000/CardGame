using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Card.CardActions
{
    //Thanks to Borjan#1804
    public class ClosingAction : CardActionBase
    {
        public override CardActionType ActionType => CardActionType.Closing;
        public override void DoAction(CardActionParameters actionParameters)
        {
            var value = actionParameters.Value * GameManager.ClosingRate;
            actionParameters.CardBase.Closing(value);
        }
    }
}