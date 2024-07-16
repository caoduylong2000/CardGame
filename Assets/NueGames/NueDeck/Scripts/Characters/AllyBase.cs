using System;
using System.Collections;
using NueGames.NueDeck.Scripts.Data.Characters;
using NueGames.NueDeck.Scripts.Interfaces;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NueGames.NueDeck.Scripts.Characters
{
    public abstract class AllyBase : CharacterBase,IAlly
    {
        [Header("Ally Base Settings")]
        [SerializeField] private AllyCanvas allyCanvas;
        [SerializeField] private AllyCharacterData allyCharacterData;
        public AllyCanvas AllyCanvas => allyCanvas;
        public AllyCharacterData AllyCharacterData => allyCharacterData;        
        
        public override void BuildCharacter()
        {
            base.BuildCharacter();
            allyCanvas.InitCanvas();
            CharacterStats = new CharacterStats(Mathf.RoundToInt(allyCharacterData.MaxHealth * GameManager.PlayerHP),allyCanvas);

            CharacterStats.OnHealthChanged += CharacterStats.UpdateData;
            CharacterStats.OnHealthChanged += UIManager.InformationCanvas.UpdateHealStats;

            if (!GameManager)
                throw new Exception("There is no GameManager");
            
            var data = GameManager.PersistentGameplayData.AllyHealthDataList.Find(x =>
                x.CharacterId == AllyCharacterData.CharacterID);
            
            if (data != null)
            {
                CharacterStats.CurrentHealth = data.CurrentHealth;
                CharacterStats.MaxHealth = data.MaxHealth;
            }
            else
            {
                GameManager.PersistentGameplayData.SetAllyHealthData(AllyCharacterData.CharacterID,CharacterStats.CurrentHealth,CharacterStats.MaxHealth);
            }
            
            CharacterStats.OnDeath += OnDeath;
            CharacterStats.SetCurrentHealth(CharacterStats.CurrentHealth);

            if (SceneManager.GetActiveScene().buildIndex == GameManager.SceneData.tutorialSceneIndex)
            {
                if (TutorialCombatManager != null)
                    TutorialCombatManager.OnAllyTurnStarted += CharacterStats.TriggerAllStatus;
            }
            else if (SceneManager.GetActiveScene().buildIndex == GameManager.SceneData.combatSceneIndex)
            {
                if (CombatManager != null)
                    CombatManager.OnAllyTurnStarted += CharacterStats.TriggerAllStatus;
            }

            
        }

        protected override void OnDeath()
        {
            base.OnDeath();
            if (SceneManager.GetActiveScene().buildIndex == GameManager.SceneData.tutorialSceneIndex)
            {
                TutorialCombatManager.OnAllyTurnStarted -= CharacterStats.TriggerAllStatus;
                TutorialCombatManager.OnAllyDeath(this);
            }
            else if (SceneManager.GetActiveScene().buildIndex == GameManager.SceneData.combatSceneIndex)
            {
                CombatManager.OnAllyTurnStarted -= CharacterStats.TriggerAllStatus;
                CombatManager.OnAllyDeath(this);
            }

            Destroy(gameObject);
        }
    }

    [Serializable]
    public class AllyHealthData
    {
        [SerializeField] private string characterId;
        [SerializeField] private int maxHealth;
        [SerializeField] private int currentHealth;
        
        public int MaxHealth
        {
            get => maxHealth;
            set => maxHealth = value;
        }

        public int CurrentHealth
        {
            get => currentHealth;
            set => currentHealth = value;
        }

        public string CharacterId
        {
            get => characterId;
            set => characterId = value;
        }
    }
}