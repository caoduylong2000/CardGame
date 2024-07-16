using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Characters;
using UnityEngine;

[System.Serializable]
public class TrainingData
{
    public string PlayerName;

    public int CrystalCount;
    public int AttackBonusLv;
    public int RecoveryBonusLv;
    public int ClosingRateLv;
    public int LegendItemRateLv;
    public int MoneyBonusLv;
    public int DesireBonusLv;
    public int PlayerHPLv;
    public int CrystalRateLv;

    public TrainingData(string playerName, int crystalCount, int attackBonusLv, int recoveryBonusLv, int closingRateLv, int legendItemRateLv, int moneyBonusLv, int desireBonusLv, int playerHPLv, int crystalRateLv)
    {
        PlayerName = playerName;
        CrystalCount = crystalCount;
        AttackBonusLv = attackBonusLv;
        RecoveryBonusLv = recoveryBonusLv;
        ClosingRateLv = closingRateLv;
        LegendItemRateLv = legendItemRateLv;
        MoneyBonusLv = moneyBonusLv;
        DesireBonusLv = desireBonusLv;
        PlayerHPLv = playerHPLv;
        CrystalRateLv = crystalRateLv;
    }
}
