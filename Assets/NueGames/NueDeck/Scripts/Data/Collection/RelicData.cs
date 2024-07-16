using System;
using System.Collections.Generic;
using System.Text;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
using NueGames.NueDeck.Scripts.NueExtentions;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Collection
{
    [CreateAssetMenu(fileName = "Relic Data",menuName = "NueDeck/Collection/Relic",order = 0)]
    public class RelicData : ScriptableObject
    {
        [Header("Relic Profile")] 
        [SerializeField] private string id;
        [SerializeField] private string relicName;
        [SerializeField] private Sprite relicSprite;
        [SerializeField] private RarityType rarity;
        //[SerializeField] private RelicType RelicType;

        [Header("Action Settings")]
        [SerializeField] private bool usableWithoutTarget;
        [SerializeField] private bool exhaustAfterPlay;
        [SerializeField] private bool haveCondition;
        [SerializeField] private List<RelicEffectData> relicEffectDataList;
        
        [Header("Description")]
        [SerializeField] private List<RelicDescriptionData> relicDescriptionDataList;
        [SerializeField] private List<SpecialKeywords> specialKeywordsList;
        
        [Header("Fx")]
        [SerializeField] private AudioActionType audioType;

        #region Cache
        public string Id => id;
        public bool UsableWithoutTarget => usableWithoutTarget;
        //public int ManaCost => manaCost;
        public string RelicName => relicName;
        public Sprite RelicSprite => relicSprite;
        public List<RelicEffectData> RelicEffectDataList => relicEffectDataList;
        public List<RelicDescriptionData> RelicDescriptionDataList => relicDescriptionDataList;
        public List<SpecialKeywords> KeywordsList => specialKeywordsList;
        public AudioActionType AudioType => audioType;
        public string MyDescription { get; set; }
        public RarityType Rarity => rarity;
        //public RelicType RelicType => RelicType;

        public bool ExhaustAfterPlay => exhaustAfterPlay;
        public bool HaveCondition => haveCondition;

        #endregion
        
        #region Methods
        public void UpdateDescription()
        {
            var str = new StringBuilder();

            foreach (var descriptionData in RelicDescriptionDataList)
            {
                str.Append(descriptionData.UseModifier
                    ? descriptionData.GetModifiedValue(this)
                    : descriptionData.GetDescription());
            }
            
            MyDescription = str.ToString();
        }
        #endregion

        #region Editor Methods
#if UNITY_EDITOR
        public void EditRelicName(string newName) => relicName = newName;
        public void EditId(string newId) => id = newId;
        //public void EditManaCost(int newCost) => manaCost = newCost;
        public void EditRarity(RarityType targetRarity) => rarity = targetRarity;
        public void EditRelicSprite(Sprite newSprite) => relicSprite = newSprite;
        public void EditUsableWithoutTarget(bool newStatus) => usableWithoutTarget = newStatus;
        public void EditExhaustAfterPlay(bool newStatus) => exhaustAfterPlay = newStatus;
        public void EditRelicEffectDataList(List<RelicEffectData> newRelicEffectDataList) =>
            relicEffectDataList = newRelicEffectDataList;
        public void EditRelicDescriptionDataList(List<RelicDescriptionData> newRelicDescriptionDataList) =>
            relicDescriptionDataList = newRelicDescriptionDataList;
        public void EditSpecialKeywordsList(List<SpecialKeywords> newSpecialKeywordsList) =>
            specialKeywordsList = newSpecialKeywordsList;
        public void EditAudioType(AudioActionType newAudioActionType) => audioType = newAudioActionType;
#endif

        #endregion

    }

    [Serializable]
    public class RelicEffectData
    {
        [SerializeField] private RelicEffectType relicEffectType;
        [SerializeField] private ActionTargetType actionTargetType;
        [SerializeField] private float actionValue;
        [SerializeField] private float actionDelay;

        public ActionTargetType ActionTargetType => actionTargetType;
        public RelicEffectType RelicEffectType => relicEffectType;
        public float ActionValue => actionValue;
        public float ActionDelay => actionDelay;

        #region Editor

#if UNITY_EDITOR
        public void EditActionType(RelicEffectType newType) =>  relicEffectType = newType;
        public void EditActionTarget(ActionTargetType newTargetType) => actionTargetType = newTargetType;
        public void EditActionValue(float newValue) => actionValue = newValue;
        public void EditActionDelay(float newValue) => actionDelay = newValue;

#endif


        #endregion
    }

    [Serializable]
    public class RelicDescriptionData
    {
        [Header("Text")]
        [SerializeField] private string descriptionText;
        [SerializeField] private bool enableOverrideColor;
        [SerializeField] private Color overrideColor = Color.black;
       
        [Header("Modifer")]
        [SerializeField] private bool useModifier;
        [SerializeField] private int modifiedActionValueIndex;
        [SerializeField] private StatusType modiferStats;
        [SerializeField] private bool usePrefixOnModifiedValue;
        [SerializeField] private string modifiedValuePrefix = "*";
        [SerializeField] private bool overrideColorOnValueScaled;

        public string DescriptionText => descriptionText;
        public bool EnableOverrideColor => enableOverrideColor;
        public Color OverrideColor => overrideColor;
        public bool UseModifier => useModifier;
        public int ModifiedActionValueIndex => modifiedActionValueIndex;
        public StatusType ModiferStats => modiferStats;
        public bool UsePrefixOnModifiedValue => usePrefixOnModifiedValue;
        public string ModifiedValuePrefix => modifiedValuePrefix;
        public bool OverrideColorOnValueScaled => overrideColorOnValueScaled;
        
        private CombatManager CombatManager => CombatManager.Instance;

        public string GetDescription()
        {
            var str = new StringBuilder();
            
            str.Append(DescriptionText);
            
            if (EnableOverrideColor && !string.IsNullOrEmpty(str.ToString())) 
                str.Replace(str.ToString(),ColorExtentions.ColorString(str.ToString(),OverrideColor));
            
            return str.ToString();
        }

        public string GetModifiedValue(RelicData RelicData)
        {
            if (RelicData.RelicEffectDataList.Count <= 0) return "";
            
            if (ModifiedActionValueIndex>=RelicData.RelicEffectDataList.Count)
                modifiedActionValueIndex = RelicData.RelicEffectDataList.Count - 1;

            if (ModifiedActionValueIndex<0)
                modifiedActionValueIndex = 0;
            
            var str = new StringBuilder();
            var value = RelicData.RelicEffectDataList[ModifiedActionValueIndex].ActionValue;
            var modifer = 0;
            if (CombatManager)
            {
                var player = CombatManager.CurrentMainAlly;
               
                if (player)
                {
                    modifer = player.CharacterStats.StatusDict[ModiferStats].StatusValue;
                    value += modifer;

                    if (modifer != 0)
                    {
                        if (usePrefixOnModifiedValue)
                            str.Append(modifiedValuePrefix);
                    }
                }
            }
           
            str.Append(value);

            if (EnableOverrideColor)
            {
                if (OverrideColorOnValueScaled)
                {
                    if (modifer != 0)
                        str.Replace(str.ToString(),ColorExtentions.ColorString(str.ToString(),OverrideColor));
                }
                else
                {
                    str.Replace(str.ToString(),ColorExtentions.ColorString(str.ToString(),OverrideColor));
                }
               
            }
            
            return str.ToString();
        }

        #region Editor
#if UNITY_EDITOR
        
        public string GetDescriptionEditor()
        {
            var str = new StringBuilder();
            
            str.Append(DescriptionText);
            
            return str.ToString();
        }

        public string GetModifiedValueEditor(RelicData RelicData)
        {
            if (RelicData.RelicEffectDataList.Count <= 0) return "";
            
            if (ModifiedActionValueIndex>=RelicData.RelicEffectDataList.Count)
                modifiedActionValueIndex = RelicData.RelicEffectDataList.Count - 1;

            if (ModifiedActionValueIndex<0)
                modifiedActionValueIndex = 0;
            
            var str = new StringBuilder();
            var value = RelicData.RelicEffectDataList[ModifiedActionValueIndex].ActionValue;
            if (CombatManager)
            {
                var player = CombatManager.CurrentMainAlly;
                if (player)
                {
                    var modifer =player.CharacterStats.StatusDict[ModiferStats].StatusValue;
                    value += modifer;
                
                    if (modifer!= 0)
                        str.Append("*");
                }
            }
           
            str.Append(value);
          
            return str.ToString();
        }
        
        public void EditDescriptionText(string newText) => descriptionText = newText;
        public void EditEnableOverrideColor(bool newStatus) => enableOverrideColor = newStatus;
        public void EditOverrideColor(Color newColor) => overrideColor = newColor;
        public void EditUseModifier(bool newStatus) => useModifier = newStatus;
        public void EditModifiedActionValueIndex(int newIndex) => modifiedActionValueIndex = newIndex;
        public void EditModiferStats(StatusType newStatusType) => modiferStats = newStatusType;
        public void EditUsePrefixOnModifiedValues(bool newStatus) => usePrefixOnModifiedValue = newStatus;
        public void EditPrefixOnModifiedValues(string newText) => modifiedValuePrefix = newText;
        public void EditOverrideColorOnValueScaled(bool newStatus) => overrideColorOnValueScaled = newStatus;

#endif
        #endregion
    }
}