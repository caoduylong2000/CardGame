using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
namespace NueGames.NueDeck.Scripts.Data.Collection
{
    [CreateAssetMenu(fileName = "EventData", menuName = "NueDeck/Collection/Events", order = 7)]
    public class EventAssets : ScriptableObject
    {
        public TextAsset eventScript;
        public LocalizedStringTable eventLocalized;
    }
}

