using System.Collections;
using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Card;
using NueGames.NueDeck.Scripts.Collection;
using NueGames.NueDeck.Scripts.Data.Collection;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Managers
{
    public class TutorialCollectionManager : MonoBehaviour
    {
        public TutorialCollectionManager(){}
      
        public static TutorialCollectionManager Instance { get; private set; }

        [Header("Controllers")] 
        [SerializeField] private HandController handController;

        public GameObject cardPopUp;

        #region Cache

        public List<CardData> DrawPile { get; private set; } = new List<CardData>();
        public List<CardData> HandPile { get; private set; } = new List<CardData>();
        public List<CardData> DiscardPile { get; private set; } = new List<CardData>();
        public List<PotionData> PotionBag { get; private set; } = new List<PotionData>();

        public List<CardData> ExhaustPile { get; private set; } = new List<CardData>();
        public HandController HandController => handController;
        protected FxManager FxManager => FxManager.Instance;
        protected AudioManager AudioManager => AudioManager.Instance;
        protected GameManager GameManager => GameManager.Instance;
        protected TutorialCombatManager CombatManager => TutorialCombatManager.Instance;

        protected UIManager UIManager => UIManager.Instance;

        #endregion
       
        #region Setup
        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                Instance = this;
            }
        }

        #endregion

        #region Public Methods
        public void DrawCards(int targetDrawCount)
        {
            var currentDrawCount = 0;

            for (var i = 0; i < targetDrawCount; i++)
            {
                if (GameManager.GameplayData.MaxCardOnHand < HandPile.Count)
                {
                    return;
                }
                    
                if (DrawPile.Count <= 0)
                {
                    var nDrawCount = targetDrawCount - currentDrawCount;
                    
                    if (nDrawCount >= DiscardPile.Count) 
                        nDrawCount = DiscardPile.Count;
                    
                    ReshuffleDiscardPile();
                    DrawCards(nDrawCount);
                    break;
                }

                var drawnCard = DrawPile[0];

                if (drawnCard.CardName == "Closing Card" && UIManager.CombatCanvas.SuccessPercentage.value >= .5f && UIManager.TutorialCanvas.closingCaseActive == true)
                    UIManager.TutorialCanvas.ClosingCase();

                var clone = GameManager.BuildAndGetCard(drawnCard, HandController.drawTransform);
                HandController.AddCardToHand(clone);
                HandPile.Add(drawnCard);
                DrawPile.RemoveAt(0);
                currentDrawCount++;
                UIManager.CombatCanvas.SetPileTexts();
            }
            
            foreach (var cardObject in HandController.hand)
                cardObject.UpdateCardText();
        }
        public void DiscardHand()
        {
            foreach (var cardBase in HandController.hand) 
                cardBase.Discard();
            
            HandController.hand.Clear();
        }
        
        public void OnCardDiscarded(CardBase targetCard)
        {
            HandPile.Remove(targetCard.CardData);
            DiscardPile.Add(targetCard.CardData);
            UIManager.CombatCanvas.SetPileTexts();
        }
        
        public void OnCardExhausted(CardBase targetCard)
        {
            HandPile.Remove(targetCard.CardData);
            ExhaustPile.Add(targetCard.CardData);
            UIManager.CombatCanvas.SetPileTexts();
        }
        public void OnCardPlayed(CardBase targetCard)
        {
            if (targetCard.CardData.ExhaustAfterPlay)
                targetCard.Exhaust();
            else
                targetCard.Discard();
          
            foreach (var cardObject in HandController.hand)
                cardObject.UpdateCardText();
        }
        public void SetGameDeck()
        {
            ClearPiles();

            foreach (var i in GameManager.PersistentGameplayData.CurrentCardsList) 
                DrawPile.Add(i);
        }

        public void SetPotionPack()
        {
            PotionBag.Clear();

            foreach (var i in GameManager.PersistentGameplayData.CurrentPotionsList)
                PotionBag.Add(i);
        }

        public void ClearPiles()
        {
            DiscardPile.Clear();
            DrawPile.Clear();
            HandPile.Clear();
            ExhaustPile.Clear();
            HandController.hand.Clear();
        }
        #endregion

        #region Private Methods
        private void ReshuffleDiscardPile()
        {
            foreach (var i in DiscardPile) 
                DrawPile.Add(i);
            
            DiscardPile.Clear();
        }
        private void ReshuffleDrawPile()
        {
            foreach (var i in DrawPile) 
                DiscardPile.Add(i);
            
            DrawPile.Clear();
        }
        #endregion

    }
}