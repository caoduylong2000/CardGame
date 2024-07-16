using System.Collections;
using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

public class TrainingScene : MonoBehaviour
{
    public Scrollbar VerticalScroll;
    public Text crystalText;
    public GameObject confirmPanel;

    private void Start()
    {
        VerticalScroll.value = 1;

        crystalText.text = GameManager.Instance.PersistentGameplayData.CurrentCrystal.ToString();
    }

    private void FixedUpdate()
    {
        crystalText.text = GameManager.Instance.PersistentGameplayData.CurrentCrystal.ToString();

        GameManager.Instance.SaveSystem.SaveTrainingDataToJson();
    }

    public void CloseConfirm()
    {
        confirmPanel.SetActive(false);
    }
}
