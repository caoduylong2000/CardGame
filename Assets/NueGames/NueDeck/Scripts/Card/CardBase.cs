using System;
using System.Collections;
using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
using NueGames.NueDeck.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace NueGames.NueDeck.Scripts.Card
{
    public class CardBase : MonoBehaviour //,I2DTooltipTarget, IPointerDownHandler, IPointerUpHandler
    {
        [Header("Base References")]
        [SerializeField] protected Transform descriptionRoot;
        [SerializeField] protected Image cardImage;
        [SerializeField] protected Image passiveImage;
        [SerializeField] protected Text nameTextField;
        [SerializeField] protected Text descTextField;
        [SerializeField] protected Text manaTextField;
        [SerializeField] protected List<RarityRoot> rarityRootList;
        [SerializeField] protected List<CardTypeRoot> cardTypeRootList;



        #region Cache
        public CardData CardData { get; private set; }
        public bool IsInactive { get; protected set; }
        protected Transform CachedTransform { get; set; }
        protected WaitForEndOfFrame CachedWaitFrame { get; set; }
        public bool IsPlayable { get; protected set; } = true;

        public List<RarityRoot> RarityRootList => rarityRootList;
        public List<CardTypeRoot> CardTypeRootList => cardTypeRootList;
        protected FxManager FxManager => FxManager.Instance;
        protected AudioManager AudioManager => AudioManager.Instance;
        protected GameManager GameManager => GameManager.Instance;
        protected CombatManager CombatManager => CombatManager.Instance;
        protected CollectionManager CollectionManager => CollectionManager.Instance;
        protected TutorialCombatManager TutorialCombatManager => TutorialCombatManager.Instance;
        protected TutorialCollectionManager TutorialCollectionManager => TutorialCollectionManager.Instance;

        public bool IsExhausted { get; private set; }

        #endregion
        
        #region Setup
        protected virtual void Awake()
        {
            CachedTransform = transform;
            CachedWaitFrame = new WaitForEndOfFrame();
        }

        public virtual void SetCard(CardData targetProfile,bool isPlayable = true)
        {
            CardData = targetProfile;
            IsPlayable = isPlayable;

            CardData.UpdateDescription();
            nameTextField.text = CardData.CardName;
            descTextField.text = CardData.MyDescription;
            manaTextField.text = CardData.ManaCost.ToString();
            cardImage.sprite = CardData.CardSprite;

            foreach (var rarityRoot in RarityRootList)
            {
                rarityRoot.gameObject.SetActive(rarityRoot.Rarity == CardData.Rarity);
            }

            foreach (var cardTypeRoot in CardTypeRootList)
            {
                if (cardTypeRoot == null)
                {
                    Debug.LogWarning("Found a null element in CardTypeRootList");
                }
                else
                {
                    cardTypeRoot.gameObject.SetActive(cardTypeRoot.CardType == CardData.CardType);
                }
            }
        }
        
        #endregion
        
        #region Card Methods
        public virtual void Use(CharacterBase self,CharacterBase targetCharacter)
        {
            if (!IsPlayable) return;

            StartCoroutine(CardUseRoutine(self, targetCharacter));

            if (!PlayerPrefs.HasKey("UseCardTutorial"))
            {
                PlayerPrefs.SetInt("UseCardTutorial", 0);
            }
        }

        private IEnumerator CardUseRoutine(CharacterBase self,CharacterBase targetCharacter)
        {
            SpendMana(CardData.ManaCost);

            var targetList = DetermineTargets(targetCharacter);

            for (int i = 0; i < CardData.CardActionDataList.Count; i++)
            {

                yield return new WaitForSeconds(CardData.CardActionDataList[i].ActionDelay);

                CardActionProcessor.GetAction(CardData.CardActionDataList[i].CardActionType)
                    .DoAction(new CardActionParameters(CardData.CardActionDataList[i].ActionValue, targetList[i], self, CardData, this));
            }
            if (SceneManager.GetActiveScene().buildIndex == GameManager.SceneData.tutorialSceneIndex)
            {
                TutorialCollectionManager.OnCardPlayed(this);
            }
            else
            {
                CollectionManager.OnCardPlayed(this);
            }
        }

        private List<CharacterBase> DetermineTargets(CharacterBase targetCharacter)
        {
            List<CharacterBase> targetList = new List<CharacterBase>();

            foreach (CardActionData playerAction in CardData.CardActionDataList)
            {
                if (playerAction.ActionTargetType == ActionTargetType.Enemy)
                {
                    if(SceneManager.GetActiveScene().buildIndex == GameManager.SceneData.tutorialSceneIndex)
                    {
                        targetCharacter = TutorialCombatManager.Instance.CurrentMainEnemy;
                    }
                    else
                    {
                        targetCharacter = CombatManager.Instance.CurrentMainEnemy;
                    }
                }
                else if (playerAction.ActionTargetType == ActionTargetType.Ally)
                {
                    if (SceneManager.GetActiveScene().buildIndex == GameManager.SceneData.tutorialSceneIndex)
                    {
                        targetCharacter = TutorialCombatManager.Instance.CurrentMainAlly;
                    }
                    else
                    {
                        targetCharacter = CombatManager.Instance.CurrentMainAlly;
                    }
                }

                if (targetCharacter != null)
                {
                    targetList.Add(targetCharacter);
                }
                else
                {
                    throw new InvalidOperationException("Không thể xác định targetCharacter.");
                }
            }

            return targetList;
        }
        
        public virtual void Discard()
        {
            if (IsExhausted) return;
            if (!IsPlayable) return;

            if (SceneManager.GetActiveScene().buildIndex == GameManager.SceneData.tutorialSceneIndex)
            {
                TutorialCollectionManager.OnCardDiscarded(this);
            }
            else
            {
                CollectionManager.OnCardDiscarded(this);
            }
            
            StartCoroutine(DiscardRoutine());
        }
        
        public virtual void Exhaust(bool destroy = true)
        {
            if (IsExhausted) return;
            if (!IsPlayable) return;
            IsExhausted = true;

            if (SceneManager.GetActiveScene().buildIndex == GameManager.SceneData.tutorialSceneIndex)
            {
                CollectionManager.OnCardExhausted(this);
            }
            else
            {
                CollectionManager.OnCardExhausted(this);
            }
            
            StartCoroutine(ExhaustRoutine(destroy));
        }

        protected virtual void SpendMana(int value)
        {
            if (!IsPlayable) return;
            GameManager.PersistentGameplayData.CurrentMana -= value;
        }

        public virtual void Closing(float value)
        {
            if (!IsPlayable) return;

            var x = UIManager.Instance.CombatCanvas.SuccessPercentage.value;
            var y = value / 100;

            var successMaxRange = Mathf.Min(.99f,x+y);

            var successValue = Random.Range(0f, 0.99f);

            if (successValue <= successMaxRange)
            {
                
                if (SceneManager.GetActiveScene().buildIndex == GameManager.SceneData.tutorialSceneIndex)
                {
                    TutorialCombatManager.WinCombat();
                }
                else
                {
                    CombatManager.WinCombat();
                }
            }
        }

        public virtual void SetInactiveMaterialState(bool isInactive) 
        {
            if (!IsPlayable) return;
            if (isInactive == this.IsInactive) return; 
            
            IsInactive = isInactive;
            passiveImage.gameObject.SetActive(isInactive);
        }
        
        public virtual void UpdateCardText()
        {
            CardData.UpdateDescription();
            nameTextField.text = CardData.CardName;
            descTextField.text = CardData.MyDescription;
            manaTextField.text = CardData.ManaCost.ToString();
        }
        
        #endregion
        
        #region Routines
        protected virtual IEnumerator DiscardRoutine(bool destroy = true)
        {
            var timer = 0f;

            if (SceneManager.GetActiveScene().buildIndex == GameManager.SceneData.tutorialSceneIndex)
            {
                transform.SetParent(TutorialCollectionManager.HandController.discardTransform);
            }
            else
            {
                transform.SetParent(CollectionManager.HandController.discardTransform);
            }

            var startPos = CachedTransform.localPosition;
            var endPos = Vector3.zero;

            var startScale = CachedTransform.localScale;
            var endScale = Vector3.zero;

            var startRot = CachedTransform.localRotation;
            var endRot = Quaternion.Euler(Random.value * 360, Random.value * 360, Random.value * 360);
            
            while (true)
            {
                timer += Time.deltaTime*5;

                CachedTransform.localPosition = Vector3.Lerp(startPos, endPos, timer);
                CachedTransform.localRotation = Quaternion.Lerp(startRot,endRot,timer);
                CachedTransform.localScale = Vector3.Lerp(startScale, endScale, timer);
                
                if (timer>=1f)  break;
                
                yield return CachedWaitFrame;
            }

            if (destroy)
                Destroy(gameObject);
           
        }
        
        protected virtual IEnumerator ExhaustRoutine(bool destroy = true)
        {
            var timer = 0f;
            

            if (SceneManager.GetActiveScene().buildIndex == GameManager.SceneData.tutorialSceneIndex)
            {
                transform.SetParent(TutorialCollectionManager.HandController.exhaustTransform);
            }
            else
            {
                transform.SetParent(CollectionManager.HandController.exhaustTransform);
            }

            var startPos = CachedTransform.localPosition;
            var endPos = Vector3.zero;

            var startScale = CachedTransform.localScale;
            var endScale = Vector3.zero;

            var startRot = CachedTransform.localRotation;
            var endRot = Quaternion.Euler(Random.value * 360, Random.value * 360, Random.value * 360);
            
            while (true)
            {
                timer += Time.deltaTime*5;

                CachedTransform.localPosition = Vector3.Lerp(startPos, endPos, timer);
                CachedTransform.localRotation = Quaternion.Lerp(startRot,endRot,timer);
                CachedTransform.localScale = Vector3.Lerp(startScale, endScale, timer);
                
                if (timer>=1f)  break;
                
                yield return CachedWaitFrame;
            }

            if (destroy)
                Destroy(gameObject);
           
        }

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
        //    if (CardData.KeywordsList.Count<=0) return;
           
        //    var tooltipManager = TooltipManager.Instance;
        //    foreach (var cardDataSpecialKeyword in CardData.KeywordsList)
        //    {
        //        var specialKeyword = tooltipManager.SpecialKeywordData.SpecialKeywordBaseList.Find(x=>x.SpecialKeyword == cardDataSpecialKeyword);
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