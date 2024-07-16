using System.Collections;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace NueGames.NueDeck.Scripts.Potion
{
    public class PotionItem : PotionBase
    {

        [Header("3D Settings")]
        [SerializeField] private Canvas canvas;

        [Header("Merchant Stage")]
        [SerializeField] private int potionPrice;
        [SerializeField] private Text potionPriceText;
        [SerializeField] private GameObject priceObject;
        [SerializeField] private GameObject soldStamp;

        public bool IsPurchase = false;
        public int PotionPrice => potionPrice;
        public Text PotionPriceText => potionPriceText;
        public GameObject PriceObject => priceObject;
        public GameObject SoldStamp => soldStamp;

        public override void SetPotion(PotionData targetProfile, bool isPlayable = true)
        {
            base.SetPotion(targetProfile, isPlayable);

            if (canvas && CollectionManager != null)
                canvas.worldCamera = CollectionManager.HandController.cam;
            else
                canvas.worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

            if(PotionPriceText != null)
            {
                SetPotionPrice(targetProfile);
            }
        }
        
        public override void SetInactiveMaterialState(bool isInactive)
        {
            base.SetInactiveMaterialState(isInactive);
        }

        private void SetPotionPrice(PotionData targetProfile)
        {
            switch (targetProfile.Rarity)
            {
                case Enums.RarityType.Common:
                    potionPrice = Random.Range(10, 30);
                    this.PotionPriceText.text = PotionPrice.ToString();
                    break;
                case Enums.RarityType.Rare:
                    potionPrice = Random.Range(40, 70);
                    this.PotionPriceText.text = PotionPrice.ToString();
                    break;
                case Enums.RarityType.Legendary:
                    potionPrice = Random.Range(80, 120);
                    this.PotionPriceText.text = PotionPrice.ToString();
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
    }
}