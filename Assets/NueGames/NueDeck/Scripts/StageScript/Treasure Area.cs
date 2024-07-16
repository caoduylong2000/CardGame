using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NueGames.NueDeck.Scripts.Managers;
using NueGames.NueDeck.Scripts.Enums;

public class OpenChest : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public Animator Animator => animator;
    private UIManager UIManager => UIManager.Instance;
    private GameManager GameManager => GameManager.Instance;

    public void Open()
    {
        StartCoroutine(OpenTreasure());
    }

    private IEnumerator OpenTreasure()
    {
        this.GetComponent<Button>().interactable = false;
        Animator.SetBool("IsOpen", true);
        yield return new WaitForSeconds(3f);
        //complete Stage

        //Reward Canvas
        UIManager.RewardCanvas.gameObject.SetActive(true);
        UIManager.RewardCanvas.PrepareCanvas();
        UIManager.RewardCanvas.BuildReward(RewardType.Treasure);
    }
}
