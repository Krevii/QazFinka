using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class Border : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI diceResult;
    public int templateVaribleSeed;
    public Dice dice;
    public GameObject pawn;
    public GameObject pawn2;

    private Tilemap _tileMap;
    private Vector3Int _startPosition;
    private Vector3Int _oldTilePos;
    private IEnumerator _currentWalker;
    private int _steps;

    private void Awake()
    {
        pawn = Instantiate(pawn, _tileMap.GetCellCenterWorld(_startPosition), Quaternion.identity);
        pawn2 = Instantiate(pawn2, _tileMap.GetCellCenterWorld(_startPosition), Quaternion.identity);
    }
    void Start()
    {
        if (TryGetComponent(out _tileMap))
        {
            _tileMap.CompressBounds();
            BoundsInt bounds = _tileMap.cellBounds;
            Debug.Log(bounds.ToString());
        }
        _startPosition = new Vector3Int(_tileMap.cellBounds.xMin, _tileMap.cellBounds.yMin, 0);
        _oldTilePos = _startPosition;

    }
    // Update is called once per frame
    void Update()
    {
    }
    private void OnGUI()
    {

        if (GUI.Button(new Rect(500, 300, 100, 50), "DiceRoll"))
        {
            if (dice != null)
            {
                for (int x = _tileMap.cellBounds.xMin; x < _tileMap.cellBounds.xMax; x++)
                {
                    for (int y = _tileMap.cellBounds.yMin; y < _tileMap.cellBounds.yMax; y++)
                    {
                        _tileMap.SetTileFlags(new Vector3Int(x, y, 0), TileFlags.None);
                        _tileMap.SetColor(new Vector3Int(x, y, 0), Color.white);
                    }
                }
                _steps = dice.DiceRoll();
                diceResult.text = $"Результат броска:{_steps}";
                if (_currentWalker != null)
                {
                    StartCoroutine(_currentWalker);
                }
                else
                {
                    StartCoroutine(WalkOnTheLeftSideOfTheBoard());
                }

            }
        }


    }
    private void CheckTile(Vector3Int tile)
    {
        string tileName = _tileMap.GetTile(tile).name;
       
        switch (tileName.ToLower())
        {
            case "deal":
                Debug.Log(tileName);
                break;
            case "payday":
                Debug.Log(tileName);
                break;
            case "downsize":
                Debug.Log(tileName);
                break;
            case "doodads":
                Debug.Log(tileName);
                break;
            case "offer":
                Debug.Log(tileName);
                break;
            case "charity":
                Debug.Log(tileName);
                break;
            default:
                Debug.Log("Вы наступили на пустую клетку");
                break;
        }

        
    }
    private void MovePawn(Vector3 moveTo)
    {

        pawn.transform.position = moveTo;
    }
    void VisitTile(int x, int y)
    {
        Vector3Int tilePosition = new Vector3Int(x, y, 0);
        Tile tile = _tileMap.GetTile<Tile>(tilePosition);

        _oldTilePos = tilePosition;

        if (tile != null)
        {
            MovePawn(_tileMap.GetCellCenterWorld(tilePosition));
        }
    }
    public IEnumerator WalkOnTheLeftSideOfTheBoard()
    {
        // Левая граница, снизу вверх
        for (int y = _tileMap.cellBounds.yMin; y < _tileMap.cellBounds.yMax - 1 && _steps > 0; y++)
        {
            if (y < _oldTilePos.y)
            {
                continue;
            }
            yield return new WaitForSeconds(.3f);
            //_tileMap.SetColor(new Vector3Int(_tileMap.cellBounds.xMin, y, 0), Color.red);
            VisitTile(_tileMap.cellBounds.xMin, y);
            _steps--;
        }
        _currentWalker = WalkOnTheLeftSideOfTheBoard();
        if (_steps > 0)
        {
            StartCoroutine(WalkOnTheTopSideOfTheBoard());
        }
        else
        {
            CheckTile(_oldTilePos);
            _currentWalker = WalkOnTheLeftSideOfTheBoard();
        }
    }
    public IEnumerator WalkOnTheTopSideOfTheBoard()
    {
        // Верхняя граница, слева направо
        for (int x = _tileMap.cellBounds.xMin; x < _tileMap.cellBounds.xMax - 1 && _steps > 0; x++)
        {
            if (x < _oldTilePos.x)
            {
                continue;
            }
            yield return new WaitForSeconds(.3f);
            //_tileMap.SetColor(new Vector3Int(x, _tileMap.cellBounds.yMax - 1, 0), Color.cyan);
            VisitTile(x, _tileMap.cellBounds.yMax - 1);
            _steps--;
        }
        if (_steps > 0)
        {
            StartCoroutine(WalkOnTheRightSideOfTheBoard());
        }
        else
        {
            CheckTile(_oldTilePos);
            _currentWalker = WalkOnTheTopSideOfTheBoard();
        }
    }
    public IEnumerator WalkOnTheRightSideOfTheBoard()
    {
        // Правая граница, сверху вниз
        for (int y = _tileMap.cellBounds.yMax - 1; y > _tileMap.cellBounds.yMin && _steps > 0; y--)
        {
            if (y > _oldTilePos.y)
            {
                continue;
            }
            yield return new WaitForSeconds(.3f);
            //_tileMap.SetColor(new Vector3Int(_tileMap.cellBounds.xMax - 1, y, 0), Color.green);
            VisitTile(_tileMap.cellBounds.xMax - 1, y);
            _steps--;
        }
        if (_steps > 0)
        {
            StartCoroutine(WalkOnTheBottomSideOfTheBoard());
        }
        else
        {
            CheckTile(_oldTilePos);
            _currentWalker = WalkOnTheRightSideOfTheBoard();
        }
    }
    public IEnumerator WalkOnTheBottomSideOfTheBoard()
    {
        // Нижняя граница, справа налево
        for (int x = _tileMap.cellBounds.xMax - 1; x > _tileMap.cellBounds.xMin && _steps > 0; x--)
        {
            if (x > _oldTilePos.x)
            {
                continue;
            }
            yield return new WaitForSeconds(.3f);
            //_tileMap.SetColor(new Vector3Int(x, _tileMap.cellBounds.xMin, 0), Color.blue);
            VisitTile(x, _tileMap.cellBounds.yMin);
            _steps--;
        }
        if (_steps > 0)
        {
            StartCoroutine(WalkOnTheLeftSideOfTheBoard());
        }
        else
        {
            CheckTile(_oldTilePos);
            _currentWalker = WalkOnTheBottomSideOfTheBoard();
        }
    }

    IEnumerator WalkPerimeter(int diceResult)
    {

        StartCoroutine(WalkOnTheLeftSideOfTheBoard());

        yield return null;
    }
}
