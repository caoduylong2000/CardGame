using System.Collections;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NueGames.NueDeck.Scripts.Card
{
    public class Card3D : CardBase
    {

        [Header("3D Settings")]
        [SerializeField] private Canvas canvas;

        [Header("Merchant Stage")]
        [SerializeField] private int cardPrice;
        [SerializeField] private Text cardPriceText;
        [SerializeField] private GameObject priceObject;
        [SerializeField] private GameObject soldStamp;

        public bool IsPurchase = false;
        public int CardPrice => cardPrice;
        public Text CardPriceText => cardPriceText;
        public GameObject PriceObject => priceObject;
        public GameObject SoldStamp => soldStamp;

        public override void SetCard(CardData targetProfile, bool isPlayable)
        {
            base.SetCard(targetProfile, isPlayable);

            if (SceneManager.GetActiveScene().buildIndex == GameManager.SceneData.tutorialSceneIndex)
            {
                if (canvas && TutorialCollectionManager != null)
                    canvas.worldCamera = TutorialCollectionManager.HandController.cam;
                else
                    canvas.worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
            }
            else if (SceneManager.GetActiveScene().buildIndex == GameManager.SceneData.combatSceneIndex)
            {
                if (canvas && CollectionManager != null)
                    canvas.worldCamera = CollectionManager.HandController.cam;
                else
                    canvas.worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
            }

            if (cardPriceText != null)
            {
                SetCardPrice(targetProfile);
            }
        }
        
        public override void SetInactiveMaterialState(bool isInactive)
        {
            base.SetInactiveMaterialState(isInactive);
        }

        public void SetCardPrice(CardData targetProfile)
        {
            switch (targetProfile.Rarity)
            {
                case Enums.RarityType.Common:
                    cardPrice = Random.Range(10, 30);
                    cardPriceText.text = CardPrice.ToString();
                    break;
                case Enums.RarityType.Rare:
                    cardPrice = Random.Range(40, 70);
                    cardPriceText.text = CardPrice.ToString();
                    break;
                case Enums.RarityType.Legendary:
                    cardPrice = Random.Range(80, 120);
                    cardPriceText.text = CardPrice.ToString();
                    break;
            }
        }

        public void CheckPurchase()
        {
            if (IsPurchase == true)
            {
                PriceObject.SetActive(false);
                SoldStamp.SetActive(true);
            }
        }

        public void SellItem(CardData targetProfile)
        {
            switch (targetProfile.Rarity)
            {
                case Enums.RarityType.Common:
                    cardPrice = 30;
                    cardPriceText.text = CardPrice.ToString();
                    break;
                case Enums.RarityType.Rare:
                    cardPrice = 70;
                    cardPriceText.text = CardPrice.ToString();
                    break;
                case Enums.RarityType.Legendary:
                    cardPrice = 120;
                    cardPriceText.text = CardPrice.ToString();
                    break;
            }
        }
    }
}