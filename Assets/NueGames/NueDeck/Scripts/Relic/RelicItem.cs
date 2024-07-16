using System.Collections;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace NueGames.NueDeck.Scripts.Relic
{
    public class RelicItem : RelicBase
    {

        [Header("3D Settings")]
        [SerializeField] private Canvas canvas;

        [Header("Merchant Stage")]
        [SerializeField] private int relicPrice;
        [SerializeField] private Text relicPriceText;
        [SerializeField] private GameObject priceObject;
        [SerializeField] private GameObject soldStamp;

        public bool IsPurchase = false;
        public int RelicPrice => relicPrice;
        public Text RelicPriceText => relicPriceText;
        public GameObject PriceObject => priceObject;
        public GameObject SoldStamp => soldStamp;

        public override void SetRelic(RelicData targetProfile, bool isPlayable = true)
        {
            base.SetRelic(targetProfile, isPlayable);

            if (canvas && CollectionManager != null)
                canvas.worldCamera = CollectionManager.HandController.cam;
            else
                canvas.worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

            if(RelicPriceText != null)
            {
                SetRelicPrice(targetProfile);
            }
        }
        
        public override void SetInactiveMaterialState(bool isInactive)
        {
            base.SetInactiveMaterialState(isInactive);
        }

        private void SetRelicPrice(RelicData targetProfile)
        {
            switch (targetProfile.Rarity)
            {
                case Enums.RarityType.Common:
                    relicPrice = Random.Range(10, 30);
                    this.RelicPriceText.text = RelicPrice.ToString();
                    break;
                case Enums.RarityType.Rare:
                    relicPrice = Random.Range(40, 70);
                    this.RelicPriceText.text = RelicPrice.ToString();
                    break;
                case Enums.RarityType.Legendary:
                    relicPrice = Random.Range(80, 120);
                    this.RelicPriceText.text = RelicPrice.ToString();
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