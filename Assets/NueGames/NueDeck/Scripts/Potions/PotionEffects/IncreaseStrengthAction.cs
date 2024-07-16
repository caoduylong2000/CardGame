using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Potion.PotionEffect
{
    public class IncreaseStrengthAction : PotionEffectBase
    {
        public override PotionEffectType ActionType => PotionEffectType.IncreaseStrength;
        public override void DoAction(PotionEffectParameters actionParameters)
        {
            var newTarget = actionParameters.TargetCharacter
                ? actionParameters.TargetCharacter
                : actionParameters.SelfCharacter;

            if (!newTarget) return;
            
            newTarget.CharacterStats.ApplyStatus(StatusType.Strength,Mathf.RoundToInt(actionParameters.Value));
            
            if (FxManager != null) 
                FxManager.PlayFx(newTarget.transform, FxType.Strength);

            if (AudioManager != null) 
                AudioManager.PlayOneShot(actionParameters.PotionData.AudioType);
        }
    }
}