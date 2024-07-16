using System;
using System.Collections.Generic;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Containers
{
    [CreateAssetMenu(fileName = "Event Data", menuName = "NueDeck/Containers/EventData", order = 6)]
    public class EventData : ScriptableObject
    {
        [Header("Settings")]
        [SerializeField] private List<EventInfomation> eventInfomation;

        public List<EventInfomation> EventInfomation => eventInfomation;
    }


    [Serializable]
    public class EventInfomation
    {
        [SerializeField] private string name;
        [SerializeField] private int eventId;
        [SerializeField] private Sprite eventCard;
        [SerializeField] private int sceneIndex;

        public string Name => name;
        public int EventId => eventId;
        public Sprite EventCard => eventCard;
        public int SceneIndex => sceneIndex;
    }
}

