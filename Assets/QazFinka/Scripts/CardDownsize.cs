using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardDownsize : MonoBehaviour, ICard
{
    public JsonData jsonData;
    public Player player;
    public GameObject cardDownsizePrefab;
    private GameObject cardDownsize;
    private static Canvas _canvas;
    private GameObject background;
    private GameObject okBtn;

    public void DoAction()
    {
        Item item = jsonData.GetRandomItem("DownsizeData");

        player = FindAnyObjectByType<TileMapObject>().currentPlayer.GetComponent<Player>();
        
        _canvas = UiManager.GetCanvas();
        cardDownsize = Instantiate(cardDownsizePrefab, _canvas.transform);


        background = cardDownsize.transform.Find("Background").gameObject;

        background.transform.Find("TextDescription").gameObject.GetComponent<TextMeshProUGUI>().text = "Вам нужно выплатить сумму, соответствующую вашим расходам, в случае безработицы.\r\n\r\n\r\n";
        okBtn = background.transform.Find("OkButton").gameObject;

        okBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        okBtn.GetComponent<Button>().onClick.AddListener(() => { 
            player.TakeCash(player.TotalExpenses);
            Destroy(cardDownsize);
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
