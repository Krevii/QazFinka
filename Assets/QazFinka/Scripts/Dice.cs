using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [Range(1, 1000)]
    public int rangeMax;
    public int seed = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int DiceRoll()
    {
        if (seed != 0)
        {
            Random.InitState(seed);
            
        }
        
        int randomNumber = Random.Range(1, rangeMax + 1);

        return randomNumber;
    }
}
