using System;
using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
using NueGames.NueDeck.Scripts.UI;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Utils
{
    public class MerchantInventoryHelper : MonoBehaviour
    {
        [SerializeField] private InventoryTypes inventoryType;
        [SerializeField] private InventoryCanvas inventoryCanvas;
        [SerializeField] private RectTransform spawnRoot;

        private UIManager UIManager => UIManager.Instance;
        
        public void OpenInventory()
        {
            RemoveObject();

            switch (inventoryType)
            {
                case InventoryTypes.CurrentDeck:
                    OpenCardInventory(GameManager.Instance.PersistentGameplayData.CurrentCardsList, "Deck");
                    GameManager.Instance.IsInventory = true;
                    break;

                case InventoryTypes.PotionBag:
                    OpenPotionInventory(GameManager.Instance.PersistentGameplayData.CurrentPotionsList, "Potion Bag");
                    GameManager.Instance.IsInventory = true;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void OpenCardInventory(List<CardData> cardList, string title)
        {
            inventoryCanvas.ChangeTitle(title); 

            inventoryCanvas.SetCards(cardList);
        }

        public void OpenPotionInventory(List<PotionData> potionList, string title)
        {
            inventoryCanvas.ChangeTitle(title);

            CollectionManager.Instance.SetPotionPack();

            inventoryCanvas.SetPotions(potionList);
        }

        public void RemoveObject()
        {
            if (spawnRoot.childCount > 0)
            {
                for (int i = 0; i < spawnRoot.childCount; i++)
                {
                    GameObject childObject = spawnRoot.GetChild(i).gameObject;

                    childObject.SetActive(false);
                }
            }
        }
    }
}