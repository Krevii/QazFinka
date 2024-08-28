using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardCharity : MonoBehaviour, ICard
{

    public JsonData jsonData;
    public GameObject cardCharityPrefab;
    private GameObject cardCharity;
    public Player player;

    private GameObject title;
    private GameObject description;
    private GameObject backGround;
    private GameObject bLeft;
    private GameObject bRight;
    private GameObject donate;
    private GameObject privilege;
    private static Canvas _canvas;

    public void DoAction()
    {
        Item item = jsonData.GetRandomItem("CharityData");
        _canvas = UiManager.GetCanvas();

        cardCharity = Instantiate(cardCharityPrefab, _canvas.transform);

        player = FindAnyObjectByType<TileMapObject>().currentPlayer.GetComponent<Player>();

        backGround = cardCharity.transform.Find("Background").gameObject;

        title = backGround.transform.Find("Title").gameObject;
        description = backGround.transform.Find("TextDescription").gameObject;
        donate = backGround.transform.Find("TextDonation").gameObject;
        privilege = backGround.transform.Find("TextPrivilege").gameObject;
        bLeft = backGround.transform.Find("LeftButton").gameObject;
        bRight = backGround.transform.Find("RightButton").gameObject;

        bLeft.GetComponent<Button>().onClick.AddListener(() => { player.TakeCash(item.Donation); Destroy(cardCharity); });
        bRight.GetComponent<Button>().onClick.AddListener(() => { Destroy(cardCharity); });

        description.GetComponent<TextMeshProUGUI>().text = item.description;
        donate.GetComponent<TextMeshProUGUI>().text = "Donation: " + item.Donation.ToString("N") + "“√";
        privilege.GetComponent<TextMeshProUGUI>().text = item.Privilege;

        Debug.Log("¿Œ¿Œ¿Œ¿Œ¿Œ¿ Ã€ ¬ CARD Charity");
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
