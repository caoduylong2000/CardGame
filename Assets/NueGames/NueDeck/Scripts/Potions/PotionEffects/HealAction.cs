using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Potion.PotionEffect
{
    public class HealAction: PotionEffectBase
    {
        public override PotionEffectType ActionType => PotionEffectType.Heal;

        public override void DoAction(PotionEffectParameters actionParameters)
        {
            var newTarget = actionParameters.TargetCharacter
                ? actionParameters.TargetCharacter
                : actionParameters.SelfCharacter;

            if (!newTarget) return;
            
            newTarget.CharacterStats.AllyHeal(Mathf.RoundToInt(actionParameters.Value * GameManager.RecoveryBonus));

            if (FxManager != null) 
                FxManager.PlayFx(newTarget.transform, FxType.Heal);
            
            if (AudioManager != null) 
                AudioManager.PlayOneShot(actionParameters.PotionData.AudioType);
        }
    }
}