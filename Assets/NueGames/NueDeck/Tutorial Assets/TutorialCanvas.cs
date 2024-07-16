using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NueGames.NueDeck.Scripts.Managers;
using System;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace NueGames.NueDeck.Scripts.UI
{
    public class TutorialCanvas : CanvasBase
    {
        [SerializeField] private float typingSpeed = 0.04f;

        [SerializeField] private GameObject messageObject;
        [SerializeField] private Text textObject;

        public List<TutorialObject> tutorialObjects;
        public int currentStep = -1;
        public string currentMessage;
        private bool couroutineRunning;

        [Header("Step 1: Choose item to sale")]
        public GameObject step1Object;

        [SerializeField] private string text11;
        [SerializeField] private string text12;

        private bool endText1 = false;
        private bool endStep1 = false;

        [Header("Step 2: UI Presentation")]
        public GameObject step2Object;

        [SerializeField] private string text21;
        [SerializeField] private string text22;

        private bool endText21 = false;
        private bool endText22 = false;

        [Header("Step 3: Use card & Desire Level")]
        public GameObject step3Object;

        [SerializeField] private string text31;

        private bool endText31 = false;

        [Header("Step 4: Case 1: Desire Level reach 50%")]
        public GameObject step4Object;

        [SerializeField] private string text41;

        public bool closingCaseActive = false;

        private bool endText41 = false;

        [Header("Step 5: Case 2: Closing Success with < 50%")]
        public GameObject step5Object;

        public string text51;
        public string text52;
        public string text53;
        public string text54;

        private bool endText51 = false;
        private bool endText52 = false;
        private bool endText53 = false;
        private bool endText54 = false;

        [Header("Step 6: Case Closing Card")]
        public GameObject step6Object;

        public string text61;
        public string text62;
        public string text63;

        private bool endText61 = false;
        private bool endText62 = false;
        private bool endText63 = false;

        // Start is called before the first frame update
        void Start()
        {
            Step11Call();
        }

        // Update is called once per frame
        void Update()
        {
            if (UIManager.CombatCanvas.Inventory.activeSelf == false)
            {
                step1Object.SetActive(false);
            }

            if (UIManager.CombatCanvas.Inventory.GetComponentInChildren<InventoryCanvas>().useComfirm.activeSelf == true && endText1 == false)
            {
                DeactiveTutorialObjects();
                Step12Call();
                endText1 = true;
            }
            #region Step2

            if (UIManager.CombatCanvas.Inventory.GetComponentInChildren<InventoryCanvas>().useComfirm.activeSelf == false && endText21 == false && currentStep != 0)
            {
                step2Object.SetActive(true);
                DeactiveTutorialObjects();
                StartCoroutine(DisplayLine("text21", textObject));
                endText21 = true;
            }

            if (currentStep == 6 && endText22 == false)
            {
                endText22 = true;
                StartCoroutine(DisplayLine("text22", textObject));
                step2Object.SetActive(false);
                step3Object.SetActive(true);
            }
            #endregion

            #region Step3

            if (currentStep == 7 && PlayerPrefs.HasKey("UseCardTutorial"))
            {
                DeactiveTutorialObjects();
                PlayerPrefs.SetInt("UseCardTutorial", 1);
                ActiveTutorialObjects();
            }

            if (currentStep == 9 && endText31 == false)
            { 
                StartCoroutine(DisplayLine("text31", textObject));
                SetCurrentStep(10);
                endText31 = true;
                step3Object.SetActive(false);
            }

            #endregion

            #region Case1

            if (UIManager.CombatCanvas.SuccessPercentage.value >= .5f & currentStep == 10 && endText41 == false)
            {
                StartCoroutine(DisplayLine("text41", textObject));
                step4Object.SetActive(true);
                SetCurrentStep(11);
                ActiveTutorialObjects();
                endText41 = true;
                SetCurrentStep(10);
                closingCaseActive = true;
            }
            #endregion

            #region CaseClosing

            if (currentStep == 14 && endText61 == false && messageObject.activeSelf == false)
            {
                StartCoroutine(DisplayLine("text61", textObject));
                endText61 = true;
            }

            if (currentStep == 14 && endText61 == true && endText62 == false && messageObject.activeSelf == false)
            {
                StartCoroutine(DisplayLine("text62", textObject));
                endText62 = true;
            }

            if (currentStep == 14 && endText62 == true && endText63 == false && messageObject.activeSelf == false)
            {
                StartCoroutine(DisplayLine("text63", textObject));
                endText63 = true;
                SetCurrentStep(10);
            }

            #endregion

            #region Case 2

            if (UIManager.CombatCanvas.SuccessPercentage.value <= .5f & currentStep == 10 && UIManager.CombatCanvas.CombatWinPanel.activeSelf == true && endText51 == false)
            {
                step5Object.SetActive(true);
                SetCurrentStep(14);
                StartCoroutine(DisplayLine("text51", textObject));
                endText51 = true;
            }

            if (endText51 == true && endText52 == false && messageObject.activeSelf == false)
            {
                StartCoroutine(DisplayLine("text52", textObject));
                endText52 = true;
            }

            if (endText52 == true && endText53 == false && messageObject.activeSelf == false)
            {
                StartCoroutine(DisplayLine("text53", textObject));
                ActiveTutorialObjects();
                endText53 = true;
            }

            if (endText53 == true && endText54 == false && messageObject.activeSelf == false)
            {
                StartCoroutine(DisplayLine("text54", textObject));
                DeactiveTutorialObjects();
                endText54 = true;
                SetCurrentStep(9);
                step5Object.SetActive(false);
            }

            #endregion
        }

        #region Step1

        private void Step11Call()
        {
            step1Object.SetActive(true);
            StartCoroutine(DisplayLine("text11", textObject));
        }

        public void Step12Call()
        {
            StartCoroutine(DisplayLine("text12", textObject));
        }
        #endregion

        #region CaseClosing

        public void ClosingCase()
        {
            step6Object.SetActive(true);
            SetCurrentStep(13);
            ActiveTutorialObjects();
        }

        #endregion


        public void SetCurrentStep(int step)
        {
            currentStep = step;
        }

        IEnumerator Delay(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
        }

        public void AcvivateObject(GameObject gameObject)
        {
            gameObject.SetActive(true);
        }

        public void DectivateObject(GameObject gameObject)
        {
            gameObject.SetActive(false);
        }

        public void DectivateMessageObject(GameObject gameObject)
        {
            if (couroutineRunning == true)
            {
                StopAllCoroutines();
                textObject.text = currentMessage;
                couroutineRunning = false;
            }
            else
            {
                gameObject.SetActive(false);
                ActiveTutorialObjects();
            } 
        }

        public void ActiveTutorialObjects()
        {
            for(int i = 0; i < tutorialObjects[currentStep].blockPanels.Count; i++)
            {
                tutorialObjects[currentStep].blockPanels[i].SetActive(true);
            }

            for (int j = 0; j < tutorialObjects[currentStep].arrowPanels.Count; j++)
            {
                tutorialObjects[currentStep].arrowPanels[j].SetActive(true);
            }
        }

        public void DeactiveTutorialObjects()
        {
            for (int i = 0; i < tutorialObjects[currentStep].blockPanels.Count; i++)
            {
                tutorialObjects[currentStep].blockPanels[i].SetActive(false);
            }

            for (int j = 0; j < tutorialObjects[currentStep].arrowPanels.Count; j++)
            {
                tutorialObjects[currentStep].arrowPanels[j].SetActive(false);
            }

            currentStep++;
        }

        private IEnumerator DisplayLine(string lineKey, Text textObject)
        {
            couroutineRunning = true;
            messageObject.SetActive(true);
            textObject.text = "";

            var localizedString = new LocalizedString("Tutorial", lineKey);

            var operation = localizedString.GetLocalizedString();
            currentMessage = operation;
            yield return operation;

            foreach (char letter in operation.ToCharArray())
            {
                textObject.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }

            couroutineRunning = false;
        }

        public void SkipTutorial()
        {
            PlayerPrefs.SetInt("TutorialDone", 1);

            Transform[] childTransforms = UIManager.CombatCanvas.SaleItemPosition.GetComponentsInChildren<Transform>();

            for (int i = 1; i < childTransforms.Length; i++)
            {
                Destroy(childTransforms[i].gameObject);
            }
        }
    }

    [Serializable]
    public class TutorialObject
    {
        public List<GameObject> blockPanels;
        public List<GameObject> arrowPanels;
    }
}
