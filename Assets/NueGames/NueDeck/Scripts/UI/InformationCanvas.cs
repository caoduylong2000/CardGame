using NueGames.NueDeck.Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using NueGames.NueDeck.Scripts.Utils;

namespace NueGames.NueDeck.Scripts.UI
{
    public class InformationCanvas : CanvasBase
    {
        [Header("Settings")]
        [SerializeField] private GameObject randomizedDeckObject;
        [SerializeField] private Text goldTextField;
        [SerializeField] private Text crystalTextField;
        [SerializeField] private Text nameTextField;
        [SerializeField] private Text healthTextField;
        [SerializeField] private Slider healthSlideField;

        public GameObject RandomizedDeckObject => randomizedDeckObject;
        public Text GoldTextField => goldTextField;
        public Text CrystalTextField => crystalTextField;
        public Text NameTextField => nameTextField;
        public Text HealthTextField => healthTextField;
        public Slider HealthSlideField => healthSlideField;

        [Header("Result")]
        [SerializeField] private float moveDuration = 1f;
        [SerializeField] private float increaseDuration = 2f;
        [SerializeField] private GameObject resultPanel;
        [SerializeField] private RectTransform resultWindow;
        [SerializeField] private Text resultTitle;
        [SerializeField] private Text goldText;
        [SerializeField] private Text quotaText;
        [SerializeField] private Button menuButton;
        [SerializeField] private Button nextButton;

        public float MoveDuration => moveDuration;
        public float IncreaseDuration => increaseDuration;
        public GameObject ResultPanel => resultPanel;
        public RectTransform ResultWindow => resultWindow;
        public Text ResultTitle => resultTitle;
        public Text GoldText => goldText;
        public Text QuotaText => quotaText;
        public Button MenuButton => menuButton;
        public Button NextButton => nextButton;

        [Header("Pause Menu")]
        [SerializeField] private GameObject pauseMenu;
        public GameObject PauseMenu => pauseMenu;

        [Header("Deck View")]
        [SerializeField] private GameObject deckView;
        public GameObject DeckView => deckView;

        [Header("Enter Player Name")]
        [SerializeField] private GameObject enterNameWindow;
        [SerializeField] private InputField inputName;
        public GameObject EnterNameWindow => enterNameWindow;
        public InputField InputName => inputName;

        [Header("Audio Manager")]
        public Slider bgmControl;
        public Slider sfxControl;

        public Toggle bgmCheckSound;
        public Toggle sfxCheckSound;

        #region Setup
        private void Awake()
        {
            ResetCanvas();

            bgmControl.value = AudioManager.Instance.musicSource.volume;
            sfxControl.value = AudioManager.Instance.sfxSource.volume;
        }
        #endregion

        private void Update()
        {
            if (bgmCheckSound.isOn)
            {
                AudioManager.Instance.musicSource.volume = bgmControl.value;
            }
            else
            {
                AudioManager.Instance.musicSource.volume = 0;
            }

            if (sfxCheckSound.isOn)
            {
                AudioManager.Instance.sfxSource.volume = sfxControl.value;
            }
            else
            {
                AudioManager.Instance.sfxSource.volume = 0;
            }
        }

        #region Public Methods

        public void SetGoldText(int value)=>GoldTextField.text = value.ToString();
        public void SetCrystalText(int value) => CrystalTextField.text = value.ToString();

        public void SetNameText(string name) => NameTextField.text = name;

        public void UpdateHealStats(int currentHealth, int maxHealth)
        {
            healthSlideField.value = (float)currentHealth / maxHealth;
            HealthTextField.text = $"{currentHealth}/{maxHealth}";
        }

        public override void ResetCanvas()
        {
            if (GameManager.PersistentGameplayData.AllyHealthDataList.Count > 0)
            {
                var data = GameManager.PersistentGameplayData.AllyHealthDataList[0];

                data.MaxHealth = (int)(GameManager.PersistentGameplayData.AllyList[0].AllyCharacterData.MaxHealth * GameManager.PlayerHP);

                UpdateHealStats(data.CurrentHealth, data.MaxHealth);
            }
            else
            {
                GameManager.playerMaxHealth = (int)(GameManager.PersistentGameplayData.AllyList[0].AllyCharacterData.MaxHealth * GameManager.PlayerHP);

                UpdateHealStats(GameManager.playerCurrentHealth, GameManager.playerMaxHealth);
            }

            RandomizedDeckObject.SetActive(GameManager.PersistentGameplayData.IsRandomHand);


            SetNameText(GameManager.GameplayData.DefaultName);
            UIManager.InformationCanvas.SetGoldText(GameManager.PersistentGameplayData.CurrentGold);
            UIManager.InformationCanvas.SetCrystalText(GameManager.PersistentGameplayData.CurrentCrystal);
        }
        #endregion

        public void OpenDeckView()
        {
            deckView.SetActive(true);

            deckView.GetComponentInChildren<MerchantInventoryHelper>().OpenInventory();
        }

        public void CloseDeckView()
        {
            deckView.SetActive(false);
        }

        public void OpenPauseMenu()
        {
            pauseMenu.SetActive(true);
        }

        public void ClosePauseMenu()
        {
            pauseMenu.SetActive(false);
        }

        public void PauseTime()
        {
            Time.timeScale = 0f;
        }

        public void ResumeTime()
        {
            Time.timeScale = 1f;
        }

