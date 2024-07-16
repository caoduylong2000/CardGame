using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using NueGames.NueDeck.Scripts.Utils;
using NueGames.NueDeck.Scripts.Managers;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Data.Collection;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using System.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;

public class DialougeLoadScript : MonoBehaviour
{
    public EventAssets selectedEvent;
    private Story eventStory;

    public Text textPrefab;
    public LocalizeStringEvent textStringEvent;

    public Button buttonPrefab;
    public LocalizeStringEvent buttonStringEvent;
    

    public GameObject textArea;
    public GameObject buttonArea;

    // Start is called before the first frame update
    void Start()
    {
        eventStory = new Story(selectedEvent.eventScript.text);

        _ = ResetUIAsync();
    }

    async Task ResetUIAsync()
    {
        DeleteUI();

        Text storyText = Instantiate(textPrefab);

        storyText.GetComponent<LocalizeStringEvent>().StringReference.TableReference = selectedEvent.eventLocalized.TableReference;

        storyText.text = await LoadStoryAsync();

        storyText.transform.SetParent(textArea.transform, false);
        
        foreach (Choice choice in eventStory.currentChoices)
        {
            Button choiceButton = Instantiate(buttonPrefab);

            choiceButton.GetComponentInChildren<Text>().text = await GetLocalizedLineAsync(selectedEvent.eventScript.name, choice.tags[0], choice.text);

            choiceButton.transform.SetParent(buttonArea.transform, false);

            ProcessActions(choice.text, choiceButton);

            choiceButton.onClick.AddListener(delegate
            {
                ChooseStoryChoice(choice);
            });
        }
    }

    void DeleteUI()
    {
        for(int i = 0; i < textArea.transform.childCount; i++)
        {
            Destroy(textArea.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < buttonArea.transform.childCount; i++)
        {
            Destroy(buttonArea.transform.GetChild(i).gameObject);
        }
    }

    void ChooseStoryChoice(Choice choice)
    {
        eventStory.ChooseChoiceIndex(choice.index);

        _ = ResetUIAsync();
    }

    async Task<string> LoadStoryAsync()
    {
        string text = "";

        if (eventStory.canContinue)
        {
            text = eventStory.ContinueMaximally();
        }

        for (int i = 0; i < eventStory.currentTags.Count; i++)
        {
            if (eventStory.currentTags[i].StartsWith("text"))
                text = await GetLocalizedLineAsync(selectedEvent.eventScript.name, eventStory.currentTags[i], eventStory.currentText);
        }

        return text;
    }

    void ProcessActions(string actionsString, Button choiceButton)
    {
        string[] actions = actionsString.Split(',');

        foreach (string action in actions)
        {
            string trimmedAction = action.Trim();

            switch (trimmedAction)
            {
                case string a when a.Contains("Cost") && a.Contains("G"):
                    int costValue = ExtractValue(trimmedAction);
                    choiceButton.onClick.AddListener(delegate {
                        gameObject.GetComponent<EventStage>().CostGold(costValue);
                    });
                    break;
                case string b when b.Contains("MaxHP"):
                    int maxHpValue = ExtractValue(trimmedAction);
                    choiceButton.onClick.AddListener(delegate {
                        gameObject.GetComponent<EventStage>().IncreaseMaxHealth(maxHpValue);
                    });
                    break;
                case string c when c.Contains("HP"):
                    int hpValue = ExtractValue(trimmedAction);
                    choiceButton.onClick.AddListener(delegate {
                        gameObject.GetComponent<EventStage>().Heal(hpValue);
                    });
                    break;
                case string d when d.Contains("Map"):
                    choiceButton.onClick.AddListener(delegate {
                        GameManager.Instance.NextStage();
                        gameObject.GetComponent<SceneChanger>().OpenMapScene();
                    });
                    break;
                case string e when e.Contains("demons"):
                    choiceButton.onClick.AddListener(delegate {
                        gameObject.GetComponent<EventStage>().LoseHealth(15);
                    });
                    break;
                case string f when f.Contains("monster"):
                    choiceButton.onClick.AddListener(delegate {
                        gameObject.GetComponent<EventStage>().LoseHealth(5);
                    });
                    break;
                case string g when g.Contains("small bag"):
                    choiceButton.onClick.AddListener(delegate {
                        UIManager.Instance.RewardCanvas.gameObject.SetActive(true);
                        UIManager.Instance.RewardCanvas.PrepareCanvas();
                        UIManager.Instance.RewardCanvas.BuildReward(RewardType.Card);
                        UIManager.Instance.RewardCanvas.BuildReward(RewardType.Potion);
                    });
                    break;
                case string h when h.Contains("gold"):
                    choiceButton.onClick.AddListener(delegate {
                        UIManager.Instance.RewardCanvas.gameObject.SetActive(true);
                        UIManager.Instance.RewardCanvas.PrepareCanvas();
                        UIManager.Instance.RewardCanvas.BuildReward(RewardType.Gold);
                    });
                    break;
                default:
                    Debug.LogWarning("Hành động không được nhận diện: " + trimmedAction);
                break;
            }
        }
    }

    int ExtractValue(string action)
    {
        string[] parts = action.Split(' ');
        if (parts.Length >= 2)
        {
            string valueString = parts[1].Replace("G", "");
            int value;
            if (int.TryParse(valueString, out value))
            {
                return value;
            }
        }
        return 0;
    }

    public static async Task<string> GetLocalizedLineAsync(string table, string localizationKey, string fallbackText)
    {
        if (string.IsNullOrEmpty(localizationKey))
        {
            return fallbackText;
        }

        var localizedString = new LocalizedString(table, localizationKey);
        AsyncOperationHandle<string> handle = localizedString.GetLocalizedStringAsync();

        // Wait for the operation to complete
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            return handle.Result;
        }
        else
        {
            Debug.LogWarning($"Failed to load localized string for key: {localizationKey}");
            return fallbackText;
        }
    }

    public async Task<string> ShowLocalizedText(string table, string localizationKey, string fallbackText)
    {
        string localizedText = await DialougeLoadScript.GetLocalizedLineAsync(table, localizationKey, fallbackText);
        Debug.Log(localizedText);
        return localizedText;
    }
}

