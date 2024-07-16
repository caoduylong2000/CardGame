using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Card;
using NueGames.NueDeck.Scripts.Potion;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using NueGames.NueDeck.Scripts.Characters;
using System.Collections;

namespace NueGames.NueDeck.Scripts.UI
{
    public class InventoryCanvas : CanvasBase
    {
        [SerializeField] private Text titleTextField;
        [SerializeField] private LayoutGroup cardSpawnRoot;
        [SerializeField] private CardBase cardUIPrefab;
        [SerializeField] private PotionBase potionUIPrefab;

        private delegate void CardButtonClickDelegate(CardUI clickedCard);
        private static event CardButtonClickDelegate OnCardButtonClick;

        private delegate void PotionButtonClickDelegate(PotionItem clickedPotion);
        private static event PotionButtonClickDelegate OnPotionButtonClick;

        public Text TitleTextField => titleTextField;
        public LayoutGroup CardSpawnRoot => cardSpawnRoot;

        private List<CardBase> _spawnedCardList = new List<CardBase>();

        private List<PotionBase> _spawnedPotionList = new List<PotionBase>();

        [Header("UseConfirm")]
        public GameObject useComfirm;
        [SerializeField] private Text itemName;
        [SerializeField] private Text itemDesc;
        [SerializeField] private Transform itemImage;
        [SerializeField] private Text priceText;
        [SerializeField] private Button useButton;
        [SerializeField] private Button sellButton;

        public void ChangeTitle(string newTitle) => TitleTextField.text = newTitle;
        
        public virtual void SetCards(List<CardData> cardDataList)
        {
            var count = 0;
            for (int i = 0; i < _spawnedCardList.Count; i++)
            {
                count++;
                if (i >= cardDataList.Count)
                {
                    _spawnedCardList[i].gameObject.SetActive(false);
                }
                else
                {
                    _spawnedCardList[i].SetCard(cardDataList[i], false);
                    _spawnedCardList[i].gameObject.SetActive(true);
                }

            }

            var cal = cardDataList.Count - count;
            if (cal > 0)
            {
                foreach (var existingCard in _spawnedCardList)
                {
                    Destroy(existingCard.gameObject);
                }
                _spawnedCardList.Clear();

                for (var i = 0; i < cardDataList.Count; i++)
                {
                    var cardData = cardDataList[i];
                    var cardBase = Instantiate(cardUIPrefab, CardSpawnRoot.transform);
                    cardBase.SetCard(cardData, true);

                    var button = cardBase.gameObject.GetComponent<Button>();
                    if (button != null)
                    {
                        button.onClick.RemoveAllListeners();
                        button.onClick.AddListener(() => OpenMessage(cardBase.gameObject));
                    }

                    _spawnedCardList.Add(cardBase);
                }
            }
        }

        public void SetPotions(List<PotionData> potionDataList)
        {
            var count = 0;
            for (int i = 0; i < _spawnedPotionList.Count; i++)
            {
                count++;
                if (i >= potionDataList.Count)
                {
                    _spawnedPotionList[i].gameObject.SetActive(false);
                }
                else
                {
                    _spawnedPotionList[i].SetPotion(potionDataList[i], true);
                    _spawnedPotionList[i].gameObject.SetActive(true);
                }
            }

            var cal = potionDataList.Count - count;

            if (cal > 0)
            {
                foreach (var existingPotion in _spawnedPotionList)
                {
                    Destroy(existingPotion.gameObject);
                }
                _spawnedPotionList.Clear();

                for (var i = 0; i < potionDataList.Count; i++)
                {
                    var potionData = potionDataList[i];
                    var potionBase = Instantiate(potionUIPrefab, CardSpawnRoot.transform);
                    potionBase.SetPotion(potionData, true);

                    var button = potionBase.gameObject.GetComponent<Button>();
                    if (button != null)
                    {
                        button.onClick.RemoveAllListeners();
                        button.onClick.AddListener(() => OpenMessage(potionBase.gameObject));
                    }
                    _spawnedPotionList.Add(potionBase);
                }
            }
        }

        public override void OpenCanvas()
        {
            base.OpenCanvas();
            if (CollectionManager)
                CollectionManager.HandController.DisableDragging();
        }

        public override void CloseCanvas()
        {
            base.CloseCanvas();
            if (CollectionManager)
                CollectionManager.HandController.EnableDragging();

            if (cardSpawnRoot.transform.childCount > 0)
            {

                for (int i = 0; i < cardSpawnRoot.transform.childCount; i++)
                {
                    GameObject childObject = cardSpawnRoot.transform.GetChild(i).gameObject;

                    childObject.SetActive(false);
                }
            }
        }

        public override void ResetCanvas()
        {
            base.ResetCanvas();
        }

