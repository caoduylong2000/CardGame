using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NueGames.NueDeck.Scripts.Managers;

public class EventStage : MonoBehaviour
{
    protected GameManager GameManager => GameManager.Instance;
    protected UIManager UIManager => UIManager.Instance;

    #region Event_Action
    public void CostGold(int gold)
    {
        if (GameManager.PersistentGameplayData.CurrentGold >= gold)
        {
            GameManager.PersistentGameplayData.CurrentGold -= gold;
            UIManager.InformationCanvas.SetGoldText(GameManager.PersistentGameplayData.CurrentGold);
        }
        else
            return;
    }

    public void Heal(int heal)
    {
        if (GameManager.PersistentGameplayData.AllyHealthDataList.Count > 0)
        {
            var data = GameManager.PersistentGameplayData.AllyHealthDataList[0];

            data.MaxHealth = (int)(GameManager.PersistentGameplayData.AllyList[0].AllyCharacterData.MaxHealth * GameManager.PlayerHP);

            data.CurrentHealth += heal;

            if (data.CurrentHealth > data.MaxHealth)
            {
                data.CurrentHealth = data.MaxHealth;
            }

            UIManager.InformationCanvas.UpdateHealStats(data.CurrentHealth, data.MaxHealth);
        }
        else
        {
            GameManager.playerMaxHealth = (int)(GameManager.PersistentGameplayData.AllyList[0].AllyCharacterData.MaxHealth * GameManager.PlayerHP);

            GameManager.playerCurrentHealth += heal;

            if (GameManager.playerCurrentHealth > GameManager.playerMaxHealth)
            {
                GameManager.playerCurrentHealth = GameManager.playerMaxHealth;
            }

            UIManager.InformationCanvas.UpdateHealStats(GameManager.playerCurrentHealth, GameManager.playerMaxHealth);
        }

        
    }

    public void IncreaseMaxHealth(int hp)
    {
        if (GameManager.PersistentGameplayData.AllyHealthDataList.Count > 0)
        {
            var data = GameManager.PersistentGameplayData.AllyHealthDataList[0];

            data.MaxHealth = (int)(GameManager.PersistentGameplayData.AllyList[0].AllyCharacterData.MaxHealth * GameManager.PlayerHP);

            data.MaxHealth += hp;

            UIManager.InformationCanvas.UpdateHealStats(data.CurrentHealth, data.MaxHealth);
        }
        else
        {
            GameManager.playerMaxHealth = (int)(GameManager.PersistentGameplayData.AllyList[0].AllyCharacterData.MaxHealth * GameManager.PlayerHP);

            GameManager.playerMaxHealth += hp;

            UIManager.InformationCanvas.UpdateHealStats(GameManager.playerCurrentHealth, GameManager.playerMaxHealth);
        }
    }

    public void LoseHealth(int hp)
    {
        if(GameManager.PersistentGameplayData.AllyHealthDataList.Count > 0)
        {
            var data = GameManager.PersistentGameplayData.AllyHealthDataList[0];

            data.MaxHealth = (int)(GameManager.PersistentGameplayData.AllyList[0].AllyCharacterData.MaxHealth * GameManager.PlayerHP);

            data.CurrentHealth -= hp;

            if (data.CurrentHealth <= 0)
                CombatManager.Instance.LoseCombat();

            UIManager.InformationCanvas.UpdateHealStats(data.CurrentHealth, data.MaxHealth);
        }
        else
        {
            GameManager.playerCurrentHealth -= hp;

            if (GameManager.playerCurrentHealth <= 0)
                CombatManager.Instance.LoseCombat();

            UIManager.InformationCanvas.UpdateHealStats(GameManager.playerCurrentHealth, GameManager.playerMaxHealth);
        }
    }
    #endregion

    public void DeactivateButton(Button button)
    {
        button.interactable = false;
    }
}
