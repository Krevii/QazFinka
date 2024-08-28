using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardPayDay : MonoBehaviour, ICard
{
    private static Canvas _canvas;
    public GameObject payDayCardPrefab;
    private GameObject payDayCard;
    private GameObject backGround;
    private GameObject title;
    private GameObject description;
    private GameObject okBtn;

    
    public void DoAction()
    {
        _canvas = UiManager.GetCanvas();
        payDayCard = Instantiate(payDayCardPrefab, _canvas.transform);

        backGround = payDayCard.transform.Find("Background").gameObject;
        title = backGround.transform.Find("Title").gameObject;
        description = backGround.transform.Find("TextDescription").gameObject;
        okBtn = backGround.transform.Find("OkButton").gameObject;

        description.transform.GetComponent<TextMeshProUGUI>().text = "Время получения деняК";

        okBtn.transform.GetComponent<Button>().onClick.AddListener(() => { Destroy(payDayCard); });  

        Debug.Log("ДЕРЖИ PAYDAY");
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
