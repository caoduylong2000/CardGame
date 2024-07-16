using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NueGames.NueDeck.Scripts.Managers;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.UI;

public class RemoveCardImage : MonoBehaviour
{
    private void Update()
    {
        CheckDeactivateButton();
    }

    public void RemoveImage()
    {
        gameObject.GetComponentInChildren<EncounterButton>().isFilpped = true;
    }

    public void CheckDeactivateButton()
    {
        if(gameObject.GetComponentInChildren<EncounterButton>().GetStatus() == EncounterButtonStatus.Passive
            || gameObject.GetComponentInChildren<EncounterButton>().GetStatus() == EncounterButtonStatus.Completed)
        {
            GetComponent<Button>().interactable = false;
        }    
        
    }
}