        public void OpenMessage(GameObject childObject)
        {
            if (childObject.GetComponent<CardUI>())
            {
                useComfirm.SetActive(true);
                itemDesc.text = childObject.GetComponent<CardUI>().CardData.MyDescription;
                itemName.text = childObject.GetComponent<CardUI>().CardData.CardName;

                var clone = Instantiate(cardUIPrefab, itemImage);

                clone.GetComponent<RectTransform>().sizeDelta = new Vector2(400, 640);

                clone.SetCard(childObject.GetComponent<CardUI>().CardData);

                if (GameManager.IsInventory == true)
                {
                    switch (childObject.GetComponent<CardUI>().CardData.Rarity)
                    {
                        case Enums.RarityType.Common:
                            GameManager.salePrice = 10;
                            priceText.text = GameManager.salePrice.ToString();
                            break;
                        case Enums.RarityType.Rare:
                            GameManager.salePrice = 30;
                            priceText.text = GameManager.salePrice.ToString();
                            break;
                        case Enums.RarityType.Legendary:
                            GameManager.salePrice = 50;
                            priceText.text = GameManager.salePrice.ToString();
                            break;
                    }

                    useButton.gameObject.SetActive(false);
                    sellButton.gameObject.SetActive(true);
                    sellButton.onClick.RemoveAllListeners();
                    sellButton.onClick.AddListener(() => SelectItemsToSell(childObject));
                }
                else
                {
                    useButton.gameObject.SetActive(false);
                    useButton.onClick.RemoveAllListeners(); // Remove all previous listeners
                }

                OnCardButtonClick?.Invoke(childObject.GetComponent<CardUI>());
            }
            else if (childObject.GetComponent<PotionItem>())
            {
                useComfirm.SetActive(true);
                itemDesc.text = childObject.GetComponent<PotionItem>().PotionData.MyDescription;
                itemName.text = childObject.GetComponent<PotionItem>().PotionData.PotionName;

                var clone = Instantiate(potionUIPrefab, itemImage);



                clone.SetPotion(childObject.GetComponent<PotionItem>().PotionData);

                if (GameManager.IsInventory == true)
                {
                    switch (childObject.GetComponent<PotionItem>().PotionData.Rarity)
                    {
                        case Enums.RarityType.Common:
                            GameManager.salePrice = 10;
                            priceText.text = GameManager.salePrice.ToString();
                            break;
                        case Enums.RarityType.Rare:
                            GameManager.salePrice = 30;
                            priceText.text = GameManager.salePrice.ToString();
                            break;
                        case Enums.RarityType.Legendary:
                            GameManager.salePrice = 50;
                            priceText.text = GameManager.salePrice.ToString();
                            break;
                    }

                    useButton.gameObject.SetActive(false);
                    sellButton.gameObject.SetActive(true);
                    sellButton.onClick.RemoveAllListeners();
                    sellButton.onClick.AddListener(() => SelectItemsToSell(childObject));

                }
                else
                {
                    useButton.gameObject.SetActive(true);
                    useButton.onClick.RemoveAllListeners(); // Remove all previous listeners
                    useButton.onClick.AddListener(() => UsePotion(childObject));
                }
                OnPotionButtonClick?.Invoke(childObject.GetComponent<PotionItem>());
            }
        }

        public void CloseMessage()
        {
            if (itemImage.childCount > 0)
            {
                for (int i = 0; i < itemImage.childCount; i++)
                {
                    GameObject childObject = itemImage.GetChild(i).gameObject;
                    Destroy(childObject);
                }
            }

            useComfirm.SetActive(false);
        }

        private void UsePotion(GameObject childObject)
        {
            StartCoroutine(PotionUseRoutine(childObject));

            CloseMessage();
        }

        private IEnumerator PotionUseRoutine(GameObject childObject)
        {
            if (childObject.GetComponent<PotionItem>())
            {
                childObject.GetComponent<PotionItem>().Use(CombatManager.CurrentMainAlly, null, CombatManager.CurrentEnemiesList, CombatManager.CurrentAlliesList);
            }

            yield return new WaitForSeconds(0f);

            CloseCanvas();
        }


        public void SelectItemsToSell(GameObject selectedItems)
        {
            if (selectedItems.GetComponent<CardUI>())
            {
                GameManager.itemForSale = cardUIPrefab.gameObject;
                GameManager.cardData = selectedItems.GetComponent<CardUI>().CardData;
                GameManager.PersistentGameplayData.CurrentCardsList.Remove(selectedItems.GetComponent<CardUI>().CardData);
            }
            else if (selectedItems.GetComponent<PotionItem>())
            {
                GameManager.itemForSale = potionUIPrefab.gameObject;
                GameManager.potionData = selectedItems.GetComponent<PotionItem>().PotionData;
                GameManager.PersistentGameplayData.CurrentPotionsList.Remove(selectedItems.GetComponent<PotionItem>().PotionData);
            }

            CloseMessage();

            UIManager.CombatCanvas.SetSaleItemData();

            if (SceneManager.GetActiveScene().buildIndex == GameManager.SceneData.tutorialSceneIndex)
            {
                TutorialCombatManager.Instance.SetNewItemData();
            }
            else if (SceneManager.GetActiveScene().buildIndex == GameManager.SceneData.combatSceneIndex)
            {
                CombatManager.Instance.SetNewItemData();
            }

            

            Transform parentTransform = transform.parent;

            if (parentTransform != null)
            {
                parentTransform.gameObject.SetActive(false);
            }
        }
    }
}
