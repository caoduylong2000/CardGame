using TMPro;
using NueGames.NueDeck.Scripts.Managers;
using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;

public class IndexMulObject : MonoBehaviour
{
    public TrainingIndex trainingIndex;

    public TextMeshProUGUI title;
    public TextMeshProUGUI level;
    public TextMeshProUGUI indexMul;
    public GameObject lvUpButton;

    public GameObject confirmPanel;
    public TextMeshProUGUI confirmMessage;
    public Button upgradeButton;
    private int indexLv;
    private int indexCrystal;

    private void FixedUpdate()
    {
        UpdateTrainingIndex();
    }

    private void UpdateTrainingIndex()
    {
        switch (trainingIndex)
        {
            case TrainingIndex.AttackBonus:
                title.text = "Attack Bonus";
                level.text = "Lv. " + GameManager.Instance.PersistentGameplayData.AttackBonusLv;
                indexMul.text = GameManager.Instance.TrainingIndex.ListTrainingLevelInfo[GameManager.Instance.PersistentGameplayData.AttackBonusLv].ListIndexMul.AttackBonus + "x";
                if (GameManager.Instance.PersistentGameplayData.CurrentCrystal < GameManager.Instance.TrainingIndex.ListTrainingLevelInfo[GameManager.Instance.PersistentGameplayData.AttackBonusLv + 1].RequireCrystal
                    || GameManager.Instance.PersistentGameplayData.AttackBonusLv >= 21)
                {
                    lvUpButton.GetComponentInChildren<Button>().interactable = false;
                }
                break;

            case TrainingIndex.RecoveryBonus:
                title.text = "Recovery Bonus";
                level.text = "Lv. " + GameManager.Instance.PersistentGameplayData.RecoveryBonusLv;
                indexMul.text = GameManager.Instance.TrainingIndex.ListTrainingLevelInfo[GameManager.Instance.PersistentGameplayData.RecoveryBonusLv].ListIndexMul.RecoveryBonus + "x";
                if (GameManager.Instance.PersistentGameplayData.CurrentCrystal < GameManager.Instance.TrainingIndex.ListTrainingLevelInfo[GameManager.Instance.PersistentGameplayData.RecoveryBonusLv + 1].RequireCrystal
                    || GameManager.Instance.PersistentGameplayData.RecoveryBonusLv >= 21)

                {
                    lvUpButton.GetComponentInChildren<Button>().interactable = false;
                }
                break;

            case TrainingIndex.ClosingRate:
                title.text = "Closing Rate";
                level.text = "Lv. " + GameManager.Instance.PersistentGameplayData.ClosingRateLv;
                indexMul.text = GameManager.Instance.TrainingIndex.ListTrainingLevelInfo[GameManager.Instance.PersistentGameplayData.ClosingRateLv].ListIndexMul.ClosingRate + "x";
                if (GameManager.Instance.PersistentGameplayData.CurrentCrystal < GameManager.Instance.TrainingIndex.ListTrainingLevelInfo[GameManager.Instance.PersistentGameplayData.ClosingRateLv + 1].RequireCrystal
                    || GameManager.Instance.PersistentGameplayData.ClosingRateLv >= 21)
                {
                    lvUpButton.GetComponentInChildren<Button>().interactable = false;
                }
                break;

            case TrainingIndex.LegendItemRate:
                title.text = "Legend Item Rate";
                level.text = "Lv. " + GameManager.Instance.PersistentGameplayData.LegendItemRateLv;
                indexMul.text = GameManager.Instance.TrainingIndex.ListTrainingLevelInfo[GameManager.Instance.PersistentGameplayData.LegendItemRateLv].ListIndexMul.LegendItemRate + "x";
                if (GameManager.Instance.PersistentGameplayData.CurrentCrystal < GameManager.Instance.TrainingIndex.ListTrainingLevelInfo[GameManager.Instance.PersistentGameplayData.LegendItemRateLv + 1].RequireCrystal
                    || GameManager.Instance.PersistentGameplayData.LegendItemRateLv >= 21)
                {
                    lvUpButton.GetComponentInChildren<Button>().interactable = false;
                }
                break;

            case TrainingIndex.MoneyBonus:
                title.text = "Money Bonus";
                level.text = "Lv. " + GameManager.Instance.PersistentGameplayData.MoneyBonusLv;
                indexMul.text = GameManager.Instance.TrainingIndex.ListTrainingLevelInfo[GameManager.Instance.PersistentGameplayData.MoneyBonusLv].ListIndexMul.MoneyBonus + "x";
                if (GameManager.Instance.PersistentGameplayData.CurrentCrystal < GameManager.Instance.TrainingIndex.ListTrainingLevelInfo[GameManager.Instance.PersistentGameplayData.MoneyBonusLv + 1].RequireCrystal
                    || GameManager.Instance.PersistentGameplayData.MoneyBonusLv >= 21)
                {
                    lvUpButton.GetComponentInChildren<Button>().interactable = false;
                }
                break;

            case TrainingIndex.DesireBonus:
                title.text = "DesireBonus";
                level.text = "Lv. " + GameManager.Instance.PersistentGameplayData.DesireBonusLv;
                indexMul.text = GameManager.Instance.TrainingIndex.ListTrainingLevelInfo[GameManager.Instance.PersistentGameplayData.DesireBonusLv].ListIndexMul.DesireBonus + "x";
                if (GameManager.Instance.PersistentGameplayData.CurrentCrystal < GameManager.Instance.TrainingIndex.ListTrainingLevelInfo[GameManager.Instance.PersistentGameplayData.DesireBonusLv + 1].RequireCrystal
                    || GameManager.Instance.PersistentGameplayData.DesireBonusLv >= 21)
                {
                    lvUpButton.GetComponentInChildren<Button>().interactable = false;
                }
                break;

            case TrainingIndex.PlayerHP:
                title.text = "Player's HP";
                level.text = "Lv. " + GameManager.Instance.PersistentGameplayData.PlayerHPLv;
                indexMul.text = GameManager.Instance.TrainingIndex.ListTrainingLevelInfo[GameManager.Instance.PersistentGameplayData.PlayerHPLv].ListIndexMul.PlayerHP + "x";
                if (GameManager.Instance.PersistentGameplayData.CurrentCrystal < GameManager.Instance.TrainingIndex.ListTrainingLevelInfo[GameManager.Instance.PersistentGameplayData.PlayerHPLv + 1].RequireCrystal
                    || GameManager.Instance.PersistentGameplayData.PlayerHPLv >= 21)
                {
                    lvUpButton.GetComponentInChildren<Button>().interactable = false;
                }
                break;

            case TrainingIndex.CrystalRate:
                title.text = "Crystal Rate";
                level.text = "Lv. " + GameManager.Instance.PersistentGameplayData.CrystalRateLv;
                indexMul.text = GameManager.Instance.TrainingIndex.ListTrainingLevelInfo[GameManager.Instance.PersistentGameplayData.CrystalRateLv].ListIndexMul.CrystalRate + "x";
                if (GameManager.Instance.PersistentGameplayData.CurrentCrystal < GameManager.Instance.TrainingIndex.ListTrainingLevelInfo[GameManager.Instance.PersistentGameplayData.CrystalRateLv + 1].RequireCrystal
                    || GameManager.Instance.PersistentGameplayData.CrystalRateLv >= 21)
                {
                    lvUpButton.GetComponentInChildren<Button>().interactable = false;
                }
                break;
        }
    }

