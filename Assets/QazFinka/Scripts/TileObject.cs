using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject : MonoBehaviour
{
    public string tileType;

    private ICard _cardType;
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _cardType = GetComponent<ICard>();
        _animator = GetComponent<Animator>();
    }

    public ICard GetCardType()
    {
        return _cardType;
    }

    void Update()
    {
        
    }
    public void WaveTileAnimation()
    {
        _animator.Play("TileWaveAnim");
    }
    
}
