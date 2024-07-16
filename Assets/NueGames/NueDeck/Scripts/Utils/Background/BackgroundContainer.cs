using System;
using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NueGames.NueDeck.Scripts.Utils.Background
{
    public class BackgroundContainer : MonoBehaviour
    {
        [SerializeField] private List<BackgroundRoot> backgroundRootList;
        public List<BackgroundRoot> BackgroundRootList => backgroundRootList;
        
        private CombatManager CombatManager => CombatManager.Instance;
        private TutorialCombatManager TutorialCombatManager => TutorialCombatManager.Instance;

        public void OpenSelectedBackground()
        {
            if (SceneManager.GetActiveScene().buildIndex == GameManager.Instance.SceneData.tutorialSceneIndex)
            {
                var encounter = TutorialCombatManager.CurrentEncounter;
                foreach (var backgroundRoot in BackgroundRootList)
                    backgroundRoot.gameObject.SetActive(encounter.TargetBackgroundType == backgroundRoot.BackgroundType);
            }
            else if (SceneManager.GetActiveScene().buildIndex == GameManager.Instance.SceneData.combatSceneIndex)
            {
                var encounter = CombatManager.CurrentEncounter;
                foreach (var backgroundRoot in BackgroundRootList)
                    backgroundRoot.gameObject.SetActive(encounter.TargetBackgroundType == backgroundRoot.BackgroundType);
            }

            
        }
    }
}