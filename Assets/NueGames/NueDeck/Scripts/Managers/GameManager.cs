using NueGames.NueDeck.Scripts.Card;
using NueGames.NueDeck.Scripts.Potion;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Data.Containers;
using NueGames.NueDeck.Scripts.Data.Settings;
using NueGames.NueDeck.Scripts.EnemyBehaviour;
using NueGames.NueDeck.Scripts.NueExtentions;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System.Collections.Generic;
using UnityEngine.Android;

namespace NueGames.NueDeck.Scripts.Managers
{
    [DefaultExecutionOrder(-10)]
    public class GameManager : MonoBehaviour
    {
        public GameManager() { }
        public static GameManager Instance { get; private set; }

        [Header("Settings")]
        [SerializeField] private GameplayData gameplayData;
        [SerializeField] private EncounterData encounterData;
        [SerializeField] private SceneData sceneData;

        #region Cache
        public SceneData SceneData => sceneData;
        public EncounterData EncounterData => encounterData;
        public GameplayData GameplayData => gameplayData;
        public PersistentGameplayData PersistentGameplayData { get; private set; }

        [HideInInspector] public int gameStatus; //-1 -> Defeated, 0 -> Unfinished, 1-> Completed
        [HideInInspector] public int IsContinue = 0;

        public string playerName;
        public int gameScore;
        public int playerCurrentHealth;
        public int playerMaxHealth;

        [HideInInspector] public int yearlyQuota;

        [HideInInspector] public bool IsInventory;
        [HideInInspector] public GameObject itemForSale;
        [HideInInspector] public int salePrice;
        [HideInInspector] public CardData cardData;
        [HideInInspector] public PotionData potionData;
        [HideInInspector] public StageData SavedStageData;

        public VoacationalTraining TrainingIndex;

        public SaveSystem SaveSystem;
        public Leaderboard Leaderboard;
        public MapManager mapManager;

        public List<Locale> locales;

        protected UIManager UIManager => UIManager.Instance;

        [Header("TrainingIndex")]
        public float AttackBonus;
        public float RecoveryBonus;
        public float ClosingRate;
        public float LegendItemRate;
        public float MoneyBonus;
        public float DesireBonus;
        public float PlayerHP;
        public float CrystalRate;

        public string TrainingJson;
        #endregion

        #region Setup
        private void Awake()
        {
            Setup();
            VoacationalTraining temp = JsonUtility.FromJson<VoacationalTraining>(TrainingJson);

            foreach(var levelInfo in temp.ListTrainingLevelInfo)
            {
                TrainingIndex.ListTrainingLevelInfo.Add(levelInfo);
            }
        }

        private void Start()
        {
            SaveSystem.LoadTrainingDataFromJson();

#if UNITY_ANDROID
            if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite) ||
            !Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
            {
                Permission.RequestUserPermission(Permission.ExternalStorageWrite);
                Permission.RequestUserPermission(Permission.ExternalStorageRead);
            }
            
#elif UNITY_IOS

#endif
        }

        public void Setup()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                transform.parent = null;
                Instance = this;
                DontDestroyOnLoad(gameObject);
                SetNewGame();
                CardActionProcessor.Initialize();
                PotionEffectProcessor.Initialize();
                EnemyActionProcessor.Initialize();
            }
        }
        #endregion

        public void SetNewGame()
        {
            InitGameplayData();
            SetInitalHand();
            SetPotionPack();
            SavedStageData.StageList.Clear();
        }

        private void FixedUpdate()
        {
            SetTrainingIndex();
        }

        #region Public Methods
        public void InitGameplayData()
        {
            PersistentGameplayData = new PersistentGameplayData(gameplayData);
            if (UIManager)
                UIManager.InformationCanvas.ResetCanvas();
        }
        public CardBase BuildAndGetCard(CardData targetData, Transform parent)
        {
            var clone = Instantiate(GameplayData.CardPrefab, parent);
            clone.SetCard(targetData);
            return clone;
        }
        private void SetInitalHand()
        {
            PersistentGameplayData.CurrentCardsList.Clear();

            if (PersistentGameplayData.IsRandomHand)
                for (var i = 0; i < GameplayData.RandomCardCount; i++)
                    PersistentGameplayData.CurrentCardsList.Add(GameplayData.AllCardsList.CardPoolObject.RandomItem());
            else
                foreach (var cardData in GameplayData.InitalDeck.CardList)
                    PersistentGameplayData.CurrentCardsList.Add(cardData);
        }

        public void SetPotionPack()
        {
            PersistentGameplayData.CurrentPotionsList.Clear();

            foreach (var potionData in GameplayData.PotionPack.PotionList)
                PersistentGameplayData.CurrentPotionsList.Add(potionData);
        }

        public void SetPlayerHealth()
        {
            playerMaxHealth = (int)(PersistentGameplayData.AllyList[0].AllyCharacterData.MaxHealth * PlayerHP);

            playerCurrentHealth = playerMaxHealth;

            UIManager.InformationCanvas.ResetCanvas();
        }

        public void NextEncounter()
        {
            PersistentGameplayData.CurrentEncounterId++;

            var currentEncounterList = EncounterData.EnemyEncounterList[0].EnemyEncounterList;

            if (PersistentGameplayData.CurrentEncounterId >= currentEncounterList.Count)
            {
                PersistentGameplayData.CurrentEncounterId = Random.Range(0, currentEncounterList.Count);
            }
        }

        public void NextStage()
        {
            PersistentGameplayData.CurrentEncounterGroup++;
        }

        public void SaveGameData()
        {
            SaveSystem.SavePlayerDataToJson();

            SaveSystem.SaveStageDataToJson();

            PlayerPrefs.SetInt("IsContinue", 1);
        }

        public void LoadGameData()
        {
            SaveSystem.LoadPlayerDataFromJson();

            Debug.Log("Start Load Stage");
            SaveSystem.LoadStageDataFromJson();
        }

        private void SetTrainingIndex()
        {
            AttackBonus = TrainingIndex.ListTrainingLevelInfo[PersistentGameplayData.AttackBonusLv].ListIndexMul.AttackBonus;
            RecoveryBonus = TrainingIndex.ListTrainingLevelInfo[PersistentGameplayData.RecoveryBonusLv].ListIndexMul.RecoveryBonus;
            ClosingRate = TrainingIndex.ListTrainingLevelInfo[PersistentGameplayData.ClosingRateLv].ListIndexMul.ClosingRate;
            LegendItemRate = TrainingIndex.ListTrainingLevelInfo[PersistentGameplayData.LegendItemRateLv].ListIndexMul.LegendItemRate;
            MoneyBonus = TrainingIndex.ListTrainingLevelInfo[PersistentGameplayData.MoneyBonusLv].ListIndexMul.MoneyBonus;
            DesireBonus = TrainingIndex.ListTrainingLevelInfo[PersistentGameplayData.DesireBonusLv].ListIndexMul.DesireBonus;
            PlayerHP = TrainingIndex.ListTrainingLevelInfo[PersistentGameplayData.PlayerHPLv].ListIndexMul.PlayerHP;
            CrystalRate = TrainingIndex.ListTrainingLevelInfo[PersistentGameplayData.CrystalRateLv].ListIndexMul.CrystalRate;
        }

        public void ActivateButton(Button button)
        {
            button.interactable = true;
        }

        public void DeactivateButton(Button button)
        {
            button.interactable = false;
        }

        public void ChangeLanguage(int langID)
        {
            LocalizationSettings.SelectedLocale = locales[langID];
        }
        #endregion
    }
}
