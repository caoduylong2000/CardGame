using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using NueGames.NueDeck.Scripts.Managers;
using NueGames.NueDeck.Scripts.Utils;

public class RestArea : MonoBehaviour
{
    protected GameManager GameManager => GameManager.Instance;
    protected UIManager UIManager => UIManager.Instance;

    [SerializeField] private Button HealButton;

    public void Heal(int HpCount)
    {
        StartCoroutine(HealInRest(HpCount));
    }

    private IEnumerator HealInRest(int HpCount)
    {
        if (GameManager.PersistentGameplayData.AllyHealthDataList.Count > 0)
        {

            //Heal
            for (int i = 0; i < HpCount; i++)
            {

                var data = GameManager.PersistentGameplayData.AllyHealthDataList[0];

                data.MaxHealth = (int)(GameManager.PersistentGameplayData.AllyList[0].AllyCharacterData.MaxHealth * GameManager.PlayerHP);

                data.CurrentHealth++;

                if (data.CurrentHealth > data.MaxHealth)
                {
                    data.CurrentHealth = data.MaxHealth;
                }


                UIManager.InformationCanvas.UpdateHealStats(data.CurrentHealth, data.MaxHealth);
                yield return new WaitForSeconds(.1f);
            }
        }
        else
        {
            for (int i = 0; i < HpCount; i++)
            {
                GameManager.playerMaxHealth = (int)(GameManager.PersistentGameplayData.AllyList[0].AllyCharacterData.MaxHealth * GameManager.PlayerHP);

                GameManager.playerCurrentHealth++;

                if (GameManager.playerCurrentHealth > GameManager.playerMaxHealth)
                {
                    GameManager.playerCurrentHealth = GameManager.playerMaxHealth;
                }


                UIManager.InformationCanvas.UpdateHealStats(GameManager.playerCurrentHealth, GameManager.playerMaxHealth);
                yield return new WaitForSeconds(.1f);
            }
        }

        HealButton.interactable = false;
    }

    public void NextStage()
    {
        GameManager.NextStage();
    }
}
