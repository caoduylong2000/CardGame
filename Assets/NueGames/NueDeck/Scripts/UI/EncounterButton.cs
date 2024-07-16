using System;
using System.Collections;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace NueGames.NueDeck.Scripts.UI
{
    public class EncounterButton : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Button button;
        [SerializeField] private Image completedImage;
        public Image backImage;
        public float progressValue;

        [SerializeField] private Image mainImage;
        [SerializeField] private bool isFinal;
        public bool isFilpped;

        protected GameManager GameManager => GameManager.Instance;

        private void Awake()
        {
            completedImage.gameObject.SetActive(false);
            backImage.gameObject.SetActive(true);

            Material dissolveMaterial = new Material(backImage.material);
            backImage.material = dissolveMaterial;
        }

        private void Update()
        {
            if(isFilpped == true && backImage.gameObject.activeSelf == true)
            {
                StartCoroutine(FlipCard(backImage.material, backImage.gameObject, .75f));
            }

            CheckDeactivateButton();
        }

        private void CheckDeactivateButton()
        {
            if (gameObject.GetComponentInChildren<EncounterButton>().GetStatus() == EncounterButtonStatus.Passive
                || gameObject.GetComponentInChildren<EncounterButton>().GetStatus() == EncounterButtonStatus.Completed)
            {
                GetComponent<Button>().interactable = false;
            }
        }

        public void SetStatus(EncounterButtonStatus targetStatus)
        {
            switch (targetStatus)
            {
                case EncounterButtonStatus.Active:
                    button.interactable = true;
                    backImage.gameObject.SetActive(true);
                    backImage.material.SetFloat("_Progress", 1);
                    mainImage.gameObject.SetActive(true);
                    completedImage.gameObject.SetActive(false);

                    if (isFinal) GameManager.PersistentGameplayData.IsFinalEncounter = true;
                    break;
                case EncounterButtonStatus.Passive:
                    button.interactable = false;
                    backImage.gameObject.SetActive(true);
                    backImage.material.SetFloat("_Progress", 1);
                    mainImage.gameObject.SetActive(false);
                    break;
                case EncounterButtonStatus.Completed:
                    button.interactable = false;
                    backImage.gameObject.SetActive(true);
                    backImage.material.SetFloat("_Progress", 1);
                    mainImage.gameObject.SetActive(false);
                    completedImage.gameObject.SetActive(true);
                    break;
                case EncounterButtonStatus.Opened:
                    button.interactable = false;
                    backImage.gameObject.SetActive(false);
                    mainImage.gameObject.SetActive(true);
                    completedImage.gameObject.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetStatus), targetStatus, null);
            }
        }

        public EncounterButtonStatus GetStatus()
        {
            if (button.interactable)
            {
                if (isFinal && GameManager.PersistentGameplayData.IsFinalEncounter)
                {
                    return EncounterButtonStatus.Completed;
                }
                else
                {
                    return EncounterButtonStatus.Active;
                }
            }
            else if (!button.interactable && !completedImage.gameObject.activeSelf)
            {
                return EncounterButtonStatus.Passive;
            }
            else if (!button.interactable && completedImage.gameObject.activeSelf)
            {
                return EncounterButtonStatus.Completed;
            }
            else
            {
                throw new InvalidOperationException("Unhandled status");
            }
        }

        IEnumerator FlipCard(Material material, GameObject backImage, float duration)
        {
            float startValue = 1.0f;
            float endValue = 0.0f;
            float elapsedTime = 0.0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float value = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
                material.SetFloat("_Progress", value);
                yield return null;
            }

            // Đảm bảo giá trị cuối cùng là endValue
            material.SetFloat("_Progress", endValue);
            backImage.SetActive(false);
        }
    }
}