using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Characters;
using UnityEngine;

[System.Serializable]
public class SavedData
{
    public int SavedGold;
    public int SavedHealth;
    public int SavedEncounterId;
    public int SavedYear;
    public int SavedStage;
    public AllyHealthData SavedAllyHealthData;
    public List<CardData> SavedCardList;
    public List<PotionData> SavedPotionList;

    public SavedData(int savedGold, int savedHealth, int savedEncounterId, int savedYear, int savedStage, AllyHealthData savedAllyHealthData,
        List<CardData> savedCardList, List<PotionData> savedPotionList)
    {
        SavedGold = savedGold;
        SavedHealth = savedHealth;
        SavedEncounterId = savedEncounterId;
        SavedYear = savedYear;
        SavedStage = savedStage;
        SavedAllyHealthData = savedAllyHealthData;
        SavedCardList = savedCardList;
        SavedPotionList = savedPotionList;
    }
}
