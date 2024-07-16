using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NueGames.NueDeck.Scripts.Data.Collection
{
    [CreateAssetMenu(fileName = "PotionPool", menuName = "NueDeck/Collection/PotionPool", order = 0)]
    public class PotionPool : ScriptableObject
    {
        [SerializeField] private List<PotionData> potionPoolObject;
        public List<PotionData> PotionPoolObject => potionPoolObject;
    }
}

