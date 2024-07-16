using System;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NueGames.NueDeck.Scripts.Utils
{
    public class InventoryHelper : MonoBehaviour
    {
        [SerializeField] private InventoryTypes inventoryType;
        
        private UIManager UIManager => UIManager.Instance;
        
        public void OpenInventory()
        {
            switch (inventoryType)
            {
                case InventoryTypes.CurrentDeck:
                    UIManager.OpenCardInventory(GameManager.Instance.PersistentGameplayData.CurrentCardsList,"Current Cards");
                    GameManager.Instance.IsInventory = false;
                    break;
                case InventoryTypes.DrawPile:
                    if(SceneManager.GetActiveScene().buildIndex == GameManager.Instance.SceneData.tutorialSceneIndex)
                    {
                        UIManager.OpenCardInventory(TutorialCollectionManager.Instance.DrawPile, "Draw Pile");
                    }
                    else
                    {
                        UIManager.OpenCardInventory(CollectionManager.Instance.DrawPile, "Draw Pile");
                    }

                    GameManager.Instance.IsInventory = false;
                    break;
                case InventoryTypes.DiscardPile:
                    if (SceneManager.GetActiveScene().buildIndex == GameManager.Instance.SceneData.tutorialSceneIndex)
                    {
                        UIManager.OpenCardInventory(TutorialCollectionManager.Instance.DiscardPile, "Discard Pile");
                    }
                    else
                    {
                        UIManager.OpenCardInventory(CollectionManager.Instance.DiscardPile, "Discard Pile");
                    }

                    GameManager.Instance.IsInventory = false;
                    break;
                case InventoryTypes.ExhaustPile:
                    if (SceneManager.GetActiveScene().buildIndex == GameManager.Instance.SceneData.tutorialSceneIndex)
                    {
                        UIManager.OpenCardInventory(TutorialCollectionManager.Instance.ExhaustPile, "Exhaust Pile");
                    }
                    else
                    {
                        UIManager.OpenCardInventory(CollectionManager.Instance.ExhaustPile, "Exhaust Pile");
                    }
                    
                    GameManager.Instance.IsInventory = false;
                    break;
                case InventoryTypes.PotionBag:
                    if (SceneManager.GetActiveScene().buildIndex == GameManager.Instance.SceneData.tutorialSceneIndex)
                    {
                        UIManager.OpenPotionInventory(TutorialCollectionManager.Instance.PotionBag, "Potion Bag");
                    }
                    else
                    {
                        UIManager.OpenPotionInventory(CollectionManager.Instance.PotionBag, "Potion Bag");
                    }
                    
                    GameManager.Instance.IsInventory = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
    }
}