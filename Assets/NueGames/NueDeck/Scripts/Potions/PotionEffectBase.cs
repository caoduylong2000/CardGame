using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;

namespace NueGames.NueDeck.Scripts.Potion
{
    public class PotionEffectParameters
    {
        public readonly float Value;
        public readonly CharacterBase TargetCharacter;
        public readonly CharacterBase SelfCharacter;
        public readonly PotionData PotionData;
        public readonly PotionBase PotionBase;
        public PotionEffectParameters(float value,CharacterBase target, CharacterBase self,PotionData potionData, PotionBase potionBase)
        {
            Value = value;
            TargetCharacter = target;
            SelfCharacter = self;
            PotionData = potionData;
            PotionBase = potionBase;
        }
    }
    public abstract class PotionEffectBase
    {
        protected PotionEffectBase(){}
        public abstract PotionEffectType ActionType { get;}
        public abstract void DoAction(PotionEffectParameters actionParameters);
        
        protected FxManager FxManager => FxManager.Instance;
        protected AudioManager AudioManager => AudioManager.Instance;
        protected GameManager GameManager => GameManager.Instance;
        protected CombatManager CombatManager => CombatManager.Instance;
        protected CollectionManager CollectionManager => CollectionManager.Instance;
        protected TutorialCombatManager TutorialCombatManager => TutorialCombatManager.Instance;
        protected TutorialCollectionManager TutorialCollectionManager => TutorialCollectionManager.Instance;
    }
    
    
   
}