    public void LeveUpIndex()
    {
        switch (trainingIndex)
        {
            case TrainingIndex.AttackBonus:
                GameManager.Instance.PersistentGameplayData.CurrentCrystal -= GameManager.Instance.TrainingIndex.ListTrainingLevelInfo[GameManager.Instance.PersistentGameplayData.AttackBonusLv + 1].RequireCrystal; GameManager.Instance.PersistentGameplayData.AttackBonusLv++;
                break;

            case TrainingIndex.RecoveryBonus:
                GameManager.Instance.PersistentGameplayData.CurrentCrystal -= GameManager.Instance.TrainingIndex.ListTrainingLevelInfo[GameManager.Instance.PersistentGameplayData.RecoveryBonusLv + 1].RequireCrystal; GameManager.Instance.PersistentGameplayData.RecoveryBonusLv++;
                break;

            case TrainingIndex.ClosingRate:
                GameManager.Instance.PersistentGameplayData.CurrentCrystal -= GameManager.Instance.TrainingIndex.ListTrainingLevelInfo[GameManager.Instance.PersistentGameplayData.ClosingRateLv + 1].RequireCrystal;
                GameManager.Instance.PersistentGameplayData.ClosingRateLv++;
                break;

            case TrainingIndex.LegendItemRate:
                GameManager.Instance.PersistentGameplayData.CurrentCrystal -= GameManager.Instance.TrainingIndex.ListTrainingLevelInfo[GameManager.Instance.PersistentGameplayData.LegendItemRateLv + 1].RequireCrystal;
                GameManager.Instance.PersistentGameplayData.LegendItemRateLv++;
                break;

            case TrainingIndex.MoneyBonus:
                GameManager.Instance.PersistentGameplayData.CurrentCrystal -= GameManager.Instance.TrainingIndex.ListTrainingLevelInfo[GameManager.Instance.PersistentGameplayData.MoneyBonusLv + 1].RequireCrystal;
                GameManager.Instance.PersistentGameplayData.MoneyBonusLv++;
                break;

            case TrainingIndex.DesireBonus:
                GameManager.Instance.PersistentGameplayData.CurrentCrystal -= GameManager.Instance.TrainingIndex.ListTrainingLevelInfo[GameManager.Instance.PersistentGameplayData.DesireBonusLv + 1].RequireCrystal;
                GameManager.Instance.PersistentGameplayData.DesireBonusLv++;
                break;

            case TrainingIndex.PlayerHP:
                GameManager.Instance.PersistentGameplayData.CurrentCrystal -= GameManager.Instance.TrainingIndex.ListTrainingLevelInfo[GameManager.Instance.PersistentGameplayData.PlayerHPLv + 1].RequireCrystal;
                GameManager.Instance.PersistentGameplayData.PlayerHPLv++;
                break;

            case TrainingIndex.CrystalRate:
                GameManager.Instance.PersistentGameplayData.CurrentCrystal -= GameManager.Instance.TrainingIndex.ListTrainingLevelInfo[GameManager.Instance.PersistentGameplayData.CrystalRateLv + 1].RequireCrystal;
                GameManager.Instance.PersistentGameplayData.CrystalRateLv++;
                break;
        }
        GameManager.Instance.SaveSystem.SaveTrainingDataToJson();
        confirmPanel.SetActive(false);
    }

