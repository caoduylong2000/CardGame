using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Settings
{
    [CreateAssetMenu(fileName = "Scene Data", menuName = "NueDeck/Settings/Scene", order = 2)]
    public class SceneData : ScriptableObject
    {
        public int mainMenuSceneIndex = 0;
        public int mapSceneIndex = 1;
        public int combatSceneIndex = 2;
        public int restSceneIndex = 3;
        public int merchantSceneIndex = 4;
        public int treasureSceneIndex = 5;
        public int eventSceneIndex = 6;
        public int trainingSceneIndex = 7;
        public int leaderboardSceneIndex = 8;
        public int introSceneIndex = 9;
        public int tutorialSceneIndex = 10;
    }
}