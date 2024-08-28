using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardChild : MonoBehaviour, ICard
{

    public JsonData jsonData;
    public GameObject cardChildPrefab;
    private GameObject cardChild;
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

        Item item = jsonData.GetRandomItem("ChildData");
        player = FindAnyObjectByType<TileMapObject>().currentPlayer.GetComponent<Player>();

        _canvas = UiManager.GetCanvas();
        cardChild = Instantiate(cardChildPrefab, _canvas.transform);

        background = cardChild.transform.Find("Background").gameObject;

        bLeft = background.transform.Find("LeftButton").gameObject;
        bRight = background.transform.Find("RightButton").gameObject;
        bClose = background.transform.Find("CloseButton").gameObject;
        title = background.transform.Find("Title").gameObject;
        description = background.transform.Find("TextDscription").gameObject;
        cost = background.transform.Find("TextCost").gameObject;
        cashFlow = background.transform.Find("CashFlowText").gameObject;
        downPayOrsharedOwned = background.transform.Find("OwnedOrDownPayText").gameObject;

        title.GetComponent<Text>().text = "ДЕТИ";
        description.GetComponent<TextMeshProUGUI>().text = "Пополнение в семье";

        cost.SetActive(false);
        cashFlow.SetActive(false);
        downPayOrsharedOwned.SetActive(false);

        bLeft.GetComponent<Button>().onClick.RemoveAllListeners();
        bRight.GetComponent<Button>().onClick.RemoveAllListeners();

        bClose.GetComponent<Button>().onClick.AddListener(() =>
        {
            Destroy(cardChild);
        });
        bRight.GetComponent<Button>().onClick.AddListener(() =>
        {
            player.AddExpenses(item.cost, item.description);
        });
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
