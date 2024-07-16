using System;
using System.Collections.Generic;
using System.Text;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
using NueGames.NueDeck.Scripts.NueExtentions;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Collection
{
    [CreateAssetMenu(fileName = "Potion Data",menuName = "NueDeck/Collection/Potion",order = 0)]
    public class PotionData : ScriptableObject
    {
        [Header("Potion Profile")] 
        [SerializeField] private string id;
        [SerializeField] private string potionName;
        [SerializeField] private Sprite potionSprite;
        [SerializeField] private RarityType rarity;
        //[SerializeField] private PotionType potionType;

        [Header("Action Settings")]
        [SerializeField] private bool usableWithoutTarget;
        [SerializeField] private bool exhaustAfterPlay;
        [SerializeField] private bool haveCondition;
        [SerializeField] private List<PotionEffectData> potionEffectDataList;
        
        [Header("Description")]
        [SerializeField] private List<PotionDescriptionData> potionDescriptionDataList;
        [SerializeField] private List<SpecialKeywords> specialKeywordsList;
        
        [Header("Fx")]
        [SerializeField] private AudioActionType audioType;

        #region Cache
        public string Id => id;
        public bool UsableWithoutTarget => usableWithoutTarget;
        //public int ManaCost => manaCost;
        public string PotionName => potionName;
        public Sprite PotionSprite => potionSprite;
        public List<PotionEffectData> PotionEffectDataList => potionEffectDataList;
        public List<PotionDescriptionData> PotionDescriptionDataList => potionDescriptionDataList;
        public List<SpecialKeywords> KeywordsList => specialKeywordsList;
        public AudioActionType AudioType => audioType;
        public string MyDescription { get; set; }
        public RarityType Rarity => rarity;
        //public PotionType PotionType => PotionType;

        public bool ExhaustAfterPlay => exhaustAfterPlay;
        public bool HaveCondition => haveCondition;

        #endregion
        
        #region Methods
        public void UpdateDescription()
        {
            var str = new StringBuilder();

            foreach (var descriptionData in PotionDescriptionDataList)
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
        public void EditPotionName(string newName) => potionName = newName;
        public void EditId(string newId) => id = newId;
        //public void EditManaCost(int newCost) => manaCost = newCost;
        public void EditRarity(RarityType targetRarity) => rarity = targetRarity;
        public void EditPotionSprite(Sprite newSprite) => potionSprite = newSprite;
        public void EditUsableWithoutTarget(bool newStatus) => usableWithoutTarget = newStatus;
        public void EditExhaustAfterPlay(bool newStatus) => exhaustAfterPlay = newStatus;
        public void EditPotionEffectDataList(List<PotionEffectData> newPotionEffectDataList) =>
            potionEffectDataList = newPotionEffectDataList;
        public void EditPotionDescriptionDataList(List<PotionDescriptionData> newPotionDescriptionDataList) =>
            potionDescriptionDataList = newPotionDescriptionDataList;
        public void EditSpecialKeywordsList(List<SpecialKeywords> newSpecialKeywordsList) =>
            specialKeywordsList = newSpecialKeywordsList;
        public void EditAudioType(AudioActionType newAudioActionType) => audioType = newAudioActionType;
#endif

        #endregion

    }

    [Serializable]
    public class PotionEffectData
    {
        [SerializeField] private PotionEffectType potionEffectType;
        [SerializeField] private ActionTargetType actionTargetType;
        [SerializeField] private float actionValue;
        [SerializeField] private float actionDelay;

        public ActionTargetType ActionTargetType => actionTargetType;
        public PotionEffectType PotionEffectType => potionEffectType;
        public float ActionValue => actionValue;
        public float ActionDelay => actionDelay;

        #region Editor

#if UNITY_EDITOR
        public void EditActionType(PotionEffectType newType) =>  potionEffectType = newType;
        public void EditActionTarget(ActionTargetType newTargetType) => actionTargetType = newTargetType;
        public void EditActionValue(float newValue) => actionValue = newValue;
        public void EditActionDelay(float newValue) => actionDelay = newValue;

#endif


        #endregion
    }

    [Serializable]
    public class PotionDescriptionData
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

        public string GetModifiedValue(PotionData PotionData)
        {
            if (PotionData.PotionEffectDataList.Count <= 0) return "";
            
            if (ModifiedActionValueIndex>=PotionData.PotionEffectDataList.Count)
                modifiedActionValueIndex = PotionData.PotionEffectDataList.Count - 1;

            if (ModifiedActionValueIndex<0)
                modifiedActionValueIndex = 0;
            
            var str = new StringBuilder();
            var value = PotionData.PotionEffectDataList[ModifiedActionValueIndex].ActionValue;
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

        public string GetModifiedValueEditor(PotionData PotionData)
        {
            if (PotionData.PotionEffectDataList.Count <= 0) return "";
            
            if (ModifiedActionValueIndex>=PotionData.PotionEffectDataList.Count)
                modifiedActionValueIndex = PotionData.PotionEffectDataList.Count - 1;

            if (ModifiedActionValueIndex<0)
                modifiedActionValueIndex = 0;
            
            var str = new StringBuilder();
            var value = PotionData.PotionEffectDataList[ModifiedActionValueIndex].ActionValue;
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