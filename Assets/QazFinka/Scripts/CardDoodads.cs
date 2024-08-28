using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardDoodads : MonoBehaviour, ICard
{
    public JsonData jsonData;
    public GameObject cardDoodadsPrefab;
    private GameObject cardDoodads;
    public Player player;

    private GameObject background;
    private GameObject title;
    private GameObject description;
    private GameObject textpay;
    private GameObject bOk;

    private static Canvas _canvas;

    public void DoAction()
    {
        player = FindAnyObjectByType<TileMapObject>().currentPlayer.GetComponent<Player>();
        Item item = jsonData.GetRandomItem("DoodadsData");
        _canvas = UiManager.GetCanvas();
        cardDoodads = Instantiate(cardDoodadsPrefab, _canvas.transform);

        background = cardDoodads.transform.Find("Background").gameObject;

        title = background.transform.Find("Title").gameObject;
        description = background.transform.Find("TextDescription").gameObject;
        textpay = background.transform.Find("TextPay").gameObject;
        bOk = background.transform.Find("OkButton").gameObject;

        bOk.GetComponent<Button>().onClick.AddListener(() => { player.TakeCash(item.pay); Destroy(cardDoodads); });


        description.GetComponent<TextMeshProUGUI>().text = item.description;
        textpay.GetComponent<TextMeshProUGUI>().text = "Затраты:" + item.pay.ToString("N0") +"ТГ.";
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