        #region ResultPanel
        public void GameStatusCheck()
        {
            GameManager.yearlyQuota = 50 + (GameManager.PersistentGameplayData.CurrentStageId * 50);

            if (GameManager.gameStatus == 1 && GameManager.PersistentGameplayData.CurrentGold >= GameManager.yearlyQuota)
            {
                WinPanel();
            }
            else if (GameManager.gameStatus == -1 || GameManager.gameStatus == 1 && GameManager.PersistentGameplayData.CurrentGold <= GameManager.yearlyQuota
                || GameManager.PersistentGameplayData.AllyHealthDataList.Count > 0 && GameManager.PersistentGameplayData.AllyHealthDataList[0].CurrentHealth < 0)
            {
                LosePanel();
            }
            else
                return;
        }

        public void WinPanel()
        {
            GameManager.gameStatus = 0;

            if (ResultWindow != null)
            {
                ResultWindow.anchoredPosition = new Vector2(0f, 4000f);
            }

            ResultPanel.SetActive(true);
            ResultTitle.text = "Winner!!!";
            QuotaText.text = GameManager.yearlyQuota.ToString();
            StartCoroutine(MoveEffectCoroutine(ResultWindow));
            StartCoroutine(Delay(3.0f));
            StartCoroutine(NumberIncreaseEffectCoroutine(GoldText, GameManager.PersistentGameplayData.CurrentGold));
            MenuButton.gameObject.SetActive(true);
            NextButton.gameObject.SetActive(true);

        }

        public void LosePanel()
        {
            GameManager.gameStatus = 0;
            GameManager.IsContinue = 0;
            PlayerPrefs.SetInt("IsContinue", 0);

            if (GameManager.playerName == null || GameManager.playerName == "")
            {
                enterNameWindow.SetActive(true);
            }
            else
            {
                if (ResultWindow != null)
                {
                    ResultWindow.anchoredPosition = new Vector2(0f, 4000f);
                }

                ResultPanel.SetActive(true);
                ResultTitle.text = "Defeated";
                QuotaText.text = GameManager.yearlyQuota.ToString();
                StartCoroutine(MoveEffectCoroutine(ResultWindow));
                StartCoroutine(Delay(3.0f));
                StartCoroutine(NumberIncreaseEffectCoroutine(GoldText, GameManager.PersistentGameplayData.CurrentGold));
                MenuButton.gameObject.SetActive(true);
                NextButton.gameObject.SetActive(false);
                GameManager.gameScore = (GameManager.PersistentGameplayData.CurrentStageId) * 100 + (int)(GameManager.PersistentGameplayData.CurrentGold / 100);

                GameManager.Leaderboard.AddScore();

                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
            }    
        }

        public void SavePlayerName()
        {
            string data = inputName.text;

            GameManager.playerName = data;

            LosePanel();

            enterNameWindow.SetActive(false);
        }

        public void AbadonGame()
        {
            GameManager.gameStatus = -1;
            LosePanel();
        }

        public void CloseResultPanel()
        {
            ResultPanel.SetActive(false);
        }

        public void NextYear()
        {
            CloseResultPanel();
            GameManager.SavedStageData.StageList.Clear();
            GameManager.PersistentGameplayData.CurrentEncounterId = 0;
            GameManager.PersistentGameplayData.CurrentEncounterGroup = 0;
            GameManager.PersistentGameplayData.IsFinalEncounter = false;
            GameManager.PersistentGameplayData.CurrentStageId++;
            if (GameManager.PersistentGameplayData.AllyHealthDataList.Count > 0)
            {
                GameManager.PersistentGameplayData.AllyHealthDataList[0].CurrentHealth = (int)(GameManager.Instance.PersistentGameplayData.AllyList[0].AllyCharacterData.MaxHealth * GameManager.PlayerHP);
            }
            else
            {
                GameManager.playerCurrentHealth = (int)(GameManager.Instance.PersistentGameplayData.AllyList[0].AllyCharacterData.MaxHealth * GameManager.PlayerHP);
            }
            UIManager.InformationCanvas.ResetCanvas();
            GameManager.gameStatus = 0;
            GameManager.IsContinue = 0;

        }

        IEnumerator MoveEffectCoroutine(RectTransform targetRectTransform)
        {
            Vector3 startPos = targetRectTransform.localPosition;

            Vector3 endPos = new Vector3(startPos.x, 0f, startPos.z);

            float elapsed = 0f;

            while (elapsed < moveDuration)
            {
                targetRectTransform.localPosition = Vector3.Lerp(startPos, endPos, elapsed / moveDuration);

                elapsed += Time.deltaTime;

                yield return null;
            }

            targetRectTransform.localPosition = endPos;
        }

        IEnumerator NumberIncreaseEffectCoroutine(Text numberText, int targetNumber)
        {
            int startNumber = 0;
            float elapsed = 0f;

            while (elapsed < increaseDuration)
            {
                int currentNumber = Mathf.RoundToInt(Mathf.Lerp(startNumber, targetNumber, elapsed / increaseDuration));

                numberText.text = currentNumber.ToString();

                elapsed += Time.deltaTime;

                yield return null;
            }

            numberText.text = targetNumber.ToString();
        }

        IEnumerator Delay(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
        }

        public void SetContinueAvailable()
        {
            Transform[] childTransforms = UIManager.CombatCanvas.SaleItemPosition.GetComponentsInChildren<Transform>();

            for (int i = 1; i < childTransforms.Length; i++)
            {
                Destroy(childTransforms[i].gameObject);
            }

            GameManager.Instance.IsContinue = 1;
            PlayerPrefs.SetInt("IsContinue", 1);
        }
        #endregion
    }
}