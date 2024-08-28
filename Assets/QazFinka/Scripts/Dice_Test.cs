using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice_Test : MonoBehaviour
{
    public int Sseed = 321;
    private System.Random random;

    void Start()
    {
        // Инициализация генератора случайных чисел с указанным seed.
        // Если seed не указан (равен нулю), используется текущее время.
        random = new System.Random(Sseed != 0 ? Sseed : System.Environment.TickCount);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log(DiceRoll());
        }
    }

    public int DiceRoll()
    {
        // Генерация случайного числа от 1 до 6 (верхняя граница исключительно).
        int randomNumber = random.Next(1, 7);

        return randomNumber;
    }
}
