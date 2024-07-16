using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NueGames.NueDeck.Scripts.Managers;
using NueGames.NueDeck.Scripts.Utils;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class MainMenuManager : MonoBehaviour
{
    protected GameManager GameManager => GameManager.Instance;
    protected UIManager UIManager => UIManager.Instance;

    [SerializeField] private Button contiueGame;
    [SerializeField] private Button newGame;
    [SerializeField] private GameObject settingPanel;

    private List<string> localizedTexts = new List<string>();

    private void Start()
    {
        AudioManager.Instance.PlayMusic(AudioManager.Instance.mainThemeSong);

        if (PlayerPrefs.GetInt("IsContinue") == 1)
        {
            GameManager.Instance.IsContinue = PlayerPrefs.GetInt("IsContinue");
            contiueGame.interactable = true;
        }
        else
        {
            contiueGame.interactable = false;
        }
    }

    public void NewGame()
    {
        GameManager.IsContinue = 0;
        GameManager.gameStatus = 0;
        GameManager.SetNewGame();
        GameManager.SetPlayerHealth();
    }

    public void ChangeScene()
    {
        if (!PlayerPrefs.HasKey("TutorialDone"))
            newGame.GetComponent<SceneChanger>().OpenIntroScene();
        else
            newGame.GetComponent<SceneChanger>().OpenMapScene();
        
    }

    public void DeactiveGroupButton(GameObject gameObject)
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Transform child = gameObject.transform.GetChild(i);

            Button button = child.GetComponent<Button>();

            if (button != null)
            {
                button.interactable = false;
            }
        }
    }

    public void ToggleSetting()
    {
        switch (settingPanel.activeSelf)
        {
            case false:
                settingPanel.SetActive(true);
                break;
            case true:
                settingPanel.SetActive(false);
                break;

        }
    }
}
