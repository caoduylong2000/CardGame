using System;
using System.Collections;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NueGames.NueDeck.Scripts.Utils
{
    public class SceneChanger : MonoBehaviour
    {

        private GameManager GameManager => GameManager.Instance;
        private UIManager UIManager => UIManager.Instance;
        
        private enum SceneType
        {
            MainMenu,
            Map,
            Combat,
            Rest,
            Merchant,
            Treasure,
            Event,
            Training,
            LeaderBoard,
            Intro,
            Tutorial
        }

        public void OpenMainMenuScene()
        {
            StartCoroutine(DelaySceneChange(SceneType.MainMenu));
        }
        private IEnumerator DelaySceneChange(SceneType type)
        {
            UIManager.SetCanvas(UIManager.Instance.InventoryCanvas, false, true);

            yield return StartCoroutine(UIManager.Instance.Fade(true));

            switch (type)
            {
                case SceneType.MainMenu:
                    UIManager.ChangeScene(GameManager.SceneData.mainMenuSceneIndex);
                    UIManager.SetCanvas(UIManager.CombatCanvas,false,true);
                    UIManager.SetCanvas(UIManager.InformationCanvas,false,true);
                    UIManager.SetCanvas(UIManager.RewardCanvas,false,true);

                    break;
                case SceneType.Map:
                    UIManager.ChangeScene(GameManager.SceneData.mapSceneIndex);
                    UIManager.SetCanvas(UIManager.CombatCanvas,false,true);
                    UIManager.SetCanvas(UIManager.TutorialCanvas, false, true);
                    UIManager.SetCanvas(UIManager.InformationCanvas,true,false);
                    UIManager.SetCanvas(UIManager.RewardCanvas,false,true);
                   
                    break;
                case SceneType.Combat:
                    UIManager.ChangeScene(GameManager.SceneData.combatSceneIndex);
                    UIManager.SetCanvas(UIManager.CombatCanvas,false,true);
                    UIManager.SetCanvas(UIManager.InformationCanvas,true,false);
                    UIManager.SetCanvas(UIManager.RewardCanvas,false,true);
                    
                    break;
                case SceneType.Rest:
                    UIManager.ChangeScene(GameManager.SceneData.restSceneIndex);
                    UIManager.SetCanvas(UIManager.InformationCanvas, true, false);
                    break;
                case SceneType.Merchant:
                    UIManager.ChangeScene(GameManager.SceneData.merchantSceneIndex);
                    UIManager.SetCanvas(UIManager.InformationCanvas, true, false);
                    break;
                case SceneType.Treasure:
                    UIManager.ChangeScene(GameManager.SceneData.treasureSceneIndex);
                    UIManager.SetCanvas(UIManager.InformationCanvas, true, false);
                    UIManager.SetCanvas(UIManager.RewardCanvas, false, true);
                    break;
                case SceneType.Event:
                    UIManager.ChangeScene(GameManager.SceneData.eventSceneIndex);
                    UIManager.SetCanvas(UIManager.InformationCanvas, true, false);
                    UIManager.SetCanvas(UIManager.RewardCanvas, false, true);
                    break;
                case SceneType.Training:
                    UIManager.ChangeScene(GameManager.SceneData.trainingSceneIndex);
                    break;
                case SceneType.LeaderBoard:
                    UIManager.ChangeScene(GameManager.SceneData.leaderboardSceneIndex);
                    break;
                case SceneType.Intro:
                    UIManager.ChangeScene(GameManager.SceneData.introSceneIndex);
                    break;
                case SceneType.Tutorial:
                    UIManager.ChangeScene(GameManager.SceneData.tutorialSceneIndex);
                    UIManager.SetCanvas(UIManager.TutorialCanvas, true, false);
                    UIManager.SetCanvas(UIManager.CombatCanvas, true, false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
        public void OpenMapScene()
        {
            StartCoroutine(DelaySceneChange(SceneType.Map));
        }
        public void OpenCombatScene()
        {
            StartCoroutine(DelaySceneChange(SceneType.Combat));
        }
        public void OpenRestScene()
        {
            StartCoroutine(DelaySceneChange(SceneType.Rest));
        }
        public void OpenMerchantScene()
        {
            StartCoroutine(DelaySceneChange(SceneType.Merchant));
        }
        public void OpenTreasureScene()
        {
            StartCoroutine(DelaySceneChange(SceneType.Treasure));
        }
        public void OpenEventScene()
        {
            StartCoroutine(DelaySceneChange(SceneType.Event));
        }
        public void OpenTrainingScene()
        {
            StartCoroutine(DelaySceneChange(SceneType.Training));
        }
        public void OpenLeaderboardScene()
        {
            StartCoroutine(DelaySceneChange(SceneType.LeaderBoard));
        }
        public void OpenIntroScene()
        {
            StartCoroutine(DelaySceneChange(SceneType.Intro));
        }
        public void OpenTutorialScene()
        {
            StartCoroutine(DelaySceneChange(SceneType.Tutorial));
        }
        public void ChangeScene(int sceneId)
        {
            if (sceneId == GameManager.SceneData.mainMenuSceneIndex)
                OpenMainMenuScene();
            else if (sceneId == GameManager.SceneData.mapSceneIndex)
                OpenMapScene();
            else if (sceneId == GameManager.SceneData.combatSceneIndex)
                OpenCombatScene();
            else if (sceneId == GameManager.SceneData.restSceneIndex)
                OpenRestScene();
            else if (sceneId == GameManager.SceneData.merchantSceneIndex)
                OpenMerchantScene();
            else if (sceneId == GameManager.SceneData.treasureSceneIndex)
                OpenTreasureScene();
            else if (sceneId == GameManager.SceneData.eventSceneIndex)
                OpenEventScene();
            else if (sceneId == GameManager.SceneData.trainingSceneIndex)
                OpenTrainingScene();
            else if (sceneId == GameManager.SceneData.leaderboardSceneIndex)
                OpenLeaderboardScene();
            else if (sceneId == GameManager.SceneData.introSceneIndex)
                OpenIntroScene();
            else if (sceneId == GameManager.SceneData.tutorialSceneIndex)
                OpenTutorialScene();
            else
                SceneManager.LoadScene(sceneId);
        }
    }
}
