using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CashBook : MonoBehaviour
{
    [SerializeField]
    private GameObject incomeView;
    [SerializeField] 
    private GameObject expensesView;
    private GameObject assetsView;
    private GameObject liablitiesView;

    private Text cashPrice;
    private Text incomePrice;
    private Text expensesPrice;
    private Text payDayPrice;
    private Text dreamText;

    public GameObject IncomeView { get => incomeView; set => incomeView = value; }
    public GameObject ExpensesView { get => expensesView; set => expensesView = value; }
    public GameObject AssetsView { get => assetsView; set => assetsView = value; }
    public GameObject LiablitiesView { get => liablitiesView; set => liablitiesView = value; }
    
    public Text CashPrice { get => cashPrice; set => cashPrice = value; }
    public Text IncomePrice { get => incomePrice; set => incomePrice = value; }
    public Text ExpensesPrice { get => expensesPrice; set => expensesPrice = value; }
    public Text PayDayPrice { get => payDayPrice; set => payDayPrice = value; }
    public Text DreamText { get => dreamText; set => dreamText = value; }

    public Slider sliderCashFlow;

    private void Awake()
    {
        DreamText = GameObject.Find("Dream text").GetComponent<Text>();

        CashPrice = GameObject.Find("Cash Price").GetComponent<Text>();
        IncomePrice = GameObject.Find("Income Price").GetComponent<Text>();
        ExpensesPrice = GameObject.Find("Expenses Price").GetComponent<Text>();
        PayDayPrice = GameObject.Find("PayDay Price").GetComponent<Text>();

        IncomeView = GameObject.Find("Income View");
        ExpensesView = GameObject.Find("Expenses View");
        AssetsView = GameObject.Find("Assets View");
        LiablitiesView = GameObject.Find("Liablities View");


        IncomeView = IncomeView.GetComponentInChildren<VerticalLayoutGroup>().gameObject;
        ExpensesView = ExpensesView.GetComponentInChildren<VerticalLayoutGroup>().gameObject;
        AssetsView = AssetsView.GetComponentInChildren<VerticalLayoutGroup>().gameObject;
        LiablitiesView = LiablitiesView.GetComponentInChildren<VerticalLayoutGroup>().gameObject;

        sliderCashFlow = GetComponentInChildren<Slider>();
        
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

}