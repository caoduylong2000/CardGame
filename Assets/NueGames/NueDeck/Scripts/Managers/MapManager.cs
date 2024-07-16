using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;
using NueGames.NueDeck.Scripts.Data.Containers;
using NueGames.NueDeck.Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Data.Collection;
//using TMPro;

namespace NueGames.NueDeck.Scripts.Managers
{
    public class MapManager : MonoBehaviour
    {
        public Scrollbar VerticalScroll;

        public static int StageCount;

        [SerializeField] private List<GameObject> encounterButtonGroup;
        [SerializeField] private EventData eventData;

        public List<GameObject> EncounterButtonGroup => encounterButtonGroup;
        public EventData EventData => eventData;

        public GameObject ParentView;
        public GameObject MonthGroup;
        public List<GameObject> ListStages;

        [SerializeField] private GameObject yearlyQuotaPanel;
        [SerializeField] private RectTransform yearlyQuotaWindowPosition;
        [SerializeField] private Text yearlyQuotaText;
        [SerializeField] private Text currentYearText;
        private float moveDuration = 1f;

        private int monthsInYear = 12;

        private GameManager GameManager => GameManager.Instance;
        private UIManager UIManager => UIManager.Instance;

        private void Awake()
        {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.mainThemeSong);

            if (GameManager.IsContinue == 1)
            {
                LoadStageData();
                GameManager.IsContinue = -1;
            }
            else if (GameManager.IsContinue == 0)
            {
                for (int i = 0; i < ListStages.Count; i++)
                {
                    ListStages[i].GetComponentInChildren<EncounterButton>().isFilpped = false;
                }

                for (int i = 0; i < monthsInYear; i++)
                {
                    GameObject groupClone = Instantiate(MonthGroup, ParentView.transform);

                    for (int j = 0; j < 3; j++)
                    {
                        if (i == 0 || i == monthsInYear - 1)
                        {
                            GameObject prefabToClone = ListStages[0];

                            Instantiate(prefabToClone, groupClone.transform);
                        }
                        else
                        {
                            //GameObject prefabToClone = ListStages[Random.Range(0, ListStages.Count)];
                            GameObject prefabToClone = ListStages[4];
                            Instantiate(prefabToClone, groupClone.transform);
                        }
                    }
                    EncounterButtonGroup.Add(groupClone);
                }
                GameManager.IsContinue = -1;
            }
            else
            {
                LoadStageData();
            }

            StageCount = EncounterButtonGroup.Count;
            SaveStageData();
        }

        private void Start()
        {

            if (GameManager.gameStatus == 0 && GameManager.PersistentGameplayData.CurrentEncounterGroup == 0)
            {
                OpenQuotaCanvas();
            }
            PrepareEncountersGroup();
            OnStageCompleted(GameManager.PersistentGameplayData.CurrentEncounterGroup);
            GameManager.SaveSystem.LoadTrainingDataFromJson();

            if (GameManager.PersistentGameplayData.CurrentEncounterGroup == StageCount - 1)
            {
                GameManager.PersistentGameplayData.IsFinalEncounter = true;
            }

            if (GameManager.PersistentGameplayData.CurrentEncounterGroup >= StageCount)
            {
                GameManager.gameStatus = 1;
            }

            UIManager.Instance.InformationCanvas.GameStatusCheck();
        } 

        private void Update()
        {
            BlockStage();
            SaveStageData();
        }

        private void FixedUpdate()
        {
            if (GameManager.gameStatus == 0)
            {
                GameManager.SaveGameData();
            }
            else return;
        }

