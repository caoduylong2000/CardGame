﻿using System;
using System.Collections;
using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
using NueGames.NueDeck.Scripts.NueExtentions;
using NueGames.NueDeck.Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace NueGames.NueDeck.Scripts.Potion
{
    public class PotionBase : MonoBehaviour
    {
        [Header("Base References")]
        [SerializeField] protected Transform descriptionRoot;
        [SerializeField] protected Image potionImage;
        [SerializeField] protected Image passiveImage;
        [SerializeField] protected TextMeshProUGUI nameTextField;
        [SerializeField] protected TextMeshProUGUI descTextField;
        //[SerializeField] protected TextMeshProUGUI manaTextField;
        [SerializeField] protected List<RarityRoot> rarityRootList;
        

        #region Cache
        public PotionData PotionData { get; private set; }
        public bool IsInactive { get; protected set; }
        protected Transform CachedTransform { get; set; }
        protected WaitForEndOfFrame CachedWaitFrame { get; set; }
        public bool IsPlayable { get; protected set; } = true;

        public List<RarityRoot> RarityRootList => rarityRootList;
        protected FxManager FxManager => FxManager.Instance;
        protected AudioManager AudioManager => AudioManager.Instance;
        protected GameManager GameManager => GameManager.Instance;
        protected CombatManager CombatManager => CombatManager.Instance;
        protected CollectionManager CollectionManager => CollectionManager.Instance;
        
        public bool IsExhausted { get; private set; }

        #endregion
        
        #region Setup
        protected virtual void Awake()
        {
            CachedTransform = transform;
            CachedWaitFrame = new WaitForEndOfFrame();
        }

        public virtual void SetPotion(PotionData targetProfile,bool isPlayable = true)
        {
            PotionData = targetProfile;
            IsPlayable = isPlayable;

            PotionData.UpdateDescription();

            nameTextField.text = PotionData.PotionName;
            descTextField.text = PotionData.MyDescription;
            potionImage.sprite = PotionData.PotionSprite;

            foreach (var rarityRoot in RarityRootList)
            {
                rarityRoot.gameObject.SetActive(rarityRoot.Rarity == PotionData.Rarity);
            }   
        }
        
        #endregion
        
        #region Potion Methods
        public virtual void Use(CharacterBase self,CharacterBase targetCharacter, List<EnemyBase> allEnemies, List<AllyBase> allAllies)
        {   
            StartCoroutine(PotionUseRoutine(self, targetCharacter, allEnemies, allAllies));
        }

        private IEnumerator PotionUseRoutine(CharacterBase self,CharacterBase targetCharacter, List<EnemyBase> allEnemies, List<AllyBase> allAllies)
        {
            bool potionUsed = false;

            foreach (var playerAction in PotionData.PotionEffectDataList)
            {
                yield return new WaitForSeconds(playerAction.ActionDelay);
                var targetList = DetermineTargets(targetCharacter, allEnemies, allAllies, playerAction);

                foreach (var target in targetList)
                    PotionEffectProcessor.GetAction(playerAction.PotionEffectType)
                        .DoAction(new PotionEffectParameters(playerAction.ActionValue,
                            target,self,PotionData,this));

                potionUsed = true;
            }

            if(potionUsed == true)
            {
                GameManager.PersistentGameplayData.CurrentPotionsList.Remove(PotionData);
            }    
                
        }

        private static List<CharacterBase> DetermineTargets(CharacterBase targetCharacter, List<EnemyBase> allEnemies, List<AllyBase> allAllies,
            PotionEffectData playerAction)
        {
            List<CharacterBase> targetList = new List<CharacterBase>();
            switch (playerAction.ActionTargetType)
            {
                case ActionTargetType.Enemy:
                    targetList.Add(targetCharacter);
                    break;
                case ActionTargetType.Ally:
                    targetList.Add(targetCharacter);
                    break;
                //case ActionTargetType.AllEnemies:
                //    foreach (var enemyBase in allEnemies)
                //        targetList.Add(enemyBase);
                //    break;
                //case ActionTargetType.AllAllies:
                //    foreach (var allyBase in allAllies)
                //        targetList.Add(allyBase);
                //    break;
                //case ActionTargetType.RandomEnemy:
                //    if (allEnemies.Count>0)
                //        targetList.Add(allEnemies.RandomItem());
                    
                //    break;
                //case ActionTargetType.RandomAlly:
                //    if (allAllies.Count>0)
                //        targetList.Add(allAllies.RandomItem());
                //    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return targetList;
        }
        
        //public virtual void DisPotion()
        //{
        //    if (IsExhausted) return;
        //    if (!IsPlayable) return;
        //    //CollectionManager.OnPotionDisPotioned(this);
        //    StartCoroutine(DisPotionRoutine());
        //}
        
        //public virtual void Exhaust(bool destroy = true)
        //{
        //    if (IsExhausted) return;
        //    if (!IsPlayable) return;
        //    IsExhausted = true;
        //    //CollectionManager.OnPotionExhausted(this);
        //    StartCoroutine(ExhaustRoutine(destroy));
        //}

        //protected virtual void SpendMana(int value)
        //{
        //    if (!IsPlayable) return;
        //    GameManager.PersistentGameplayData.CurrentMana -= value;
        //}

        //public virtual void Closing(float value)
        //{
        //    if (!IsPlayable) return;
        //    var successRate = Random.Range(0f, 0.99f) + (value/100);

        //    if (successRate > 0f && successRate < UIManager.Instance.CombatCanvas.SuccessPercentage.value || successRate > UIManager.Instance.CombatCanvas.SuccessPercentage.value)
        //    {
        //        CombatManager.WinCombat();
        //        Debug.Log("Close Success");
        //    }
        //    else
        //    {
        //        GameManager.Instance.playerCurrentHealth -= (int)value;
        //        UIManager.Instance.InformationCanvas.UpdateHealStats(GameManager.Instance.playerCurrentHealth, GameManager.Instance.playerMaxHealth);

        //        Debug.Log("Close Fail");
        //    }
        //}

        public virtual void SetInactiveMaterialState(bool isInactive) 
        {
            if (!IsPlayable) return;
            if (isInactive == this.IsInactive) return; 
            
            IsInactive = isInactive;
            passiveImage.gameObject.SetActive(isInactive);
        }
        
        public virtual void UpdatePotionText()
        {
            PotionData.UpdateDescription();
            nameTextField.text = PotionData.PotionName;
            descTextField.text = PotionData.MyDescription;
            //manaTextField.text = PotionData.ManaCost.ToString();
        }
        
        #endregion
        
        #region Routines
        //protected virtual IEnumerator DisPotionRoutine(bool destroy = true)
        //{
        //    var timer = 0f;
        //    //transform.SetParent(CollectionManager.HandController.disPotionTransform);
            
        //    var startPos = CachedTransform.localPosition;
        //    var endPos = Vector3.zero;

        //    var startScale = CachedTransform.localScale;
        //    var endScale = Vector3.zero;

        //    var startRot = CachedTransform.localRotation;
        //    var endRot = Quaternion.Euler(Random.value * 360, Random.value * 360, Random.value * 360);
            
        //    while (true)
        //    {
        //        timer += Time.deltaTime*5;

        //        CachedTransform.localPosition = Vector3.Lerp(startPos, endPos, timer);
        //        CachedTransform.localRotation = Quaternion.Lerp(startRot,endRot,timer);
        //        CachedTransform.localScale = Vector3.Lerp(startScale, endScale, timer);
                
        //        if (timer>=1f)  break;
                
        //        yield return CachedWaitFrame;
        //    }

        //    if (destroy)
        //        Destroy(gameObject);
           
        //}
        
        //protected virtual IEnumerator ExhaustRoutine(bool destroy = true)
        //{
        //    var timer = 0f;
        //    transform.SetParent(CollectionManager.HandController.exhaustTransform);
            
        //    var startPos = CachedTransform.localPosition;
        //    var endPos = Vector3.zero;

        //    var startScale = CachedTransform.localScale;
        //    var endScale = Vector3.zero;

        //    var startRot = CachedTransform.localRotation;
        //    var endRot = Quaternion.Euler(Random.value * 360, Random.value * 360, Random.value * 360);
            
        //    while (true)
        //    {
        //        timer += Time.deltaTime*5;

        //        CachedTransform.localPosition = Vector3.Lerp(startPos, endPos, timer);
        //        CachedTransform.localRotation = Quaternion.Lerp(startRot,endRot,timer);
        //        CachedTransform.localScale = Vector3.Lerp(startScale, endScale, timer);
                
        //        if (timer>=1f)  break;
                
        //        yield return CachedWaitFrame;
        //    }

        //    if (destroy)
        //        Destroy(gameObject);
           
        //}

        #endregion

        #region Pointer Events
        //public virtual void OnPointerEnter(PointerEventData eventData)
        //{
        //    ShowTooltipInfo();
        //}

        //public virtual void OnPointerExit(PointerEventData eventData)
        //{
        //    HideTooltipInfo(TooltipManager.Instance);
        //}

        //public virtual void OnPointerDown(PointerEventData eventData)
        //{
        //    HideTooltipInfo(TooltipManager.Instance);
        //}

        //public virtual void OnPointerUp(PointerEventData eventData)
        //{
        //    ShowTooltipInfo();
        //}
        #endregion

        #region Tooltip
        //protected virtual void ShowTooltipInfo()
        //{
        //    if (!descriptionRoot) return;
        //    if (PotionData.KeywordsList.Count<=0) return;
           
        //    var tooltipManager = TooltipManager.Instance;
        //    foreach (var PotionDataSpecialKeyword in PotionData.KeywordsList)
        //    {
        //        var specialKeyword = tooltipManager.SpecialKeywordData.SpecialKeywordBaseList.Find(x=>x.SpecialKeyword == PotionDataSpecialKeyword);
        //        if (specialKeyword != null)
        //            ShowTooltipInfo(tooltipManager,specialKeyword.GetContent(),specialKeyword.GetHeader(),descriptionRoot,CursorType.Default,CollectionManager ? CollectionManager.HandController.cam : Camera.main);
        //    }
        //}
        //public virtual void ShowTooltipInfo(TooltipManager tooltipManager, string content, string header = "", Transform tooltipStaticTransform = null, CursorType targetCursor = CursorType.Default,Camera cam = null, float delayShow =0)
        //{
        //    tooltipManager.ShowTooltip(content,header,tooltipStaticTransform,targetCursor,cam,delayShow);
        //}

        //public virtual void HideTooltipInfo(TooltipManager tooltipManager)
        //{
        //    tooltipManager.HideTooltip();
        //}
        #endregion
       
    }
}