
using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Data.Collection.RewardData;
using NueGames.NueDeck.Scripts.NueExtentions;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NueGames.NueDeck.Scripts.Data.Containers
{
    [CreateAssetMenu(fileName = "Reward Container", menuName = "NueDeck/Containers/Reward", order = 4)]
    public class RewardContainerData : ScriptableObject
    {
        [SerializeField] private List<CardRewardData> cardRewardDataList;
        [SerializeField] private List<PotionRewardData> potionRewardDataList;
        [SerializeField] private List<GoldRewardData> goldRewardDataList;
        [SerializeField] private List<SellRewardData> sellRewardDataList;
        [SerializeField] private List<CrystalRewardData> crystalRewardDataList;
        public List<CardRewardData> CardRewardDataList => cardRewardDataList;
        public List<PotionRewardData> PotionRewardDataList => potionRewardDataList;
        public List<GoldRewardData> GoldRewardDataList => goldRewardDataList;
        public List<SellRewardData> SellRewardDataList => sellRewardDataList;
        public List<CrystalRewardData> CrystalRewardDataList => crystalRewardDataList;


        public List<CardData> GetRandomCardRewardList(out CardRewardData rewardData)
        {
            rewardData = CardRewardDataList.RandomItem();
            
            List<CardData> cardList = new List<CardData>();
            
            foreach (var cardData in rewardData.RewardCardList)
                cardList.Add(cardData);

            return cardList;
        }

        public List<PotionData> GetRandomPotionRewardList(out PotionRewardData rewardData)
        {
            rewardData = PotionRewardDataList.RandomItem();

            List<PotionData> potionList = new List<PotionData>();

            foreach (var potionData in rewardData.RewardPotionList)
                potionList.Add(potionData);

            return potionList;
        }

        public int GetRandomGoldReward(out GoldRewardData rewardData)
        {
            //Random Gold Reward
            rewardData = GoldRewardDataList.RandomItem();
            var value = Random.Range(rewardData.MinGold, rewardData.MaxGold);

            return value;
        }

        public int GetSellGoldReward(out SellRewardData rewardData)
        {
            //Gold Sell item Reward
            rewardData = SellRewardDataList[0];
            var value = Random.Range(0, 50);

            return value;
        }

        public int GetCrystalDropReward(out CrystalRewardData rewardData)
        {
            //Crystal Reward
            rewardData = CrystalRewardDataList[0];
            var value = 1;

            return value;
        }

        public int GetBossCrystalDropReward(out CrystalRewardData rewardData)
        {
            //Crystal Reward
            rewardData = CrystalRewardDataList[1];
            var value = GameManager.Instance.PersistentGameplayData.CurrentStageId + 1;

            return value;
        }
    }
}