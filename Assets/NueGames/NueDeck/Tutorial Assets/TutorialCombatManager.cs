using System;
using System.Collections;
using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Characters.Enemies;
using NueGames.NueDeck.Scripts.Data.Containers;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.UI;
using NueGames.NueDeck.Scripts.Utils;
using NueGames.NueDeck.Scripts.Utils.Background;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Managers
{
    public class TutorialCombatManager : MonoBehaviour
    {
        private TutorialCombatManager() { }
        public static TutorialCombatManager Instance { get; private set; }

        public EnemyExample tutorialEnemy;

        [Header("References")]
        [SerializeField] private BackgroundContainer backgroundContainer;
        [SerializeField] private List<Transform> enemyPosList;
        [SerializeField] private List<Transform> allyPosList;

        #region Cache
        public List<EnemyBase> CurrentEnemiesList { get; private set; } = new List<EnemyBase>();
        public List<AllyBase> CurrentAlliesList { get; private set; }= new List<AllyBase>();

        public Action OnAllyTurnStarted;
        public Action OnEnemyTurnStarted;
        public List<Transform> EnemyPosList => enemyPosList;

        public List<Transform> AllyPosList => allyPosList;

        public AllyBase CurrentMainAlly => CurrentAlliesList.Count>0 ? CurrentAlliesList[0] : null;

        public EnemyBase CurrentMainEnemy => CurrentEnemiesList.Count > 0 ? CurrentEnemiesList[0] : null;

        public EnemyEncounter CurrentEncounter { get; private set; }

        public CombatStateType CurrentCombatStateType
        {
            get => _currentCombatStateType;
            private set
            {
                ExecuteCombatState(value);
                _currentCombatStateType = value;
            }
        }

        public CombatStateType _currentCombatStateType;
        protected FxManager FxManager => FxManager.Instance;
        protected AudioManager AudioManager => AudioManager.Instance;
        protected GameManager GameManager => GameManager.Instance;
        protected UIManager UIManager => UIManager.Instance;

        protected TutorialCollectionManager CollectionManager => TutorialCollectionManager.Instance;

        #endregion
        
        
        #region Setup
        private void Awake()
        {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.battleThemeSong);

            if (Instance)
            {
                Destroy(gameObject);
                return;
            } 
            else
            {
                Instance = this;
                CurrentCombatStateType = CombatStateType.PrepareCombat;
            }
        }

        private void Start()
        {
            StartCombat();
            GameManager.IsInventory = true;
            UIManager.CombatCanvas.CombatLosePanel.SetActive(false);
            UIManager.CombatCanvas.Inventory.SetActive(true);
            var cardSpawnRoot = UIManager.CombatCanvas.Inventory.GetComponentInChildren<InventoryCanvas>().CardSpawnRoot;
            var itemTransforms = cardSpawnRoot.transform.childCount;

            if (itemTransforms > 0)
            {
                List<Transform> objectsToDestroy = new List<Transform>();

                for (int i = 0; i < itemTransforms; i++)
                {
                    objectsToDestroy.Add(cardSpawnRoot.transform.GetChild(i));
                }

                foreach (var obj in objectsToDestroy)
                {
                    obj.gameObject.SetActive(false);
                }
            }

            UIManager.CombatCanvas.Inventory.GetComponentInChildren<MerchantInventoryHelper>().OpenCardInventory(GameManager.Instance.PersistentGameplayData.CurrentCardsList, "Current Cards");
        }

        public void StartCombat()
        {
            BuildEnemies();
            BuildAllies();
            backgroundContainer.OpenSelectedBackground();

            CollectionManager.SetGameDeck();
            CollectionManager.SetPotionPack();

            UIManager.CombatCanvas.gameObject.SetActive(true);
            //UIManager.InformationCanvas.gameObject.SetActive(true); 
        }

        public void SetNewItemData()
        {
            CollectionManager.ClearPiles();

            CollectionManager.SetGameDeck();

            CollectionManager.SetPotionPack();

            CurrentCombatStateType = CombatStateType.AllyTurn;
        }

        private void ExecuteCombatState(CombatStateType targetStateType)
        {
            switch (targetStateType)
            {
                case CombatStateType.PrepareCombat:
                    break;
                case CombatStateType.AllyTurn:

                    OnAllyTurnStarted?.Invoke();

                    if (CurrentMainAlly.CharacterStats.IsStunned)
                    {
                        EndTurn();
                        return;
                    }

                    GameManager.PersistentGameplayData.CurrentMana = GameManager.PersistentGameplayData.MaxMana;

                    CollectionManager.DrawCards(GameManager.PersistentGameplayData.DrawCount);

                    GameManager.PersistentGameplayData.CanSelectCards = true;

                    break;
                case CombatStateType.EnemyTurn:

                    OnEnemyTurnStarted?.Invoke();

                    CollectionManager.DiscardHand();

                    StartCoroutine(nameof(EnemyTurnRoutine));

                    GameManager.PersistentGameplayData.CanSelectCards = false;

                    break;
                case CombatStateType.EndCombat:

                    GameManager.PersistentGameplayData.CanSelectCards = false;

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetStateType), targetStateType, null);
            }
        }
        #endregion

        #region Public Methods
        public void EndTurn()
        {
            CurrentCombatStateType = CombatStateType.EnemyTurn;
        }
        public void OnAllyDeath(AllyBase targetAlly)
        {
            var targetAllyData = GameManager.PersistentGameplayData.AllyList.Find(x =>
                x.AllyCharacterData.CharacterID == targetAlly.AllyCharacterData.CharacterID);
            if (GameManager.PersistentGameplayData.AllyList.Count>1)
                GameManager.PersistentGameplayData.AllyList.Remove(targetAllyData);
            CurrentAlliesList.Remove(targetAlly);
            UIManager.InformationCanvas.ResetCanvas();
            if (CurrentAlliesList.Count<=0)
                LoseCombat();
        }
        public void OnEnemyDeath(EnemyBase targetEnemy)
        {
            targetEnemy.CharacterStats.CurrentHealth = 1;
        }
        public void DeactivateCardHighlights()
        {
            foreach (var currentEnemy in CurrentEnemiesList)
                currentEnemy.EnemyCanvas.SetHighlight(false);

            foreach (var currentAlly in CurrentAlliesList)
                currentAlly.AllyCanvas.SetHighlight(false);
        }
        public void IncreaseMana(int target)
        {
            GameManager.PersistentGameplayData.CurrentMana += target;
            UIManager.CombatCanvas.SetPileTexts();
        }
        public void HighlightCardTarget(ActionTargetType targetTypeTargetType)
        {
            switch (targetTypeTargetType)
            {
                case ActionTargetType.Enemy:
                    foreach (var currentEnemy in CurrentEnemiesList)
                        currentEnemy.EnemyCanvas.SetHighlight(true);
                    break;
                case ActionTargetType.Ally:
                    foreach (var currentAlly in CurrentAlliesList)
                        currentAlly.AllyCanvas.SetHighlight(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetTypeTargetType), targetTypeTargetType, null);
            }
        }
        #endregion
        
        #region Private Methods
        private void BuildEnemies()
        {
            CurrentEncounter = GameManager.EncounterData.GetEnemyEncounter(
                GameManager.PersistentGameplayData.CurrentEncounterId,
                GameManager.PersistentGameplayData.IsFinalEncounter);

            var enemyList = CurrentEncounter.EnemyList;
            for (var i = 0; i < enemyList.Count; i++)
            {
                var clone = Instantiate(enemyList[i].EnemyPrefab, EnemyPosList.Count >= i ? EnemyPosList[i] : EnemyPosList[0]);
                clone.BuildCharacter();
                CurrentEnemiesList.Add(clone);
            }
        }
        private void BuildAllies()
        {
            for (var i = 0; i < GameManager.PersistentGameplayData.AllyList.Count; i++)
            {
                var clone = Instantiate(GameManager.PersistentGameplayData.AllyList[i], AllyPosList.Count >= i ? AllyPosList[i] : AllyPosList[0]);
                clone.BuildCharacter();
                CurrentAlliesList.Add(clone);
            }
        }
        public void LoseCombat()
        {
            //Transform[] childTransforms = UIManager.CombatCanvas.SaleItemPosition.GetComponentsInChildren<Transform>();

            //for (int i = 1; i < childTransforms.Length; i++)
            //{
            //    Destroy(childTransforms[i].gameObject);
            //}

            if (CurrentCombatStateType == CombatStateType.EndCombat) return;
            
            CurrentCombatStateType = CombatStateType.EndCombat;
            
            CollectionManager.DiscardHand();
            CollectionManager.ClearPiles();
            UIManager.CombatCanvas.gameObject.SetActive(true);
            UIManager.CombatCanvas.CombatLosePanel.SetActive(true);

            GameManager.gameStatus = -1;
        }
        public void WinCombat()
        {
            Transform[] childTransforms = UIManager.CombatCanvas.SaleItemPosition.GetComponentsInChildren<Transform>();

            for (int i = 1; i < childTransforms.Length; i++)
            {
                Destroy(childTransforms[i].gameObject);
            }


            if (CurrentCombatStateType == CombatStateType.EndCombat) return;
          
            CurrentCombatStateType = CombatStateType.EndCombat;
           
            foreach (var allyBase in CurrentAlliesList)
            {
                GameManager.PersistentGameplayData.SetAllyHealthData(allyBase.AllyCharacterData.CharacterID,
                    allyBase.CharacterStats.CurrentHealth, allyBase.CharacterStats.MaxHealth);
            }
            
            CollectionManager.ClearPiles();
            CurrentMainAlly.CharacterStats.ClearAllStatus();

            UIManager.CombatCanvas.gameObject.SetActive(true);
            UIManager.CombatCanvas.CombatWinPanel.SetActive(true);
        }
        #endregion
        
        #region Routines
        private IEnumerator EnemyTurnRoutine()
        {
            var waitDelay = new WaitForSeconds(0.1f);

            foreach (var currentEnemy in CurrentEnemiesList)
            {
                yield return currentEnemy.StartCoroutine(nameof(EnemyExample.ActionRoutine));
                yield return waitDelay;
            }

            if (CurrentCombatStateType != CombatStateType.EndCombat)
                CurrentCombatStateType = CombatStateType.AllyTurn;
        }

        public void EndTutorial()
        {
            PlayerPrefs.SetInt("TutorialDone", 1);
            UIManager.TutorialCanvas.gameObject.SetActive(false);
        }
        #endregion
    }
}