using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, IPunObservable
{
    public int Cash;
    public int TotalIncome;
    public int TotalExpenses;
    public Slider cashflow;
    public GameObject bookItemPrefab;
    public bool isCashflow = false;
    public int dreamId;
    public float sliderVaule;
    public float maxSliderVaule;

    public JsonData jsonData;
    private GameObject bookItem;
    private GameManager gameManager;
    [SerializeField]
    private CashBook book;
    [SerializeField]
    public PhotonView view;
    PlayerSetup playerSetup;
    private int yourPay = 250000;

    // Start is called before the first frame update
    void Start()
    {
        jsonData = GameObject.FindAnyObjectByType<JsonData>();
        playerSetup = GameObject.FindAnyObjectByType<PlayerSetup>();
        view = GetComponent<PhotonView>();
        if (!view.IsMine) return;
        book = GameObject.Find("CashBook").GetComponent<CashBook>();
        gameManager = GameObject.FindAnyObjectByType<GameManager>();
        cashflow = book.sliderCashFlow;
        book.sliderCashFlow.onValueChanged.AddListener(ToggleOut);

        Cash = yourPay;
        AddLiablities(600000, "Кредит на машину");
        AddLiablities(5000000, "Кредит на дом");
        AddIncome(yourPay, "Работа консультантом");
        Cash = TotalIncome;
        CalculateUpdate();

        Item item = jsonData.GetRandomItem("DreamData");

        dreamId = item.assetID;

        book.DreamText.text = book.DreamText.text + $" {item.description}";

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ToggleOut(float value)
    {
        if (value >= book.sliderCashFlow.maxValue && value != 0 && book.sliderCashFlow.maxValue != 0)
        {
            isCashflow = true;
        }
    }
    public void CalculateUpdate()
    {
        gameManager.ProfileSetup(playerSetup.players);
        if (!view.IsMine) return;
        TotalIncome = 0;
        TotalExpenses = 0;


        BookViewItem[] incomeItems = book.IncomeView.GetComponentsInChildren<BookViewItem>();
        BookViewItem[] expensesItems = book.ExpensesView.GetComponentsInChildren<BookViewItem>();

        foreach (var item in incomeItems)
        {
            TotalIncome += int.Parse(item.price.text.Split("ТГ")[0], NumberStyles.Number);

        }


        foreach (var item in expensesItems)
        {
            TotalExpenses += int.Parse(item.price.text.Split("ТГ")[0], NumberStyles.Number);

        }


        book.CashPrice.text = Cash.ToString("N0") + "ТГ";
        book.IncomePrice.text = TotalIncome.ToString("N0") + "ТГ";
        book.ExpensesPrice.text = TotalExpenses.ToString("N0") + "ТГ";
        book.PayDayPrice.text = (TotalIncome - TotalExpenses).ToString("N0") + "ТГ";

        book.sliderCashFlow.maxValue = TotalExpenses;
        book.sliderCashFlow.value = TotalIncome - yourPay;


        sliderVaule = cashflow.value;
        maxSliderVaule = cashflow.maxValue;

        //gameManager.ProfileSetup(playerSetup.players);

        gameManager.CheckLose(Cash);
        //GetComponent<Pawn>().sliderVaule = (int)book.sliderCashFlow.maxValue;
        //GetComponent<Pawn>().maxSliderVaule = (int)book.sliderCashFlow.value;

        //FindObjectsByType<Profile>(FindObjectsSortMode.InstanceID).Where(p => p.viewId == view.ViewID).First().slider = book.sliderCashFlow;

    }
    public void PayDayCheck(TileObject tile)
    {
        if (!view.IsMine) return;

        CardPayDay cardPayDay;

        if (tile.TryGetComponent<CardPayDay>(out cardPayDay))
        {
            Cash += TotalIncome - TotalExpenses;
        }

        //if (Cash < 0)
        //{
        //    FindAnyObjectByType<GameManager>().CheckLose(Cash);
        //}

        CalculateUpdate();
    }

    private void WriteToCashBookItem(float price, string description, int count = 1)
    {
        if (!view.IsMine) return;
        BookViewItem bookViewItem;
        bookViewItem = bookItem.GetComponent<BookViewItem>();

        bookViewItem.description.text = description;

        if (count > 1)
        {
            bookViewItem.price.text = price.ToString("N0") + $"ТГ X{count}";

        }
        else
        {
            bookViewItem.price.text = price.ToString("N0") + "ТГ";
        }

        bookViewItem.count = count;

        CalculateUpdate();
    }
    private bool CanIBuy(float price)
    {
        return Cash >= price;
    }
    public void TakeCash(int price)
    {
        if (!view.IsMine) return;
        if ((Cash - price) >= 0)
        {
            Cash -= price;
        }
        else
        {
            AddLiablities(price, "Кредит на погашение долгов");

            //Debug.Log("У вас недостаточно денег");
        }

        CalculateUpdate();
    }
    public void GiveCash(int price)
    {
        if (!view.IsMine) return;
        Cash += price;

        CalculateUpdate();
    }
    public void AddIncome(int price = 0, string description = "Default", int count = 1)
    {
        if (!view.IsMine) return;
        if (CanIBuy(price))
        {
            bookItem = Instantiate(bookItemPrefab, book.IncomeView.transform);
            Cash -= price;
            WriteToCashBookItem(price, description, count);
        }


    }
    public void AddExpenses(float price, string description = "Default")
    {
        if (!view.IsMine) return;
        bookItem = Instantiate(bookItemPrefab, book.ExpensesView.transform);

        WriteToCashBookItem(price, description);
    }
    public void AddAssets(int price, string description = "Default", int cashFlow = 0, int assetID = 0, int count = 1)
    {
        if (!view.IsMine) return;
        if (count <= 0) return;

        GameObject oldBookItem = null;

        if (CanIBuy(price * count))
        {
            bookItem = Instantiate(bookItemPrefab, book.AssetsView.transform);
            oldBookItem = bookItem;

            Cash -= price * count;

            oldBookItem.GetComponent<BookViewItem>().assetID = assetID;
            WriteToCashBookItem(price, description, count);
        }
        else
        {
            return;
        }
        if (cashFlow != 0 && oldBookItem != null && assetID != 0)
        {
            Cash += cashFlow;
            AddIncome(cashFlow, description, count);
            oldBookItem.GetComponent<BookViewItem>().IncomeItem = bookItem;
            oldBookItem.GetComponent<BookViewItem>().assetID = assetID;
        }
        Debug.Log($"Перед проверкой:assetID:{assetID} dreamID:{dreamId} check:{assetID == dreamId}");
        if (assetID == dreamId)
        {
            Debug.Log("После проверки");
            gameManager.CheckVictory(dreamId, assetID, view.ViewID);
        }


    }
    public void AddLiablities(float price, string description = "Default", float downPay = 0)
    {
        if (!view.IsMine) return;
        if (!CanIBuy(downPay)) return;

        bookItem = Instantiate(bookItemPrefab, book.LiablitiesView.transform);
        GameObject oldBookItem = bookItem;


        WriteToCashBookItem(price, description);

        AddExpenses(price / 100 * 4, description + " платеж");
        oldBookItem.GetComponent<BookViewItem>().ExpensesItem = bookItem;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) // Если мы отправляем данные по сети
        {
            stream.SendNext(view.ViewID);
            stream.SendNext(isCashflow);

            stream.SendNext(cashflow.value);
            stream.SendNext(cashflow.maxValue);

        }
        else // Если мы принимаем данные по сети
        {
            int tempViewId = (int)stream.ReceiveNext();
            bool tempIsCashFlow = (bool)stream.ReceiveNext();

            float tempCashflowValue = (float)stream.ReceiveNext();
            float tempCashflowMaxValue = (float)stream.ReceiveNext();

            Debug.Log($"{tempCashflowValue}");
            Debug.Log($"{tempCashflowMaxValue}");

            if (view == null) return;
            if (tempViewId == view.ViewID)
            {
                sliderVaule = tempCashflowValue;
                maxSliderVaule = tempCashflowMaxValue;
                isCashflow = tempIsCashFlow;
                
            }
        }
    }
}