    public void ConfirmUpdate()
    {
        confirmPanel.SetActive(true);

        switch (trainingIndex)
        {
            case TrainingIndex.AttackBonus:
                indexCrystal = GameManager.Instance.TrainingIndex.ListTrainingLevelInfo[GameManager.Instance.PersistentGameplayData.AttackBonusLv + 1].RequireCrystal;
                indexLv = GameManager.Instance.PersistentGameplayData.AttackBonusLv + 1;
                break;

            case TrainingIndex.RecoveryBonus:
                indexCrystal = GameManager.Instance.TrainingIndex.ListTrainingLevelInfo[GameManager.Instance.PersistentGameplayData.RecoveryBonusLv + 1].RequireCrystal;
                indexLv = GameManager.Instance.PersistentGameplayData.RecoveryBonusLv + 1;
                break;

            case TrainingIndex.ClosingRate:
                indexCrystal = GameManager.Instance.TrainingIndex.ListTrainingLevelInfo[GameManager.Instance.PersistentGameplayData.ClosingRateLv + 1].RequireCrystal;
                indexLv = GameManager.Instance.PersistentGameplayData.ClosingRateLv + 1;
                break;

            case TrainingIndex.LegendItemRate:
                indexCrystal = GameManager.Instance.TrainingIndex.ListTrainingLevelInfo[GameManager.Instance.PersistentGameplayData.LegendItemRateLv + 1].RequireCrystal;
                indexLv = GameManager.Instance.PersistentGameplayData.LegendItemRateLv + 1;
                break;

            case TrainingIndex.MoneyBonus:
                indexCrystal = GameManager.Instance.TrainingIndex.ListTrainingLevelInfo[GameManager.Instance.PersistentGameplayData.MoneyBonusLv + 1].RequireCrystal;
                indexLv = GameManager.Instance.PersistentGameplayData.MoneyBonusLv + 1;
                break;

            case TrainingIndex.DesireBonus:
                indexCrystal = GameManager.Instance.TrainingIndex.ListTrainingLevelInfo[GameManager.Instance.PersistentGameplayData.DesireBonusLv + 1].RequireCrystal;
                indexLv = GameManager.Instance.PersistentGameplayData.DesireBonusLv + 1;
                break;

            case TrainingIndex.PlayerHP:
                indexCrystal = GameManager.Instance.TrainingIndex.ListTrainingLevelInfo[GameManager.Instance.PersistentGameplayData.PlayerHPLv + 1].RequireCrystal;
                indexLv = GameManager.Instance.PersistentGameplayData.PlayerHPLv + 1;
                break;

            case TrainingIndex.CrystalRate:
                indexCrystal = GameManager.Instance.TrainingIndex.ListTrainingLevelInfo[GameManager.Instance.PersistentGameplayData.CrystalRateLv + 1].RequireCrystal;
                indexLv = GameManager.Instance.PersistentGameplayData.CrystalRateLv + 1;
                break;
        }

        var messageColor = "#38c42b";

        confirmMessage.text = $"Upgrade <color={messageColor}>{trainingIndex}</color> to Lv.<color={messageColor}>{indexLv}</color> need cost <color={messageColor}>{indexCrystal }</color> crystal(s)";

        upgradeButton.onClick.RemoveAllListeners();

        upgradeButton.onClick.AddListener(LeveUpIndex);
    }
}
