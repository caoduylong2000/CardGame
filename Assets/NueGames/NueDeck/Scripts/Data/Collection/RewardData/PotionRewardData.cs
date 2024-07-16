using System.Collections.Generic;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Collection.RewardData
{
    [CreateAssetMenu(fileName = "Potion Reward Data",menuName = "NueDeck/Collection/Rewards/PotionRW",order = 0)]
    public class PotionRewardData : RewardDataBase
    {
        [SerializeField] private List<PotionData> rewardPotionList;
        public List<PotionData> RewardPotionList => rewardPotionList;
    }
}