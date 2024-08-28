using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDeal : MonoBehaviour, ICard
{

    public Player player;
    public JsonData jsonData;

    public GameObject cardDealPrefab;
    private GameObject cardDeal;

    private CashBook book;
    private GameObject background;
    private GameObject bLeft;
    private GameObject bRight;
    private GameObject bClose;
    private GameObject title;
    private GameObject description;
    private GameObject cost;
    private GameObject cashFlow;
    private GameObject downPayOrsharedOwned;

    private static Canvas _canvas;

    public void DoAction()
    {
        player = FindAnyObjectByType<TileMapObject>().currentPlayer.GetComponent<Player>();

        _canvas = UiManager.GetCanvas();
        cardDeal = Instantiate(cardDealPrefab, _canvas.transform);

        background = cardDeal.transform.Find("Background").gameObject;

        bLeft = background.transform.Find("LeftButton").gameObject;
        bRight = background.transform.Find("RightButton").gameObject;
        bClose = background.transform.Find("CloseButton").gameObject;
        title = background.transform.Find("Title").gameObject;
        description = background.transform.Find("TextDscription").gameObject;
        cost = background.transform.Find("TextCost").gameObject;
        cashFlow = background.transform.Find("CashFlowText").gameObject;
        downPayOrsharedOwned = background.transform.Find("OwnedOrDownPayText").gameObject;

        title.GetComponent<Text>().text = "—ƒ≈À ¿";
        description.GetComponent<TextMeshProUGUI>().text = "¬€¡≈–»“≈ “»œ —ƒ≈À »";

        cost.SetActive(false);
        cashFlow.SetActive(false);
        downPayOrsharedOwned.SetActive(false);

        bLeft.GetComponent<Button>().onClick.RemoveAllListeners();
        bRight.GetComponent<Button>().onClick.RemoveAllListeners();

        bClose.GetComponent<Button>().onClick.AddListener(() => { Destroy(cardDeal); });
        bRight.GetComponent<Button>().onClick.AddListener(() => { Invoke("BigDeal", 0f); });
        bLeft.GetComponent<Button>().onClick.AddListener(() => { Invoke("SmallDeal", 0f); });
    }

    // Start is called before the first frame update
    void Start()
    {
        jsonData = GameObject.FindAnyObjectByType<JsonData>();
        book = GameObject.FindFirstObjectByType<CashBook>();
    }

    public void BigDeal()
    {
        Item item = jsonData.GetRandomItem("BigDealData");
        int downPay = item.cost / 100 * 15;
        title.GetComponent<Text>().text = "¡ŒÀ‹ÿ¿ﬂ —ƒ≈À ¿";
        description.GetComponent<TextMeshProUGUI>().text = item.description;
        cost.GetComponent<TextMeshProUGUI>().text = "÷≈Õ¿:" + item.cost.ToString("N0") + "“√";
        cashFlow.GetComponent<TextMeshProUGUI>().text = "œ¿——»¬Õ€… ƒŒ’Œƒ:" + item.cashFlow.ToString("N0") + "“√";
        downPayOrsharedOwned.GetComponent<TextMeshProUGUI>().text = "œ≈–¬ŒÕ¿◊¿À‹Õ€… ¬«ÕŒ—:" + (downPay).ToString("N0") + "“√";

        cost.SetActive(true);
        cashFlow.SetActive(true);
        downPayOrsharedOwned.SetActive(true);

        bLeft.GetComponent<Button>().onClick.RemoveAllListeners();
        bRight.GetComponent<Button>().onClick.RemoveAllListeners();
        bRight.SetActive(false);

        //bRight.GetComponent<Button>().onClick.AddListener(() => { Destroy(cardDeal); });
        //bRight.GetComponentInChildren<Text>().text = "PASS";

        bLeft.gameObject.transform.localPosition = new Vector3(0, bLeft.gameObject.transform.localPosition.y);

        bLeft.GetComponent<Button>().onClick.AddListener(() =>
        {
            player.AddLiablities(item.cost, item.description, downPay);
            player.AddAssets(downPay, item.description, item.cashFlow, item.assetID);
        });
        bLeft.GetComponentInChildren<Text>().text = " ”œ»“‹";
    }

    public void SmallDeal()
    {
        Item item = jsonData.GetRandomItem("SmallDealData");

        title.GetComponent<Text>().text = "Ã¿À¿ﬂ —ƒ≈À ¿";
        description.GetComponent<TextMeshProUGUI>().text = item.description;
        cost.GetComponent<TextMeshProUGUI>().text = "÷≈Õ¿:" + item.cost.ToString("N0") + "“√";
        cashFlow.GetComponent<TextMeshProUGUI>().text = "œ¿——»¬Õ€… ƒŒ’Œƒ:" + item.cashFlow.ToString("N0") + "“√";

        cost.SetActive(true);
        cashFlow.SetActive(true);
        //downPayOrsharedOwned.SetActive(true);

        bLeft.GetComponent<Button>().onClick.RemoveAllListeners();
        bRight.GetComponent<Button>().onClick.RemoveAllListeners();

        bRight.SetActive(true);
        bRight.GetComponentInChildren<Text>().text = "œ–Œƒ¿“‹";

        GameObject InputFieldComponent = cardDeal.transform.Find("Background").transform.Find("InputField").gameObject;
        InputFieldComponent.SetActive(true);

        bRight.GetComponent<Button>().onClick.AddListener(() =>
        {

            string countText = InputFieldComponent.GetComponent<TMP_InputField>().textComponent.text;
            int cardAssetCount = 1;

            if (countText.Length > 0)
            {
                cardAssetCount = int.Parse(countText.Remove(countText.Length - 1));
            }

            BookViewItem[] assetsItems = book.AssetsView.GetComponentsInChildren<BookViewItem>();

            foreach (var lsitItem in assetsItems)
            {
                if (lsitItem.assetID != item.assetID) continue;

                

                if ((lsitItem.count - cardAssetCount) <= 0)
                {
                    int maxHowMuchSell = (int)(lsitItem.count - cardAssetCount) + cardAssetCount;

                    lsitItem.count -= maxHowMuchSell;

                    cardAssetCount -= maxHowMuchSell;

                    Debug.Log(maxHowMuchSell);
                    player.GiveCash(item.cost * maxHowMuchSell);

                    lsitItem.DeleteItemAssets();


                }
                else
                {
                    lsitItem.count -= cardAssetCount;
                    player.GiveCash(item.cost * cardAssetCount);

                    string[] values = lsitItem.price.text.Split("X");
                    values[1] = $"X{lsitItem.count}";

                    lsitItem.price.text = string.Join("", values);
                    return;
                }

            }
        });


        bLeft.GetComponent<Button>().onClick.AddListener(() =>
        {

            string countText = InputFieldComponent.GetComponent<TMP_InputField>().textComponent.text;
            int cardAssetCount = 1;

            if (countText.Length > 0)
            {
                cardAssetCount = int.Parse(countText.Remove(countText.Length - 1));
            }
            player.AddAssets(item.cost, item.description, item.cashFlow, item.assetID, cardAssetCount);

        });
        bLeft.GetComponentInChildren<Text>().text = " ”œ»“‹";
    }

}
