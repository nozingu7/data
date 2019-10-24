using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ItemType
{
    head, body, left, right, leg
}

[Serializable]
public struct PlayerState
{
    public int hp;
    public int mp;
    public int str;
    public int def;
    public int dex;
    public int _int;
}

[Serializable]
public struct SelectItemData
{
    public string name;
    public int price;
    public string information;
    public string sprite;
    public int spriteSize;
    public ItemType type;
    public PlayerState state;

    public SelectItemData(string _name, int _price, string _information, string _sprite, int _spriteSize, ItemType _type, PlayerState _state)
    {
        name = _name;
        price = _price;
        information = _information;
        sprite = _sprite;
        spriteSize = _spriteSize;
        type = _type;
        state = _state;
    }
}


public class Manager : MonoBehaviour
{
    static public Manager instance = null;

    [SerializeField]
    public PlayerState playerState;
    public Type itemType;
    public List<SelectItemData> itemList = new List<SelectItemData>();
    public LinkedList<GameObject> possessionItem = new LinkedList<GameObject>();
    public List<Item> playerItem = new List<Item>();
    public Transform[] sellerSlot = new Transform[10];
    public Transform[] inventorySlot = new Transform[20];
    public Transform[] stateSlot = new Transform[5];
    public GameObject inventory;
    public Camera cam;
    public UILabel moenyPrint;
    public UILabel sellerLabel;
    public UILabel stateLabel;
    public UILabel moneyEffect;
    public Transform selectObject;
    public Transform inventorySelectObject;
    public Transform dragItem;
    public int curMoney;
    public int playerSlotCount;
    public int equipItemCount;
    public GameObject msg;
    public GameObject block;
    public GameObject exitMsg;

    [SerializeField]
    public SelectItemData tempItem;

    // 프리팹
    public GameObject Item;
    public GameObject sellerItem;
    public GameObject selectImg;
    public GameObject popup;

    // 다시 시작
    public Transform oldTR;
    public Transform newTR;

    public UICamera uicam;

