using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NueGames.NueDeck.Scripts.Managers;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Card;
using NueGames.NueDeck.Scripts.Potion;

public class MerchantArea : MonoBehaviour
{
    protected GameManager GameManager => GameManager.Instance;
    protected UIManager UIManager => UIManager.Instance;

    private delegate void CardButtonClickDelegate(Card3D clickedCard);
    private static event CardButtonClickDelegate OnCardButtonClick;

    private delegate void PotionButtonClickDelegate(PotionItem clickedPotion);
    private static event PotionButtonClickDelegate OnPotionButtonClick;

    [Header("Item Area")]
    [SerializeField] private GameObject cardArea;
    [SerializeField] private GameObject potionArea;
    [SerializeField] private GameObject relicArea;

    public GameObject CardArea => cardArea;
    public GameObject PotionArea => potionArea;
    public GameObject RelicArea => relicArea;

    private int MaxCard = 6;
    private int MaxPotion = 4;

    [Header("Card Pool")]
    public CardPool CardPool;
    public CardBase CardItem;

    [Header("Potion Pool")]
    public PotionPool PotionPool;
    public PotionBase PotionItem;

    [Header("BuyConfirm")]
    [SerializeField] private GameObject buyComfirm;
    [SerializeField] private Text itemName;
    [SerializeField] private Text itemDesc;
    [SerializeField] private Transform itemImage;
    [SerializeField] private Text priceText;
    [SerializeField] private int price;
    [SerializeField] private Button buyButton;

    //[Header("Inventory")]
    //[SerializeField] private GameObject inventory;

    #region Setup
    private void Awake()
    {
        GetCard();
        GetPotion();
    }

    private void Start()
    {
        Button[] cardButtons = CardArea.GetComponentsInChildren<Button>();
        Card3D[] cards = GetComponentsInChildren<Card3D>();

        Button[] potionButtons = PotionArea.GetComponentsInChildren<Button>();
        PotionItem[] potions = GetComponentsInChildren<PotionItem>();

        if (cardButtons.Length > 0)
        {
            foreach (Card3D card in cards)
            {
                card.gameObject.GetComponent<Button>().onClick.AddListener(() => OpenMessage(card.gameObject));
            }
        }
        else
        {
            Debug.LogError("No Button components found!");
        }

        if (potionButtons.Length > 0)
        {
            foreach (PotionItem potion in potions)
            {
                potion.gameObject.GetComponent<Button>().onClick.AddListener(() => OpenMessage(potion.gameObject));
            }
        }
        else
        {
            Debug.LogError("No Button components found!");
        }
    }
    #endregion
    #region Merchant
    private void GetCard()
    {
        for (int i = 0; i < MaxCard; i++)
        {
            var cardNumber = Random.Range(0, CardPool.CardPoolObject.Count);
            BuildCard(CardPool.CardPoolObject[cardNumber], CardArea.transform);
        }
    }

    private void GetPotion()
    {
        for (int i = 0; i < MaxPotion; i++)
        {
            var potionNumber = Random.Range(0, PotionPool.PotionPoolObject.Count);
            BuildPotion(PotionPool.PotionPoolObject[potionNumber], potionArea.transform);
        }
    }

    private CardBase BuildCard(CardData targetData, Transform parent)
    {
        var clone = Instantiate(CardItem, parent);
        clone.SetCard(targetData);
        clone.UpdateCardText();
        return clone;
    }

    private PotionBase BuildPotion(PotionData targetData, Transform parent)
    {
        var clone = Instantiate(PotionItem, parent);
        clone.SetPotion(targetData);
        clone.UpdatePotionText();
        return clone;
    }



