using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice_Test : MonoBehaviour
{
    public int Sseed = 321;
    private System.Random random;

    void Start()
    {
        // ������������� ���������� ��������� ����� � ��������� seed.
        // ���� seed �� ������ (����� ����), ������������ ������� �����.
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
        // ��������� ���������� ����� �� 1 �� 6 (������� ������� �������������).
        int randomNumber = random.Next(1, 7);

        return randomNumber;
    }
}
