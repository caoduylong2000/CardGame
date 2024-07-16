using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using NueGames.NueDeck.Scripts.Utils;
using NueGames.NueDeck.Scripts.Managers;
using System;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class IntroDialogue : MonoBehaviour
{
    //[Header("Ink JSON")]
    //[SerializeField] private TextAsset inkJSON;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private Text dialogueText;
    [SerializeField] private Text charName;
    [SerializeField] private Image bgObject;
    [SerializeField] private List<Sprite> BGIntroList;
    [SerializeField] private float delayDialogue = 2f;
    private float transitionDuration = 0.5f;


    [Header("Params")]
    [SerializeField] private float typingSpeed = 0.04f;

    [SerializeField] private TextAsset introDialogue;

    private Story currentStory;
    private Coroutine displayLineCoroutine;
    private bool canContinueToNextLine = false;
    private bool displayingLine = false;

    private const string CHARACTER_TAG = "character";
    private const string AVATAR_TAG = "avatar";
    private const string LAYOUT_TAB = "side";
    private const string BG_TAG = "background";

    private string localizationKey;
    private string localizationLine;

    public bool dialogueIsPlaying { get; private set; }

    private void Start()
    {
        AudioManager.Instance.PlayMusic(AudioManager.Instance.introThemeSong);

        StartCoroutine(Delay(delayDialogue));
    }

    private IEnumerator Delay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        EnterDialogueMode(introDialogue);
    }

    private void Update()
    {
        if (!dialogueIsPlaying)
            return;

        localizationKey = GetLocalizationKey();
        localizationLine = GetLocalizedLine(localizationKey);
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        dialogueBox.SetActive(true);

        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialogueBox.SetActive(true);
    }

    public void ExitDialogue()
    {
        StartCoroutine(ExitDialogueMode());
    }

    public IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(.2f);

        dialogueIsPlaying = false;
        dialogueBox.SetActive(false);
        dialogueText.text = "";

        PlayerPrefs.SetInt("FirstIntroDone",1);

        gameObject.GetComponent<SceneChanger>().OpenTutorialScene();
    }

    public void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            if (displayingLine)
            {
                StopCoroutine(displayLineCoroutine);
                dialogueText.text = localizationLine;
                displayingLine = false;
                canContinueToNextLine = true;

                HandleTags(currentStory.currentTags);
            }
            else
            {
                DisplayNextLine();

                HandleTags(currentStory.currentTags);
            }
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    private void DisplayNextLine()
    {
        if (displayLineCoroutine != null)
        {
            StopCoroutine(displayLineCoroutine);
        }

        displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));
    }

    private IEnumerator DisplayLine(string line)
    {
        dialogueText.text = "";
        displayingLine = true; 

        canContinueToNextLine = false;

        string langKey = GetLocalizationKey();
        string langLine = GetLocalizedLine(langKey);

        foreach (char letter in langLine.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        displayingLine = false;
        canContinueToNextLine = true;
    }

    private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');

            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                case CHARACTER_TAG:
                    string newCharName = GetLocalizedLine(tagValue);

                    charName.text = newCharName;
                    if (tagValue == "default")
                    {
                        charName.text = "";
                    }
                    break;

                case BG_TAG:
                    if (int.TryParse(tagValue, out int intValue))
                    {
                        StartCoroutine(TransitionCoroutine(intValue));
                    }
                    break;
                default:
                    break;
            }
        }
    }

    IEnumerator TransitionCoroutine(int intValue)
    {
        float t = 0.0f;
        int currentIndex = intValue;

        while (t < 1.0f)
        {
            t += Time.deltaTime / transitionDuration;

            bgObject.color = Color.Lerp(Color.white, Color.clear, t);

            yield return null;
        }

        bgObject.color = Color.clear;

        bgObject.sprite = BGIntroList[currentIndex];

        t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime / transitionDuration;

            bgObject.color = Color.Lerp(Color.clear, Color.white, t);

            yield return null;
        }

        bgObject.color = Color.white;
    }

    string GetLocalizationKey()
    {
        foreach (string tag in currentStory.currentTags)
        {
            string[] splitTag = tag.Split(':');

            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            if(tagKey == "ID")
            {
                return tagValue;
            }
        }
        return null;
    }

    string GetLocalizedLine(string localizationKey)
    {
        if (!string.IsNullOrEmpty(localizationKey))
        {
            var localizedString = new LocalizedString("Intro", localizationKey);
            return localizedString.GetLocalizedString();
        }
        else
        {
            return currentStory.currentText;
        }
    }
}
