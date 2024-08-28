using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDream : MonoBehaviour, ICard
{
    public JsonData jsonData;
    public GameObject cardDreamPrefab;
    private GameObject cardDream;
    public Player player;

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
        Item item = jsonData.GetRandomItem("DreamData");
        player = FindAnyObjectByType<TileMapObject>().currentPlayer.GetComponent<Player>();

        _canvas = UiManager.GetCanvas();
        cardDream = Instantiate(cardDreamPrefab, _canvas.transform);

        background = cardDream.transform.Find("Background").gameObject;

        bLeft = background.transform.Find("LeftButton").gameObject;
        bRight = background.transform.Find("RightButton").gameObject;
        bClose = background.transform.Find("CloseButton").gameObject;
        title = background.transform.Find("Title").gameObject;
        description = background.transform.Find("TextDscription").gameObject;
        cost = background.transform.Find("TextCost").gameObject;
        cashFlow = background.transform.Find("CashFlowText").gameObject;
        downPayOrsharedOwned = background.transform.Find("OwnedOrDownPayText").gameObject;

        title.GetComponent<Text>().text = "Ã≈◊“¿";
        description.GetComponent<TextMeshProUGUI>().text = "œÓÍÛÔÍ‡ ÏÂ˜Ú˚";

        cost.SetActive(true);
        cashFlow.SetActive(false);
        downPayOrsharedOwned.SetActive(true);

        description.GetComponent<TextMeshProUGUI>().text = item.description;
        cost.GetComponent<TextMeshProUGUI>().text = "÷≈Õ¿:" + item.cost.ToString("N0") + "“√";
        downPayOrsharedOwned.GetComponent<TextMeshProUGUI>().text = "AssetID:" + item.assetID;

        bLeft.GetComponent<Button>().onClick.RemoveAllListeners();
        bRight.GetComponent<Button>().onClick.RemoveAllListeners();

        bClose.GetComponent<Button>().onClick.AddListener(() => { Destroy(cardDream); });
        bLeft.GetComponent<Button>().onClick.AddListener(() => {
            player.AddAssets(item.cost, item.description, item.cashFlow, item.assetID);
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        jsonData = GameObject.FindAnyObjectByType<JsonData>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
