using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Relic
{ 
    public static class RelicEffectProcessor
    {
        private static readonly Dictionary<RelicEffectType, RelicEffectBase> RelicEffectDict =
            new Dictionary<RelicEffectType, RelicEffectBase>();

        public static bool IsInitialized { get; private set; }

        public static void Initialize()
        {
            RelicEffectDict.Clear();

            var allActionRelics = Assembly.GetAssembly(typeof(RelicEffectBase)).GetTypes()
                .Where(t => typeof(RelicEffectBase).IsAssignableFrom(t) && t.IsAbstract == false);

            foreach (var actionRelic in allActionRelics)
            {
                RelicEffectBase action = Activator.CreateInstance(actionRelic) as RelicEffectBase;
                if (action != null) RelicEffectDict.Add(action.ActionType, action);
            }

            IsInitialized = true;
        }

        public static RelicEffectBase GetAction(RelicEffectType targetAction) =>
            RelicEffectDict[targetAction];
    }
}