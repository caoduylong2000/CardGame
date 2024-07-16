using NueGames.NueDeck.Scripts.Card;
using NueGames.NueDeck.Scripts.Potion;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Utils;
using NueGames.NueDeck.Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace NueGames.NueDeck.Scripts.UI
{
    public class CombatCanvas : CanvasBase
    {
        [Header("Texts")]
        [SerializeField] private Text drawPileTextField;
        [SerializeField] private Text discardPileTextField;
        [SerializeField] private Text exhaustPileTextField;
        [SerializeField] private Text manaTextTextField;
        [SerializeField] private Slider successPercentage;
        [SerializeField] private TextMeshProUGUI successPercentageText;
        [SerializeField] private GameObject inventory;
        [SerializeField] private Transform saleItemPosition;

        [Header("Panels")]
        [SerializeField] private GameObject combatLosePanel;
        [SerializeField] private GameObject combatWinPanel;


        public Text DrawPileTextField => drawPileTextField;
        public Text DiscardPileTextField => discardPileTextField;
        public Text ManaTextTextField => manaTextTextField;
        public Text ExhaustPileTextField => exhaustPileTextField;
        public Slider SuccessPercentage => successPercentage;
        public TextMeshProUGUI SuccessPercentageText => successPercentageText;
        public GameObject Inventory => inventory;
        public Transform SaleItemPosition => saleItemPosition;
        public GameObject CombatLosePanel => combatLosePanel;
        public GameObject CombatWinPanel => combatWinPanel;

        #region Setup

        #endregion

        #region Public Methods
        public void SetPileTexts()
        {
            if (SceneManager.GetActiveScene().buildIndex == GameManager.SceneData.tutorialSceneIndex)
            {
                DrawPileTextField.text = $"{TutorialCollectionManager.DrawPile.Count}";
                DiscardPileTextField.text = $"{TutorialCollectionManager.DiscardPile.Count}";
                ExhaustPileTextField.text = $"{TutorialCollectionManager.ExhaustPile.Count}";
                ManaTextTextField.text = $"{GameManager.PersistentGameplayData.CurrentMana}/{GameManager.PersistentGameplayData.MaxMana}";
            }
            else if (SceneManager.GetActiveScene().buildIndex == GameManager.SceneData.combatSceneIndex)
            {
                DrawPileTextField.text = $"{CollectionManager.DrawPile.Count}";
                DiscardPileTextField.text = $"{CollectionManager.DiscardPile.Count}";
                ExhaustPileTextField.text = $"{CollectionManager.ExhaustPile.Count}";
                ManaTextTextField.text = $"{GameManager.PersistentGameplayData.CurrentMana}/{GameManager.PersistentGameplayData.MaxMana}";
            }
        }

        public override void ResetCanvas()
        {
            base.ResetCanvas();
        }

        public void EndTurn()
        {
            if (SceneManager.GetActiveScene().buildIndex == GameManager.SceneData.tutorialSceneIndex)
            {
                if (TutorialCombatManager.CurrentCombatStateType == CombatStateType.AllyTurn)
                    TutorialCombatManager.EndTurn();
            }
            else if (SceneManager.GetActiveScene().buildIndex == GameManager.SceneData.combatSceneIndex)
            {
                if (CombatManager.CurrentCombatStateType == CombatStateType.AllyTurn)
                    CombatManager.EndTurn();
            }

        }

        public void SkipCombat()
        {
            CombatManager.WinCombat();
        }

        public void SetSaleItemData()
        {
            if (saleItemPosition.childCount > 0)
            {
                for (int i = 1; i < saleItemPosition.childCount; i++)
                {
                    Destroy(saleItemPosition.GetChild(i));
                }
            }
            else
            {
                var saleItem = Instantiate(GameManager.itemForSale, saleItemPosition);

                saleItem.GetComponent<RectTransform>().sizeDelta = new Vector2(400, 640);
                saleItem.gameObject.SetActive(true);

                if (saleItem.GetComponent<CardUI>())
                {
                    saleItem.GetComponent<CardUI>().SetCard(GameManager.cardData, false);
                }
                else if (saleItem.GetComponent<PotionItem>())
                {
                    saleItem.GetComponent<PotionItem>().SetPotion(GameManager.potionData, false);
                }
            }
        }

        public void UpdateSuccessText(int currentHealth, int maxHealth) => successPercentageText.text = $"{((float)(maxHealth - currentHealth) / maxHealth) * 100:F2}%";

        public void UpdateSuccessSlider(int currentHealth, int maxHealth) => successPercentage.value = (float)(maxHealth - currentHealth) / maxHealth;
        #endregion
    }
}