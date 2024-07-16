using System.IO;
using UnityEngine;
using NueGames.NueDeck.Scripts.Managers;
using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Characters;

public class SaveSystem : MonoBehaviour
{
    int defaultGold = 100;
    int defaultHealth = 150;
    AllyHealthData defaultAllyHealthData;
    List<CardData> defaultListCard = new List<CardData>();
    List<PotionData> defaultListPotion = new List<PotionData>();

    [HideInInspector] public StageData stageData;

    public void SavePlayerDataToJson()
    {
        SavedData playerData = new SavedData(defaultGold, defaultHealth, 0, 0, 0, defaultAllyHealthData, defaultListCard, defaultListPotion);

        if (GameManager.Instance.PersistentGameplayData == null)
        {
            playerData.SavedEncounterId = 0;
            playerData.SavedGold = 0;
            playerData.SavedHealth = 0;
            playerData.SavedStage = 0;
            playerData.SavedYear = 0;
            playerData.SavedAllyHealthData = null;
            playerData.SavedCardList = null;
            playerData.SavedPotionList = null;
        }
        else
        {
            playerData.SavedEncounterId = GameManager.Instance.PersistentGameplayData.CurrentEncounterId;
            playerData.SavedGold = GameManager.Instance.PersistentGameplayData.CurrentGold;

            if (GameManager.Instance.PersistentGameplayData.AllyHealthDataList.Count > 0)
            {
                playerData.SavedAllyHealthData = GameManager.Instance.PersistentGameplayData.AllyHealthDataList[0];
                playerData.SavedHealth = GameManager.Instance.PersistentGameplayData.AllyHealthDataList[0].CurrentHealth;
            }
            else
            {
                playerData.SavedAllyHealthData = new AllyHealthData();
                playerData.SavedHealth = GameManager.Instance.playerMaxHealth;
            }
                

            playerData.SavedStage = GameManager.Instance.PersistentGameplayData.CurrentEncounterGroup;
            playerData.SavedYear = GameManager.Instance.PersistentGameplayData.CurrentStageId;
            playerData.SavedCardList = GameManager.Instance.PersistentGameplayData.CurrentCardsList;
            playerData.SavedPotionList = GameManager.Instance.PersistentGameplayData.CurrentPotionsList;

        }

        string json = JsonUtility.ToJson(playerData, true);
        string path = Application.persistentDataPath + "/PlayerData.json";
        File.WriteAllText(path, json);
    }

