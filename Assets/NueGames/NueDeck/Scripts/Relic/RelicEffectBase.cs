using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;

namespace NueGames.NueDeck.Scripts.Relic
{
    public class RelicEffectParameters
    {
        public readonly float Value;
        public readonly CharacterBase TargetCharacter;
        public readonly CharacterBase SelfCharacter;
        public readonly RelicData RelicData;
        public readonly RelicBase RelicBase;
        public RelicEffectParameters(float value,CharacterBase target, CharacterBase self,RelicData relicData, RelicBase relicBase)
        {
            Value = value;
            TargetCharacter = target;
            SelfCharacter = self;
            RelicData = relicData;
            RelicBase = relicBase;
        }
    }
    public abstract class RelicEffectBase
    {
        protected RelicEffectBase(){}
        public abstract RelicEffectType ActionType { get;}
        public abstract void DoAction(RelicEffectParameters actionParameters);
        
        protected FxManager FxManager => FxManager.Instance;
        protected AudioManager AudioManager => AudioManager.Instance;
        protected GameManager GameManager => GameManager.Instance;
        protected CombatManager CombatManager => CombatManager.Instance;
        protected CollectionManager CollectionManager => CollectionManager.Instance;
        
    }
    
    
   
}