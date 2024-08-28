using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pawn : MonoBehaviour, IPunObservable
{
    public float speed;
    public string skin = "";
    public string nickname = "";
    public TileObject currentTile; // Добавляем переменную для хранения текущей плитки
    public PlayerData playerData;
    private PhotonView view;
    private Animator animator;
    private Vector3 endPositon;
    private int ViewId;
    Vector3 oldposition;
    public int sliderVaule;
    public int maxSliderVaule;
    private Profile profile;

    void Start()
    {
        view = GetComponent<PhotonView>();
        ViewId = view.ViewID;

        if (skin != "")
        {
            GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>(skin);
        }

        if (view.IsMine)
        {
            GetComponentInChildren<SpriteRenderer>().color = Color.green;
        }

        

        oldposition = transform.position;

        animator = GetComponent<Animator>();
        //Debug.Log(Resources.Load<TextAsset>("JsonData"));
        //Debug.Log(JsonUtility.FromJson<ItemsList>(Resources.Load<TextAsset>("JsonData").text).items[0].description);

    }

    void Update()
    {
        MoveUpdate(transform.position, endPositon);
    }

    public void MovePawn(Vector3 moveTo)
    {
        Vector3 startPosition = transform.position;
        endPositon = moveTo;
        oldposition = moveTo;
        //MoveUpdate(startPosition, endPositon);
        animator.Play("WalkerPlayer");
    }
    private void MoveUpdate(Vector3 start, Vector3 end)
    {
        if (end == Vector3.zero) return;

        transform.position = Vector3.Lerp(start, end, speed * Time.deltaTime);
    }
    public void TileCheck(TileObject tile)
    {
        ICard card = tile.GetCardType();
        card.DoAction();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) // Если мы отправляем данные по сети
        {
            stream.SendNext(sliderVaule);
            stream.SendNext(maxSliderVaule);
            stream.SendNext(nickname);
            stream.SendNext(skin);
            stream.SendNext(ViewId);

        }
        else // Если мы принимаем данные по сети
        {
            int value = (int)stream.ReceiveNext();
            int valueMax = (int)stream.ReceiveNext();
            string nick = (string)stream.ReceiveNext();
            string tempSkin = (string)stream.ReceiveNext();
            int tempViewId = (int)stream.ReceiveNext();

            if (view == null) return;
            if (tempViewId == view.ViewID && skin == "")
            {
                GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>(tempSkin);
                nickname = nick;
            }
        }
    }
}
