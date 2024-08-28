using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BookViewItem : MonoBehaviour
{
    public Text description;
    public Text price;
    public float cash;
    public GameObject ExpensesItem;
    public GameObject IncomeItem;
    public int assetID;
    public int count;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        //description = transform.GetComponentInChildren<Text>();
        //price = transform.GetChild(1).transform.GetComponentInChildren<Text>();



    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DeleteItem()
    {
        if (transform.parent.gameObject.name != "Liablities View") return;
        if (player == null)
        {
            //player = FindFirstObjectByType<Player>();
            player = FindObjectsByType<Player>(FindObjectsSortMode.InstanceID).Where(player => player.view.IsMine).First();
        }
        int needCash = int.Parse(price.text.Split("ТГ")[0], NumberStyles.Number);

        if (player.Cash >= needCash)
        {
            player.Cash -= needCash;

            Destroy(ExpensesItem);
            Destroy(gameObject);

        }
        else
        {
            Debug.Log($"Не хватает денег:{needCash}/ сколько есть{player.Cash}");
        }


    }
    public void DeleteItemAssets()
    {
        Destroy(IncomeItem);
        Destroy(gameObject);
    }
    public void SellAssetItem()
    {

        if (transform.parent.gameObject.name != "Assets View") return;

        if (player == null)
        {
            player = FindFirstObjectByType<Player>();
        }

        GameObject cardOffer = GameObject.FindGameObjectWithTag("cardOffer");
        if (cardOffer == null) return;
        
        GameObject cardAssetId = cardOffer.transform.Find("Background").transform.Find("assetID").gameObject;


        if (assetID == int.Parse(cardAssetId.GetComponent<TextMeshProUGUI>().text))
        {

            player.Cash += int.Parse(cardOffer.transform.Find("Background").transform.Find("TextCost").GetComponent<TextMeshProUGUI>().text.Split(":")[1]);

            Destroy(IncomeItem);
            Destroy(gameObject);

        }
    }
    private void OnDestroy()
    {
        if (player != null)
        {
            player.CalculateUpdate();
        }
    }
}