    public void OpenMessage(GameObject childObject)
    {
        //card

        if (childObject.GetComponent<Card3D>())
        {
            buyComfirm.SetActive(true);

            itemDesc.text = childObject.GetComponent<Card3D>().CardData.MyDescription;
            itemName.text = childObject.GetComponent<Card3D>().CardData.CardName;

            if (itemImage.childCount > 0)
            {
                for (int i = 0; i < itemImage.childCount; i++)
                {
                    GameObject imageObject = itemImage.GetChild(i).gameObject;
                    Destroy(imageObject);
                }
            }

            var clone = Instantiate(CardItem, itemImage);
            clone.GetComponent<Card3D>().PriceObject.SetActive(false);
            clone.SetCard(childObject.GetComponent<Card3D>().CardData);

            priceText.text = childObject.GetComponent<Card3D>().CardPrice.ToString();

            if (GameManager.PersistentGameplayData.CurrentGold < childObject.GetComponent<Card3D>().CardPrice)
            {
                buyButton.interactable = false;
            }

            buyButton.onClick.RemoveAllListeners(); // Remove all previous listeners
            buyButton.onClick.AddListener(() => BuyCard(childObject.GetComponent<Card3D>()));

            OnCardButtonClick?.Invoke(childObject.GetComponent<Card3D>());

        }
        else if (childObject.GetComponent<PotionItem>())
        {
            buyComfirm.SetActive(true);

            itemDesc.text = childObject.GetComponent<PotionItem>().PotionData.MyDescription;
            itemName.text = childObject.GetComponent<PotionItem>().PotionData.PotionName;

            if (itemImage.childCount > 0)
            {
                for (int i = 0; i < itemImage.childCount; i++)
                {
                    GameObject imageObject = itemImage.GetChild(i).gameObject;
                    Destroy(imageObject);
                }
            }

            var clone = Instantiate(PotionItem, itemImage);
            clone.GetComponent<PotionItem>().PriceObject.SetActive(false);
            clone.SetPotion(childObject.GetComponent<PotionItem>().PotionData);

            priceText.text = childObject.GetComponent<PotionItem>().PotionPrice.ToString();

            if (GameManager.PersistentGameplayData.CurrentGold < childObject.GetComponent<PotionItem>().PotionPrice)
            {
                buyButton.interactable = false; 
            }

            buyButton.onClick.RemoveAllListeners(); // Remove all previous listeners
            buyButton.onClick.AddListener(() => BuyPotion(childObject.GetComponent<PotionItem>()));

            OnPotionButtonClick?.Invoke(childObject.GetComponent<PotionItem>());
        }
        else return;
    }

    public void CloseMessage()
    {
        buyComfirm.SetActive(false);

        if (itemImage.childCount > 0)
        {
            for (int i = 0; i < itemImage.childCount; i++)
            {
                GameObject childObject = itemImage.GetChild(i).gameObject;
                Destroy(childObject);
            }
        }
    }

    public void BuyCard(Card3D selectedCard)
    {
        GameManager.PersistentGameplayData.CurrentGold -= selectedCard.CardPrice;

        GameManager.PersistentGameplayData.CurrentCardsList.Add(selectedCard.CardData);

        buyComfirm.SetActive(false);

        UIManager.InformationCanvas.SetGoldText(GameManager.PersistentGameplayData.CurrentGold);
        selectedCard.IsPurchase = true;
        selectedCard.GetComponent<Button>().interactable = false;
        selectedCard.CheckPurchase();
    }

    public void BuyPotion(PotionItem selectedPotion)
    {
        GameManager.PersistentGameplayData.CurrentGold -= selectedPotion.PotionPrice;

        GameManager.PersistentGameplayData.CurrentPotionsList.Add(selectedPotion.PotionData);

        buyComfirm.SetActive(false);


        UIManager.InformationCanvas.SetGoldText(GameManager.PersistentGameplayData.CurrentGold);
        selectedPotion.IsPurchase = true;
        selectedPotion.GetComponent<Button>().interactable = false;
        selectedPotion.CheckPurchase();
    }

    public void DeactivateButton(Button button)
    {
        button.interactable = false;
    }

    public void NextStage()
    {
        GameManager.NextStage();
    }

    #endregion

    //#region Inventory
    //public void OpenInventory()
    //{
    //    inventory.SetActive(true);
    //}

    //public void CloseInventory()
    //{
    //    inventory.SetActive(false);
    //}
    //#endregion
}