        private void BlockStage()
        {
            for(int i=0; i < EncounterButtonGroup.Count; i++)
            {
                var grp = EncounterButtonGroup[i];
                var cardFlipped = 0;
                if (GameManager.PersistentGameplayData.CurrentEncounterGroup == i)
                {
                    for (int a = 0; a < EncounterButtonGroup[i].GetComponentsInChildren<EncounterButton>().Length; a++)
                    {
                        if(grp.GetComponentsInChildren<EncounterButton>()[a].isFilpped == true)
                        {
                            cardFlipped++;
                            if(cardFlipped >= 2)
                            {
                                for (int j = 0; j < EncounterButtonGroup[i].GetComponentsInChildren<EncounterButton>().Length; j++)
                                {
                                    if (grp.GetComponentsInChildren<EncounterButton>()[j].isFilpped == false)
                                    {
                                        grp.GetComponentsInChildren<EncounterButton>()[j].SetStatus(EncounterButtonStatus.Passive);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void PrepareEncountersGroup()
        {

            for (int i = 0; i < EncounterButtonGroup.Count; i++)
            {
                var grp = EncounterButtonGroup[i];
                if (GameManager.PersistentGameplayData.CurrentEncounterGroup == i)
                {
                    for (int a = 0; a < EncounterButtonGroup[i].GetComponentsInChildren<EncounterButton>().Length; a++)
                    {
                        grp.GetComponentsInChildren<EncounterButton>()[a].SetStatus(EncounterButtonStatus.Active);
                    }
                }
            
                else if (GameManager.PersistentGameplayData.CurrentEncounterGroup > i)
                    for (int a = 0; a < EncounterButtonGroup[i].GetComponentsInChildren<EncounterButton>().Length; a++)
                    {
                        if (EncounterButtonGroup[i].GetComponentsInChildren<EncounterButton>()[a].isFilpped == true)
                            grp.GetComponentsInChildren<EncounterButton>()[a].SetStatus(EncounterButtonStatus.Opened);
                        else
                            grp.GetComponentsInChildren<EncounterButton>()[a].SetStatus(EncounterButtonStatus.Completed);
                    }
                        
                else
                    for (int a = 0; a < EncounterButtonGroup[i].GetComponentsInChildren<EncounterButton>().Length; a++)
                    {
                        grp.GetComponentsInChildren<EncounterButton>()[a].SetStatus(EncounterButtonStatus.Passive);
                    }  
            }
        }

        public void OnStageCompleted(int completedStageIndex)
        {

            StartCoroutine(UpdateScrollbarValue(completedStageIndex));
        }

        private IEnumerator UpdateScrollbarValue(int completedStageIndex)
        {
            yield return null;

            if (completedStageIndex > 0)
            {
                VerticalScroll.value = (completedStageIndex - 1) * 0.143f;
                if (VerticalScroll.value > 1)
                    VerticalScroll.value = 1;
            }
            else
            {
                VerticalScroll.value = 0;
            }
        }

        public void SaveStageData()
        {
            GameManager.SavedStageData.StageList.Clear();

            for (int i = 0; i < EncounterButtonGroup.Count; i++)
            {
                StageData.Stage stage = new StageData.Stage(new List<StageData.Stage.StagePart>());

                for (int j = 0; j < EncounterButtonGroup[i].transform.childCount; j++)
                {
                    if (EncounterButtonGroup[i].transform.GetChild(j).name == "Combat(Clone)")
                    {
                        StageData.Stage.StagePart item = new StageData.Stage.StagePart(ListStages[0], false);
                        //item.StageItem = ListStages[0];
                        if (EncounterButtonGroup[i].transform.GetChild(j).GetComponentInChildren<EncounterButton>().isFilpped == true)
                            item.StageFlipped = true;

                        stage.StageParts.Add(item);
                    }
                    else if (EncounterButtonGroup[i].transform.GetChild(j).name == "Rest(Clone)")
                    {
                        StageData.Stage.StagePart item = new StageData.Stage.StagePart(ListStages[1], false);
                        //item.StageItem = ListStages[1];
                        if (EncounterButtonGroup[i].transform.GetChild(j).GetComponentInChildren<EncounterButton>().isFilpped == true)
                            item.StageFlipped = true;

                        stage.StageParts.Add(item);
                    }
                    else if (EncounterButtonGroup[i].transform.GetChild(j).name == "Treasure(Clone)")
                    {
                        StageData.Stage.StagePart item = new StageData.Stage.StagePart(ListStages[2], false);
                        //item.StageItem = ListStages[2];
                        if (EncounterButtonGroup[i].transform.GetChild(j).GetComponentInChildren<EncounterButton>().isFilpped == true)
                            item.StageFlipped = true;

                        stage.StageParts.Add(item);
                    }
                    else if (EncounterButtonGroup[i].transform.GetChild(j).name == "Merchant(Clone)")
                    {
                        StageData.Stage.StagePart item = new StageData.Stage.StagePart(ListStages[3], false);
                        //item.StageItem = ListStages[3];
                        if (EncounterButtonGroup[i].transform.GetChild(j).GetComponentInChildren<EncounterButton>().isFilpped == true)
                            item.StageFlipped = true;

                        stage.StageParts.Add(item);
                    }
                    else if (EncounterButtonGroup[i].transform.GetChild(j).name == "Event(Clone)")
                    {
                        StageData.Stage.StagePart item = new StageData.Stage.StagePart(ListStages[4], false);
                        //item.StageItem = ListStages[4];
                        if (EncounterButtonGroup[i].transform.GetChild(j).GetComponentInChildren<EncounterButton>().isFilpped == true)
                            item.StageFlipped = true;

                        stage.StageParts.Add(item);
                    }
                    else return;
                }
                GameManager.SavedStageData.StageList.Add(stage);
            }
        }

        
        private void LoadStageData()
        {
            EncounterButtonGroup.Clear();

            for (int i = 0; i < GameManager.SavedStageData.StageList.Count; i++)
            {
                GameObject groupClone = Instantiate(MonthGroup, ParentView.transform);

                for (int j = 0; j < GameManager.SavedStageData.StageList[i].StageParts.Count; j++)
                {
                    GameObject prefabToClone = GameManager.SavedStageData.StageList[i].StageParts[j].StageItem;

                    EncounterButton buttonflip = prefabToClone.GetComponentInChildren<EncounterButton>();

                    if (buttonflip != null)
                        buttonflip.isFilpped = GameManager.SavedStageData.StageList[i].StageParts[j].StageFlipped;
                    else
                        buttonflip.isFilpped = false;

                    Instantiate(prefabToClone, groupClone.transform);
                }

                EncounterButtonGroup.Add(groupClone);
            }
        }

        private void OpenQuotaCanvas()
        {
            yearlyQuotaPanel.SetActive(true);

            GameManager.yearlyQuota = 50 + (GameManager.PersistentGameplayData.CurrentStageId * 50);
            StartCoroutine(MoveInEffectCoroutine(yearlyQuotaWindowPosition));
            yearlyQuotaText.text = GameManager.yearlyQuota.ToString();
            currentYearText.text = "Year " + (GameManager.PersistentGameplayData.CurrentStageId + 1).ToString();
        }

        public void CloseQuotaCanvas()
        {
            StartCoroutine(MoveOutEffectCoroutine(yearlyQuotaWindowPosition));
            StartCoroutine(Delay(3.0f));
            yearlyQuotaPanel.SetActive(false);
        }

        IEnumerator Delay(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
        }

        IEnumerator MoveInEffectCoroutine(RectTransform targetRectTransform)
        {
            Vector3 startPos = targetRectTransform.localPosition;

            Vector3 endPos = new Vector3 (startPos.x, 0f, startPos.z);

            float elapsed = 0f;

            while (elapsed < moveDuration)
            {
                targetRectTransform.localPosition = Vector3.Lerp(startPos, endPos, elapsed / moveDuration);

                elapsed += Time.deltaTime;

                yield return null;
            }

            targetRectTransform.localPosition = endPos;
        }

        IEnumerator MoveOutEffectCoroutine(RectTransform targetRectTransform)
        {
            Vector3 startPos = targetRectTransform.localPosition;

            Vector3 endPos = new Vector3(startPos.x, 3000f, startPos.z);

            float elapsed = 0f;

            while (elapsed < moveDuration)
            {
                targetRectTransform.localPosition = Vector3.Lerp(startPos, endPos, elapsed / moveDuration);

                elapsed += Time.deltaTime;

                yield return null;
            }

            targetRectTransform.localPosition = endPos;
        }

        public void DeactiveGroupStage()
        {
                
        }
    }
}
