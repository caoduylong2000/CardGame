using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NueGames.NueDeck.Scripts.Data.Collection
{
    [CreateAssetMenu(fileName = "CardPool", menuName = "NueDeck/Collection/CardPool", order = 0)]
    public class CardPool : ScriptableObject
    {
        [SerializeField] private List<CardData> cardPoolObject;
        public List<CardData> CardPoolObject => cardPoolObject;
    }
}

