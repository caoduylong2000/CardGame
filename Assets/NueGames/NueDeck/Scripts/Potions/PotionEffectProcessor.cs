using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Potion
{ 
    public static class PotionEffectProcessor
    {
        private static readonly Dictionary<PotionEffectType, PotionEffectBase> PotionEffectDict =
            new Dictionary<PotionEffectType, PotionEffectBase>();

        public static bool IsInitialized { get; private set; }

        public static void Initialize()
        {
            PotionEffectDict.Clear();

            var allActionPotions = Assembly.GetAssembly(typeof(PotionEffectBase)).GetTypes()
                .Where(t => typeof(PotionEffectBase).IsAssignableFrom(t) && t.IsAbstract == false);

            foreach (var actionPotion in allActionPotions)
            {
                PotionEffectBase action = Activator.CreateInstance(actionPotion) as PotionEffectBase;
                if (action != null) PotionEffectDict.Add(action.ActionType, action);
            }

            IsInitialized = true;
        }

        public static PotionEffectBase GetAction(PotionEffectType targetAction) =>
            PotionEffectDict[targetAction];
    }
}