    private PlayerState temp;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            print("매니저 오류");
            return;
        }
    }

    // Use this for initialization
    void Start()
    {
        cam = NGUITools.FindCameraForLayer(gameObject.layer);

        curMoney = 100000;

        playerState.hp = 100;
        playerState.mp = 100;
        playerState.str = 30;
        playerState.def = 30;
        playerState.dex = 30;
        playerState._int = 30;

        for (int x = 0; x <10; x++)
        {
            if (x == 0)
            {
                temp.hp = 0;
                temp.mp = 0;
                temp.str = 15;
                temp.def = 0;
                temp.dex = -5;
                temp._int = 0;

                itemList.Add(new SelectItemData("도끼", 200, "날이 예리한 도끼\n힘 +15 민첩 -5", "axe", 90, ItemType.left, temp));
            }
            else if (x == 1)
            {
                temp.hp = 0;
                temp.mp = 0;
                temp.str = 10;
                temp.def = 0;
                temp.dex = -2;
                temp._int = 0;

                itemList.Add(new SelectItemData("롱소드", 1200, "장인의 손길로 만들어진 칼\n힘 +10 민첩 -2", "sword", 80, ItemType.left, temp));
            }
            else if (x == 2)
            {
                temp.hp = 0;
                temp.mp = 20;
                temp.str = 0;
                temp.def = 0;
                temp.dex = 0;
                temp._int = 10;

                itemList.Add(new SelectItemData("마법책", 9000, "백과사전\n기력 +20 지능 10", "book", 90, ItemType.left, temp));
            }
            else if (x == 3)
            {
                temp.hp = 0;
                temp.mp = 10;
                temp.str = 0;
                temp.def = 0;
                temp.dex = 0;
                temp._int = 5;

                itemList.Add(new SelectItemData("스크롤", 7200, "신비한 마법의 스크롤\n기력 +10 지능 5", "scroll", 80, ItemType.left, temp));
            }
            else if (x == 4)
            {
                temp.hp = 50;
                temp.mp = 0;
                temp.str = 0;
                temp.def = 20;
                temp.dex = -5;
                temp._int = 0;

                itemList.Add(new SelectItemData("가죽갑옷", 1100, "섬세한 가죽갑옷\n체력 +50 방어 +20 민첩 -5", "armor", 85, ItemType.body, temp));
            }
            else if (x == 5)
            {
                temp.hp = 0;
                temp.mp = 0;
                temp.str = 0;
                temp.def = 10;
                temp.dex = -10;
                temp._int = 0;

                itemList.Add(new SelectItemData("도란방패", 5000, "메두사의 방패\n방어 +10 민첩 -10", "shield", 90, ItemType.right, temp));
            }
            else if (x == 6)
            {
                temp.hp = 0;
                temp.mp = 0;
                temp.str = 0;
                temp.def = 5;
                temp.dex = 5;
                temp._int = 0;

                itemList.Add(new SelectItemData("가죽장화", 800, "섬세한 가죽장화\n방어 +5 민첩 +5", "boots", 85, ItemType.leg, temp));
            }
            else if (x == 7)
            {
                temp.hp = 20;
                temp.mp = 0;
                temp.str = 0;
                temp.def = 10;
                temp.dex = 0;
                temp._int = 0;

                itemList.Add(new SelectItemData("강철투구", 3000, "단단한 강철투구\n체력 +20 방어 +10", "helmets", 85, ItemType.head, temp));
            }
            else if (x == 8)
            {
                temp.hp = 0;
                temp.mp = 0;
                temp.str = 0;
                temp.def = 0;
                temp.dex = 10;
                temp._int = 0;

                itemList.Add(new SelectItemData("나무활", 500, "초보자용 나무활\n민첩 +10", "bow", 85, ItemType.left, temp));
            }
            else if (x == 9)
            {
                temp.hp = 0;
                temp.mp = 0;
                temp.str = 0;
                temp.def = 0;
                temp.dex = 0;
                temp._int = 10;

                itemList.Add(new SelectItemData("마법반지", 2500, "영롱한 반지\n지능 +10", "rings", 85, ItemType.right, temp));
            }
        }

        for (int a = 0; a < 10; a++)
        {
            if (sellerSlot[a].childCount == 0)
            {
                GameObject selItem = Instantiate(sellerItem);
                selItem.transform.parent = sellerSlot[a];
                selItem.transform.localPosition = Vector3.zero;
                selItem.transform.localScale = Vector3.one;
                SellerItem temp = selItem.GetComponent<SellerItem>();
                UISprite tempSP = selItem.GetComponent<UISprite>();
                temp.name = itemList[a].name;
                temp.price = itemList[a].price;
                tempSP.spriteName = itemList[a].sprite;
                tempSP.width = itemList[a].spriteSize;
                tempSP.height = itemList[a].spriteSize;
                temp.selectItem = itemList[a];
                if (temp.name == "도끼")
                {
                    temp.selectItem.type = ItemType.left;
                }
                else if (temp.name == "롱소드")
                {
                    temp.selectItem.type = ItemType.left;
                }
                else if (temp.name == "마법책")
                {
                    temp.selectItem.type = ItemType.left;
                }
                else if (temp.name == "스크롤")
                {
                    temp.selectItem.type = ItemType.left;
                }
                else if (temp.name == "가죽갑옷")
                {
                    temp.selectItem.type = ItemType.body;
                }
                else if (temp.name == "도란방패")
                {
                    temp.selectItem.type = ItemType.right;
                }
                else if (temp.name == "가죽장화")
                {
                    temp.selectItem.type = ItemType.leg;
                }
                else if (temp.name == "강철투구")
                {
                    temp.selectItem.type = ItemType.head;
                }
                else if (temp.name == "나무활")
                {
                    temp.selectItem.type = ItemType.left;
                }
                else if (temp.name == "마법반지")
                {
                    temp.selectItem.type = ItemType.right;
                }
            }
        }

        for (int i = 0; i < stateSlot.Length; i++)
        {
            StateSlot _stateSlot = stateSlot[i].GetComponent<StateSlot>();

            if (i == 0)
            {
                _stateSlot.slotType = ItemType.head;
            }
            else if (i == 1)
            {
                _stateSlot.slotType = ItemType.body;
            }
            else if (i == 2)
            {
                _stateSlot.slotType = ItemType.left;
            }
            else if (i == 3)
            {
                _stateSlot.slotType = ItemType.right;
            }
            else
            {
                _stateSlot.slotType = ItemType.leg;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        moenyPrint.text = curMoney.ToString();
        stateLabel.text = string.Format("체력 : {0}     기력 : {1}\n 힘   : {2}       방어 : {3}\n민첩 : {4}       지능 : {5}",
                                        playerState.hp, playerState.mp, playerState.str, playerState.def, playerState.dex, playerState._int);

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            exitMsg.SetActive(true);
        }
    }
}
