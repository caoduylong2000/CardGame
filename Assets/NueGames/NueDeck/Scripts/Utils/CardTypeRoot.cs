using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Utils
{
    public class CardTypeRoot : MonoBehaviour
    {
        [SerializeField] private CardType cardType;

        public CardType CardType => cardType;
    }
}