    public void LoadPlayerDataFromJson()
    {
        string path = Application.persistentDataPath + "/PlayerData.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            SavedData playerData = JsonUtility.FromJson<SavedData>(json);

            GameManager.Instance.PersistentGameplayData.CurrentEncounterId = playerData.SavedEncounterId;
            GameManager.Instance.PersistentGameplayData.CurrentGold = playerData.SavedGold;

            if (GameManager.Instance.PersistentGameplayData.AllyHealthDataList.Count > 0)
            {
                GameManager.Instance.PersistentGameplayData.AllyHealthDataList[0].CurrentHealth = playerData.SavedHealth;
                GameManager.Instance.PersistentGameplayData.AllyHealthDataList[0].MaxHealth = GameManager.Instance.playerMaxHealth;
            }
            else
            {
                GameManager.Instance.PersistentGameplayData.AllyHealthDataList.Add(playerData.SavedAllyHealthData);
                GameManager.Instance.PersistentGameplayData.AllyHealthDataList[0].CurrentHealth = playerData.SavedHealth;
                GameManager.Instance.PersistentGameplayData.AllyHealthDataList[0].MaxHealth = GameManager.Instance.playerMaxHealth;
            }

            GameManager.Instance.PersistentGameplayData.CurrentEncounterGroup = playerData.SavedStage;
            GameManager.Instance.PersistentGameplayData.CurrentStageId = playerData.SavedYear;
            GameManager.Instance.PersistentGameplayData.CurrentCardsList = playerData.SavedCardList;
        }
        else
        {
            Debug.Log("Save File not found in " + path);
        }
    }

    public void SaveTrainingDataToJson()
    {
        TrainingData trainingData = new TrainingData("", 0, 0, 0, 0, 0, 0, 0, 0, 0);

        if (GameManager.Instance.PersistentGameplayData == null)
        {
            trainingData.PlayerName = "";
            trainingData.CrystalCount = 0;
            trainingData.AttackBonusLv = 0;
            trainingData.RecoveryBonusLv = 0;
            trainingData.ClosingRateLv = 0;
            trainingData.LegendItemRateLv = 0;
            trainingData.MoneyBonusLv = 0;
            trainingData.DesireBonusLv = 0;
            trainingData.PlayerHPLv = 0;
            trainingData.CrystalRateLv = 0;
        }
        else
        {
            trainingData.PlayerName = GameManager.Instance.playerName;
            trainingData.CrystalCount = GameManager.Instance.PersistentGameplayData.CurrentCrystal;
            trainingData.AttackBonusLv = GameManager.Instance.PersistentGameplayData.AttackBonusLv;
            trainingData.RecoveryBonusLv = GameManager.Instance.PersistentGameplayData.RecoveryBonusLv;
            trainingData.ClosingRateLv = GameManager.Instance.PersistentGameplayData.ClosingRateLv;
            trainingData.LegendItemRateLv = GameManager.Instance.PersistentGameplayData.LegendItemRateLv;
            trainingData.MoneyBonusLv = GameManager.Instance.PersistentGameplayData.MoneyBonusLv;
            trainingData.DesireBonusLv = GameManager.Instance.PersistentGameplayData.DesireBonusLv;
            trainingData.PlayerHPLv = GameManager.Instance.PersistentGameplayData.PlayerHPLv;
            trainingData.CrystalRateLv = GameManager.Instance.PersistentGameplayData.CrystalRateLv;

        }

        string json = JsonUtility.ToJson(trainingData, true);
        string path = Application.persistentDataPath + "/TrainingData.json";
        File.WriteAllText(path, json);
    }

    public void LoadTrainingDataFromJson()
    {
        string path = Application.persistentDataPath + "/TrainingData.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            TrainingData trainingData = JsonUtility.FromJson<TrainingData>(json);

            GameManager.Instance.playerName = trainingData.PlayerName;
            GameManager.Instance.PersistentGameplayData.CurrentCrystal = trainingData.CrystalCount;
            GameManager.Instance.PersistentGameplayData.AttackBonusLv = trainingData.AttackBonusLv;
            GameManager.Instance.PersistentGameplayData.RecoveryBonusLv = trainingData.RecoveryBonusLv;
            GameManager.Instance.PersistentGameplayData.ClosingRateLv = trainingData.ClosingRateLv;
            GameManager.Instance.PersistentGameplayData.LegendItemRateLv = trainingData.LegendItemRateLv;
            GameManager.Instance.PersistentGameplayData.MoneyBonusLv = trainingData.MoneyBonusLv;
            GameManager.Instance.PersistentGameplayData.DesireBonusLv = trainingData.DesireBonusLv;
            GameManager.Instance.PersistentGameplayData.PlayerHPLv = trainingData.PlayerHPLv;
            GameManager.Instance.PersistentGameplayData.CrystalRateLv = trainingData.CrystalRateLv;

            
        }
        else
        {
            GameManager.Instance.PersistentGameplayData.AttackBonusLv = 0;
            GameManager.Instance.PersistentGameplayData.RecoveryBonusLv = 0;
            GameManager.Instance.PersistentGameplayData.ClosingRateLv = 0;
            GameManager.Instance.PersistentGameplayData.LegendItemRateLv = 0;
            GameManager.Instance.PersistentGameplayData.MoneyBonusLv = 0;
            GameManager.Instance.PersistentGameplayData.DesireBonusLv = 0;
            GameManager.Instance.PersistentGameplayData.PlayerHPLv = 0;
            GameManager.Instance.PersistentGameplayData.CrystalRateLv = 0;
        }

        GameManager.Instance.AttackBonus = GameManager.Instance.TrainingIndex.ListTrainingLevelInfo[GameManager.Instance.PersistentGameplayData.AttackBonusLv].ListIndexMul.AttackBonus;
        GameManager.Instance.RecoveryBonus = GameManager.Instance.TrainingIndex.ListTrainingLevelInfo[GameManager.Instance.PersistentGameplayData.RecoveryBonusLv].ListIndexMul.RecoveryBonus;
        GameManager.Instance.ClosingRate = GameManager.Instance.TrainingIndex.ListTrainingLevelInfo[GameManager.Instance.PersistentGameplayData.ClosingRateLv].ListIndexMul.ClosingRate;
        GameManager.Instance.LegendItemRate = GameManager.Instance.TrainingIndex.ListTrainingLevelInfo[GameManager.Instance.PersistentGameplayData.LegendItemRateLv].ListIndexMul.LegendItemRate;
        GameManager.Instance.MoneyBonus = GameManager.Instance.TrainingIndex.ListTrainingLevelInfo[GameManager.Instance.PersistentGameplayData.MoneyBonusLv].ListIndexMul.MoneyBonus;
        GameManager.Instance.DesireBonus = GameManager.Instance.TrainingIndex.ListTrainingLevelInfo[GameManager.Instance.PersistentGameplayData.DesireBonusLv].ListIndexMul.DesireBonus;
        GameManager.Instance.PlayerHP = GameManager.Instance.TrainingIndex.ListTrainingLevelInfo[GameManager.Instance.PersistentGameplayData.PlayerHPLv].ListIndexMul.PlayerHP;
        GameManager.Instance.CrystalRate = GameManager.Instance.TrainingIndex.ListTrainingLevelInfo[GameManager.Instance.PersistentGameplayData.CrystalRateLv].ListIndexMul.CrystalRate;
    }

    public void SaveStageDataToJson()
    {
        StageData stageData =  GameManager.Instance.SavedStageData;

        string jsonData = JsonUtility.ToJson(stageData);
        string path = Application.persistentDataPath + "/StageData.json";
        File.WriteAllText(path, jsonData);
    }

    public void LoadStageDataFromJson()
    {
        string path = Application.persistentDataPath + "/StageData.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            StageData stageData = JsonUtility.FromJson<StageData>(json);

            GameManager.Instance.SavedStageData = stageData;
        }
        else
        {
            Debug.Log("Save File not found in " + path);
        }
    }
}
