using System;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NueGames.NueDeck.Scripts.Potion
{
    public class ChoicePotion : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler
    {
        [SerializeField] private float showScaleRate = 1.15f;
        private PotionBase _potionBase;
        private Vector3 _initalScale;
        public Action OnPotionChose;
        public GameManager GameManager => GameManager.Instance;
        public UIManager UIManager => UIManager.Instance;

        public void BuildReward(PotionData potionData)
        {
            _potionBase = GetComponent<PotionBase>();
            _initalScale = transform.localScale;
            _potionBase.SetPotion(potionData);
            _potionBase.UpdatePotionText();
        }


        private void OnChoice()
        {
            if (GameManager != null)
                GameManager.PersistentGameplayData.CurrentPotionsList.Add(_potionBase.PotionData);

            if (UIManager != null)
                UIManager.RewardCanvas.ChoicePanel.DisablePanel();
            OnPotionChose?.Invoke();
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.localScale = _initalScale * showScaleRate;
        }

        public void OnPointerDown(PointerEventData eventData)
        {

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.localScale = _initalScale;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnChoice();

        }
    }
}
