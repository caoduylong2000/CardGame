using System.Collections.Generic;
using UnityEngine;
using NueGames.NueDeck.Scripts.Data.Collection;
public class RandomEvent : MonoBehaviour
{
    [SerializeField] private List<EventAssets> EventList;
    [SerializeField] private DialougeLoadScript eventScript;

    private void Start()
    {
        int RandomStage = Random.Range(0, EventList.Count);

        for (int i = 0; i < EventList.Count; i++)
        {
            if (i == RandomStage)
                eventScript.selectedEvent = EventList[i];
        }
    }
}
