using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonData : MonoBehaviour
{
    // Конвертирование JSON в объекты Item
    private List<Item> itemsList;

    void Start()
    {
        // Парсим JSON в список объектов Item
        //ParseJSON();
    }

    void ParseJSON()
    {
        //string json = Resources.Load<TextAsset>("JsonData").text;

        // Парсим JSON в список объектов Item
        
        //itemsList = JsonUtility.FromJson<ItemsList>(json).items;
    }

    public Item GetRandomItem(string fileName)
    {
        string json = Resources.Load<TextAsset>(fileName).text;

        // Парсим JSON в список объектов Item
        itemsList = JsonUtility.FromJson<ItemsList>(json).items;
        int randomIndex = UnityEngine.Random.Range(0, itemsList.Count);
        
        return itemsList[randomIndex];

    }
}

[Serializable]
public class Item
{
    public string description;
    public int cost;
    public int cashFlow;
    //public float downPay;
    public int pay;
    public int Donation;
    public string Privilege;
    public int assetID;
}

// Класс-контейнер для списка объектов Item
[Serializable]
public class ItemsList
{
    public List<Item> items;

    
}