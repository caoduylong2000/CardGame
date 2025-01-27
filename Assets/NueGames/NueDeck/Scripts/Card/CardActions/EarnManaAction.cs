﻿using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NueGames.NueDeck.Scripts.Card.CardActions
{
    public class EarnManaAction : CardActionBase
    {
        public override CardActionType ActionType => CardActionType.EarnMana;
        public override void DoAction(CardActionParameters actionParameters)
        {
            if (SceneManager.GetActiveScene().buildIndex == GameManager.SceneData.tutorialSceneIndex)
            {
                if (TutorialCombatManager != null)
                    TutorialCombatManager.IncreaseMana(Mathf.RoundToInt(actionParameters.Value));
                else
                    Debug.LogError("There is no CombatManager");
            }
            else if (SceneManager.GetActiveScene().buildIndex == GameManager.SceneData.combatSceneIndex)
            {
                if (CombatManager != null)
                    CombatManager.IncreaseMana(Mathf.RoundToInt(actionParameters.Value));
                else
                    Debug.LogError("There is no CombatManager");
            }

            if (FxManager != null)
                FxManager.PlayFx(actionParameters.SelfCharacter.transform, FxType.Mana);
            
            if (AudioManager != null) 
                AudioManager.PlayOneShot(actionParameters.CardData.AudioType);
        }
    }
}