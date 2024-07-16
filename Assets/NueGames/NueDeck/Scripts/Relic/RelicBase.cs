using System;
using System.Collections;
using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
using NueGames.NueDeck.Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NueGames.NueDeck.Scripts.Relic
{
    public class RelicBase : MonoBehaviour
    {
        [Header("Base References")]
        [SerializeField] protected Transform descriptionRoot;
        [SerializeField] protected Image relicImage;
        [SerializeField] protected Image passiveImage;
        [SerializeField] protected TextMeshProUGUI nameTextField;
        [SerializeField] protected TextMeshProUGUI descTextField;
        [SerializeField] protected List<RarityRoot> rarityRootList;
        

        #region Cache
        public RelicData RelicData { get; private set; }
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

        private void Update()
        {
            
        }

        public virtual void SetRelic(RelicData targetProfile,bool isPlayable = true)
        {
            RelicData = targetProfile;
            IsPlayable = isPlayable;

            RelicData.UpdateDescription();

            nameTextField.text = RelicData.RelicName;
            descTextField.text = RelicData.MyDescription;
            relicImage.sprite = RelicData.RelicSprite;

            foreach (var rarityRoot in RarityRootList)
            {
                rarityRoot.gameObject.SetActive(rarityRoot.Rarity == RelicData.Rarity);
            }   
        }
        
        #endregion
        
        #region Relic Methods
        public virtual void Use(CharacterBase self,CharacterBase targetCharacter)
        {
            if (CanUseRelic(self, targetCharacter))
            {
                StartCoroutine(RelicUseRoutine(self, targetCharacter));
            }
        }

        private bool CanUseRelic(CharacterBase self, CharacterBase targetCharacter)
        {
            // Thực hiện kiểm tra điều kiện phù hợp ở đây.
            // Ví dụ:
            // Nếu relic chỉ có thể được sử dụng cho đồng minh và nhân vật mục tiêu là đồng minh, return true.
            // Ngược lại, return false.
            return true;
        }

        private IEnumerator RelicUseRoutine(CharacterBase self,CharacterBase targetCharacter)
        {
            foreach (var playerAction in RelicData.RelicEffectDataList)
            {
                yield return new WaitForSeconds(playerAction.ActionDelay);
                var targetList = DetermineTargets(targetCharacter, playerAction);

                foreach (var target in targetList)
                    RelicEffectProcessor.GetAction(playerAction.RelicEffectType)
                        .DoAction(new RelicEffectParameters(playerAction.ActionValue,
                            target, self, RelicData, this));
            }
        }

        private static List<CharacterBase> DetermineTargets(CharacterBase targetCharacter, RelicEffectData playerAction)
        {
            List<CharacterBase> targetList = new List<CharacterBase>();

            Debug.Log(playerAction.ActionTargetType);

            switch (playerAction.ActionTargetType)
            {
                case ActionTargetType.Enemy:
                    targetList.Add(targetCharacter);
                    break;
                case ActionTargetType.Ally:
                    targetList.Add(targetCharacter);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return targetList;
        }

        public virtual void SetInactiveMaterialState(bool isInactive) 
        {
            if (!IsPlayable) return;
            if (isInactive == this.IsInactive) return; 
            
            IsInactive = isInactive;
            passiveImage.gameObject.SetActive(isInactive);
        }
        
        public virtual void UpdateRelicText()
        {
            RelicData.UpdateDescription();
            nameTextField.text = RelicData.RelicName;
            descTextField.text = RelicData.MyDescription;
        }
        
        #endregion
    }
}