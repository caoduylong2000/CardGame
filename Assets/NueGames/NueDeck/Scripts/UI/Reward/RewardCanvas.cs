using System;
using System.Collections;
using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Card;
using NueGames.NueDeck.Scripts.Potion;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Data.Containers;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.NueExtentions;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace NueGames.NueDeck.Scripts.UI.Reward
{
    public class RewardCanvas : CanvasBase
    {
        //[Header("Reward VFX")]
        //[SerializeField] Transform startPos;
        //[SerializeField] Transform crystalEndPos;
        //[SerializeField] Transform goldEndPos;
        //public Transform cardEndPos;


        [SerializeField] GameObject coinPrefab;
        [SerializeField] GameObject crystalPrefab;
        public GameObject cardPrefab;

        [SerializeField] float maxDistance = 1.0f;
        public float moveSpawnDuration = 0.25f;
        public float moveDuration = 1f;


        [Header("References")]
        [SerializeField] private RewardContainerData rewardContainerData;
        [SerializeField] private Transform rewardRoot;
        [SerializeField] private RewardContainer rewardContainerPrefab;
        public Transform rewardPanelRoot;

        [Header("Choice")]
        [SerializeField] private Transform choice2DCardSpawnRoot;
        public ChoiceCard choiceCardUIPrefab;
        [SerializeField] private ChoicePotion choicePotionUIPrefab;
        [SerializeField] private ChoicePanel choicePanel;


        [SerializeField] private Button skipButtonInRewardCanvas;
        [SerializeField] private Button skipButtonInChoicesPanel;

        private readonly List<RewardContainer> _currentRewardsList = new List<RewardContainer>();
        private readonly List<ChoiceCard> _spawnedCardChoiceList = new List<ChoiceCard>();
        private readonly List<ChoicePotion> _spawnedPotionChoiceList = new List<ChoicePotion>();
        private readonly List<CardData> _cardRewardList = new List<CardData>();
        private readonly List<PotionData> _potionRewardList = new List<PotionData>();

        public ChoicePanel ChoicePanel => choicePanel;
        
        #region Public Methods

        public void PrepareCanvas()
        {
            rewardPanelRoot.gameObject.SetActive(true);
            GameManager.ActivateButton(skipButtonInRewardCanvas);
            GameManager.ActivateButton(skipButtonInChoicesPanel);
        }
        public void BuildReward(RewardType rewardType)
        {
            var rewardClone = Instantiate(rewardContainerPrefab, rewardRoot);
            _currentRewardsList.Add(rewardClone);
            
            switch (rewardType)
            {
                case RewardType.Gold:
                    var rewardGold = rewardContainerData.GetRandomGoldReward(out var goldRewardData);
                    rewardClone.BuildReward(goldRewardData.RewardSprite, goldRewardData.RewardDescription);
                    rewardClone.RewardButton.onClick.AddListener(() => GetGoldReward(rewardClone, rewardGold));
                    break;
                case RewardType.ItemSellGold:
                    var sellItemGoldContainer = rewardContainerData.GetSellGoldReward(out var sellItemGoldData);
                    var sellItemGold = GameManager.salePrice;
                    rewardClone.BuildReward(sellItemGoldData.RewardSprite, sellItemGoldData.RewardDescription);
                    rewardClone.RewardButton.onClick.AddListener(()=>GetGoldReward(rewardClone, Mathf.RoundToInt(sellItemGold * GameManager.MoneyBonus)));
                    break;
                case RewardType.Card:
                    var rewardCardList = rewardContainerData.GetRandomCardRewardList(out var cardRewardData);
                    _cardRewardList.Clear();
                    foreach (var cardData in rewardCardList)
                        _cardRewardList.Add(cardData);
                    rewardClone.BuildReward(cardRewardData.RewardSprite,cardRewardData.RewardDescription);
                    rewardClone.RewardButton.onClick.AddListener(()=>GetCardReward(rewardClone,3));
                    break;
                case RewardType.Crystal:
                    var rewardCrystal = rewardContainerData.GetCrystalDropReward(out var crystalRewardData);
                    rewardClone.BuildReward(crystalRewardData.RewardSprite, crystalRewardData.RewardDescription);
                    rewardClone.RewardButton.onClick.AddListener(() => GetCrystalReward(rewardClone, rewardCrystal));
                    break;
                case RewardType.BossCrystal:
                    var rewardBossCrystal = rewardContainerData.GetBossCrystalDropReward(out var bossCrystalRewardData);
                    rewardClone.BuildReward(bossCrystalRewardData.RewardSprite, bossCrystalRewardData.RewardDescription);
                    rewardClone.RewardButton.onClick.AddListener(() => GetCrystalReward(rewardClone, rewardBossCrystal));
                    break;
                case RewardType.Relic:
                    break;
                case RewardType.Potion:
                    var rewardPotionList = rewardContainerData.GetRandomPotionRewardList(out var potionRewardData);
                    _potionRewardList.Clear();
                    foreach (var potionData in rewardPotionList)
                        _potionRewardList.Add(potionData);
                    rewardClone.BuildReward(potionRewardData.RewardSprite, potionRewardData.RewardDescription);
                    rewardClone.RewardButton.onClick.AddListener(() => GetPotionReward(rewardClone, 3));
                    break;
                case RewardType.Treasure:
                    var rewardTreasure = rewardContainerData.GetRandomCardRewardList(out var treasureRewardData);
                    _cardRewardList.Clear();
                    foreach (var cardData in rewardTreasure)
                        _cardRewardList.Add(cardData);
                    rewardClone.BuildReward(treasureRewardData.RewardSprite, treasureRewardData.RewardDescription);
                    rewardClone.RewardButton.onClick.AddListener(() => GetCardReward(rewardClone, 1));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rewardType), rewardType, null);
            }
        }
        
        public override void ResetCanvas()
        {
            ResetRewards();

            ResetChoice();
        }

        private void ResetRewards()
        {
            foreach (var rewardContainer in _currentRewardsList)
                Destroy(rewardContainer.gameObject);

            _currentRewardsList?.Clear();
        }

        private void ResetChoice()
        {
            foreach (var choice in _spawnedCardChoiceList)
            {
                Destroy(choice.gameObject);
            }

            foreach (var choice in _spawnedPotionChoiceList)
            {
                Destroy(choice.gameObject);
            }

            _spawnedCardChoiceList?.Clear();
            _spawnedPotionChoiceList?.Clear();
            ChoicePanel.DisablePanel();
        }

        #endregion
        
        #region Private Methods
        private void GetGoldReward(RewardContainer rewardContainer,int amount)
        {
            GameManager.PersistentGameplayData.CurrentGold += amount;
            _currentRewardsList.Remove(rewardContainer);
            UIManager.InformationCanvas.SetGoldText(GameManager.PersistentGameplayData.CurrentGold);
            Destroy(rewardContainer.gameObject);

            //_currentRewardsList.Remove(rewardContainer);

            //for (int i = 0; i < amount; i++)
            //{
            //    // Instantiate the object at the position of the spawner (this.transform.position)
            //    GameObject newObject = Instantiate(coinPrefab, startPos.position, Quaternion.identity);

            //    newObject.transform.SetParent(startPos);

            //    // Generate a random direction and distance
            //    Vector3 randomDirection = Random.insideUnitSphere.normalized;
            //    float randomDistance = Random.Range(0, maxDistance);

            //    // Calculate the new position
            //    Vector3 newPosition = startPos.position + randomDirection * randomDistance;

            //    // Set the object's position to the new position
            //    StartCoroutine(MoveObject(newObject, newPosition, goldEndPos, 0.5f));

            //    GameManager.PersistentGameplayData.CurrentGold += 1;

            //    UIManager.InformationCanvas.SetGoldText(GameManager.PersistentGameplayData.CurrentGold);
            //} 
            //Destroy(rewardContainer.gameObject);
        }

        private void GetCrystalReward(RewardContainer rewardContainer, int amount)
        {
            GameManager.PersistentGameplayData.CurrentCrystal += amount;
            _currentRewardsList.Remove(rewardContainer);
            UIManager.InformationCanvas.SetCrystalText(GameManager.PersistentGameplayData.CurrentCrystal);
            Destroy(rewardContainer.gameObject);

            //_currentRewardsList.Remove(rewardContainer);

            //for (int i = 0; i < amount; i++)
            //{
            //    // Instantiate the object at the position of the spawner (this.transform.position)
            //    GameObject newObject = Instantiate(crystalPrefab, startPos.position, Quaternion.identity);

            //    // Generate a random direction and distance
            //    Vector3 randomDirection = Random.insideUnitSphere.normalized;
            //    float randomDistance = Random.Range(0, maxDistance);

            //    // Calculate the new position
            //    Vector3 newPosition = startPos.position + randomDirection * randomDistance;

            //    // Set the object's position to the new position
            //    StartCoroutine(MoveObject(newObject, newPosition, crystalEndPos, 0.5f));
            //}
            //Destroy(rewardContainer.gameObject);
        }

        private void GetCardReward(RewardContainer rewardContainer,int amount = 3)
        {
            ChoicePanel.gameObject.SetActive(true);
            
            for (int i = 0; i < amount; i++)
            {
                Transform spawnTransform = choice2DCardSpawnRoot;
              
                var choice = Instantiate(choiceCardUIPrefab, spawnTransform);
                var reward = _cardRewardList.RandomItem();
                choice.BuildReward(reward);
                choice.OnCardChose += ResetChoice;

                _cardRewardList.Remove(reward);
                _spawnedCardChoiceList.Add(choice);
                _currentRewardsList.Remove(rewardContainer);
            }

            Destroy(rewardContainer.gameObject);
        }



        private void GetPotionReward(RewardContainer rewardContainer, int amount = 3)
        {
            ChoicePanel.gameObject.SetActive(true);

            for (int i = 0; i < amount; i++)
            {
                Transform spawnTransform = choice2DCardSpawnRoot;

                var choice = Instantiate(choicePotionUIPrefab, spawnTransform);

                var reward = _potionRewardList.RandomItem();
                choice.BuildReward(reward);
                choice.OnPotionChose += ResetChoice;

                _potionRewardList.Remove(reward);
                _spawnedPotionChoiceList.Add(choice);
                _currentRewardsList.Remove(rewardContainer);

            }

            Destroy(rewardContainer.gameObject);
        }

        public void DeactivateObject(GameObject window)
        {
            window.SetActive(false);
        }

        #endregion

        public void NextStage()
        {
            GameManager.NextStage();
        }

        private IEnumerator MoveObject(GameObject obj, Vector2 targetPosition, Transform endPos, float duration)
        {
            yield return new WaitForSeconds(Random.Range(0f, duration));

            Vector2 startPosition = obj.transform.position;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                obj.transform.position = Vector2.Lerp(startPosition, targetPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            obj.transform.position = targetPosition;

            if ((Vector2)obj.transform.position == (Vector2)endPos.position)
            {
                GameManager.PersistentGameplayData.CurrentCrystal += 1;

                UIManager.InformationCanvas.SetCrystalText(GameManager.PersistentGameplayData.CurrentCrystal);

                Destroy(obj);
            }

            yield return new WaitForSeconds(1f);

            yield return MoveObject(obj, endPos.position, null, 0.5f);
        }
    }
}