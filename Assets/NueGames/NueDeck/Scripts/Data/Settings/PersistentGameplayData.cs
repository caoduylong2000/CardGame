using System;
using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Data.Collection;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Settings
{
    [Serializable]
    public class PersistentGameplayData
    {
        private readonly GameplayData _gameplayData;
        
        [SerializeField] private int currentGold;
        [SerializeField] private int currentCrystal;
        [SerializeField] private int drawCount;
        [SerializeField] private int maxMana;
        [SerializeField] private int currentMana;
        [SerializeField] private bool canUseCards;
        [SerializeField] private bool canSelectCards;
        [SerializeField] private bool isRandomHand;
        [SerializeField] private List<AllyBase> allyList;
        [SerializeField] private int currentStageId;
        [SerializeField] private int currentEncounterId;
        [SerializeField] private int currentEncounterGroup;
        [SerializeField] private bool isFinalEncounter;
        [SerializeField] private List<CardData> currentCardsList;
        [SerializeField] private List<PotionData> currentPotionsList;
        [SerializeField] private List<RelicData> currentRelicsList;
        [SerializeField] private List<AllyHealthData> allyHealthDataList;
        [SerializeField] private int attackBonusLv;
        [SerializeField] private int recoveryBonusLv;
        [SerializeField] private int closingRateLv;
        [SerializeField] private int legendItemRateLv;
        [SerializeField] private int moneyBonusLv;
        [SerializeField] private int desireBonusLv;
        [SerializeField] private int playerHPLv;
        [SerializeField] private int crystalRateLv;

        public PersistentGameplayData(GameplayData gameplayData)
        {
            _gameplayData = gameplayData;

            InitData();
        }
        
        public void SetAllyHealthData(string id,int newCurrentHealth, int newMaxHealth)
        {
            var data = allyHealthDataList.Find(x => x.CharacterId == id);
            var newData = new AllyHealthData();
            newData.CharacterId = id;
            newData.CurrentHealth = newCurrentHealth;
            newData.MaxHealth = newMaxHealth;
            if (data != null)
            {
                allyHealthDataList.Remove(data);
                allyHealthDataList.Add(newData);
            }
            else
            {
                allyHealthDataList.Add(newData);
            }
        } 
        public void InitData()
        {
            DrawCount = _gameplayData.DrawCount;
            MaxMana = _gameplayData.MaxMana;
            CurrentMana = MaxMana;
            CanUseCards = true;
            CanSelectCards = true;
            IsRandomHand = _gameplayData.IsRandomHand;
            AllyList = new List<AllyBase>(_gameplayData.InitalAllyList);
            CurrentEncounterId = 0;
            CurrentEncounterGroup = 0;
            CurrentStageId = 0;
            CurrentGold = 100;
            CurrentCardsList = new List<CardData>();
            CurrentPotionsList = new List<PotionData>();
            CurrentRelicsList = new List<RelicData>();
            IsFinalEncounter = false;
            AllyHealthDataList = new List<AllyHealthData>();
            AttackBonusLv = 0;
            RecoveryBonusLv = 0;
            ClosingRateLv = 0;
            LegendItemRateLv = 0;
            MoneyBonusLv = 0;
            DesireBonusLv = 0;
            PlayerHPLv = 0;
            CrystalRateLv = 0;


            //For test final Stage
            //DrawCount = _gameplayData.DrawCount;
            //MaxMana = _gameplayData.MaxMana;
            //CurrentMana = MaxMana;
            //CanUseCards = true;
            //CanSelectCards = true;
            //IsRandomHand = _gameplayData.IsRandomHand;
            //AllyList = new List<AllyBase>(_gameplayData.InitalAllyList);
            //CurrentEncounterId = 0;
            //CurrentEncounterGroup = 0;
            //CurrentStageId = 0;
            //CurrentGold = 1000;
            //CurrentCardsList = new List<CardData>();
            //CurrentPotionsList = new List<PotionData>();
            //IsFinalEncounter = false;
            //AllyHealthDataList = new List<AllyHealthData>();
            //AttackBonusLv = 0;
            //RecoveryBonusLv = 0;
            //ClosingRateLv = 0;
            //LegendItemRateLv = 0;
            //MoneyBonusLv = 0;
            //DesireBonusLv = 0;
            //PlayerHPLv = 0;
            //CrystalRateLv = 0;
        }

        #region Encapsulation

        public int DrawCount
        {
            get => drawCount;
            set => drawCount = value;
        }

        public int MaxMana
        {
            get => maxMana;
            set => maxMana = value;
        }

        public int CurrentMana
        {
            get => currentMana;
            set => currentMana = value;
        }

        public bool CanUseCards
        {
            get => canUseCards;
            set => canUseCards = value;
        }

        public bool CanSelectCards
        {
            get => canSelectCards;
            set => canSelectCards = value;
        }

        public bool IsRandomHand
        {
            get => isRandomHand;
            set => isRandomHand = value;
        }

        public List<AllyBase> AllyList
        {
            get => allyList;
            set => allyList = value;
        }

        public int CurrentStageId
        {
            get => currentStageId;
            set => currentStageId = value;
        }

        public int CurrentEncounterId
        {
            get => currentEncounterId;
            set => currentEncounterId = value;
        }

        public int CurrentEncounterGroup
        {
            get => currentEncounterGroup;
            set => currentEncounterGroup = value;
        }

        public bool IsFinalEncounter
        {
            get => isFinalEncounter;
            set => isFinalEncounter = value;
        }

        public List<CardData> CurrentCardsList
        {
            get => currentCardsList;
            set => currentCardsList = value;
        }

        public List<PotionData> CurrentPotionsList
        {
            get => currentPotionsList;
            set => currentPotionsList = value;
        }

        public List<RelicData> CurrentRelicsList
        {
            get => currentRelicsList;
            set => currentRelicsList = value;
        }

        public List<AllyHealthData> AllyHealthDataList
        {
            get => allyHealthDataList;
            set => allyHealthDataList = value;
        }

        public int CurrentGold
        {
            get => currentGold;
            set => currentGold = value;
        }

        public int CurrentCrystal
        {
            get => currentCrystal;
            set => currentCrystal = value;
        }

        public int AttackBonusLv
        {
            get => attackBonusLv;
            set => attackBonusLv = value;
        }

        public int RecoveryBonusLv
        {
            get => recoveryBonusLv;
            set => recoveryBonusLv = value;
        }

        public int ClosingRateLv
        {
            get => closingRateLv;
            set => closingRateLv = value;
        }

        public int LegendItemRateLv
        {
            get => legendItemRateLv;
            set => legendItemRateLv = value;
        }

        public int MoneyBonusLv
        {
            get => moneyBonusLv;
            set => moneyBonusLv = value;
        }

        public int DesireBonusLv
        {
            get => desireBonusLv;
            set => desireBonusLv = value;
        }

        public int PlayerHPLv
        {
            get => playerHPLv;
            set => playerHPLv = value;
        }

        public int CrystalRateLv
        {
            get => crystalRateLv;
            set => crystalRateLv = value;
        }

        #endregion
    }
}