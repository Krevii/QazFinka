using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UiManager : MonoBehaviour
{

    private static UiManager instance;
    private static Canvas _canvas;


    // Start is called before the first frame update
    void Start()
    {
        _canvas = GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static UiManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<Canvas>().GetComponent<UiManager>();
            }
            return instance;
        }
    }
    public static Canvas GetCanvas()
    {
        return _canvas;
    }
}
