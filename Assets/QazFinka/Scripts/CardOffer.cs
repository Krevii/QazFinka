using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardOffer : MonoBehaviour, ICard
{
    public Player player;
    public JsonData jsonData;
    private static Canvas _canvas;
    public GameObject cardOfferPrefab;
    private GameObject cardOffer;
    private GameObject backGround;
    private GameObject title;
    private GameObject description;
    private GameObject okBtn;

    public void DoAction()
    {
        Item item = jsonData.GetRandomItem("OfferData");
        _canvas = UiManager.GetCanvas();
        cardOffer = Instantiate(cardOfferPrefab, _canvas.transform);

        player = FindAnyObjectByType<TileMapObject>().currentPlayer.GetComponent<Player>();
        
        backGround = cardOffer.transform.Find("Background").gameObject;
        title = backGround.transform.Find("Title").gameObject;
        description = backGround.transform.Find("TextDscription").gameObject;
        okBtn = backGround.transform.Find("OkButton").gameObject;
        
        backGround.transform.Find("assetID").GetComponent<TextMeshProUGUI>().text = item.assetID.ToString();
        backGround.transform.Find("TextCost").GetComponent<TextMeshProUGUI>().text = "÷≈Õ¿:" + item.cost.ToString("N0") + "“√"; ;
        description.GetComponent<TextMeshProUGUI>().text = item.description;
        
        okBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        okBtn.GetComponent<Button>().onClick.AddListener(() => {
            Destroy(cardOffer);
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        jsonData = GameObject.FindAnyObjectByType<JsonData>();
    }
}
