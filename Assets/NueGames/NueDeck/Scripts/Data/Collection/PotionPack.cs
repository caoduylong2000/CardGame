using System.Collections.Generic;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Collection
{
    [CreateAssetMenu(fileName = "Potion Pack", menuName = "NueDeck/Collection/Pack", order = 0)]
    public class PotionPack : ScriptableObject
    {
        [SerializeField] private string packId;
        [SerializeField] private string packName;
        [SerializeField] private List<PotionData> potionList;

        public string PackId => packId;
        public string PackName => packName;
        public List<PotionData> PotionList => potionList;

        
    }
}