using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;
using UnityEngine.SceneManagement;

public class SaveLoad : MonoBehaviour
{
    PlayerState itemdata;

    public void Save()
    {
        string filePath = string.Format(Application.streamingAssetsPath + "/GameData.xml");

        XmlDocument document = new XmlDocument();
        XmlElement GameData = document.CreateElement("게임데이터");
        document.AppendChild(GameData);
        XmlElement inventoryItem = document.CreateElement("인벤");
        GameData.AppendChild(inventoryItem);
        inventoryItem.SetAttribute("InvenCount", Manager.instance.playerSlotCount.ToString());
        XmlElement equipItem = document.CreateElement("장착");
        GameData.AppendChild(equipItem);
        equipItem.SetAttribute("EquipCount", Manager.instance.equipItemCount.ToString());

        foreach (var data in Manager.instance.playerItem)
        {
            if (data.equipItem == false)
            {
                XmlElement item = document.CreateElement("아이템");
                item.SetAttribute("이름", data.itemData.name);
                item.SetAttribute("슬롯번호", data.transform.parent.GetComponent<Slot>().number.ToString());
                inventoryItem.AppendChild(item);
            }
            else
            {
                XmlElement equip = document.CreateElement("장착아이템");
                equip.SetAttribute("이름", data.itemData.name);
                equipItem.AppendChild(equip);
            }
        }

        XmlElement money = document.CreateElement("소지금");
        GameData.AppendChild(money);
        money.SetAttribute("돈", Manager.instance.curMoney.ToString());

        document.Save(filePath);

        Manager.instance.block.SetActive(true);
        Manager.instance.msg.SetActive(true);
        TweenScale ts = Manager.instance.msg.GetComponent<TweenScale>();
        UILabel label = Manager.instance.msg.transform.GetChild(0).GetComponent<UILabel>();
        label.text = "저장이 완료 되었습니다.";
        ts.PlayForward();
        SoundAction.instance.audio.volume = 0.5f;
        SoundAction.instance.audio.PlayOneShot(SoundAction.instance.clips[2]);
    }

    public void Load()
    {
        Manager.instance.playerState.hp = 100;
        Manager.instance.playerState.mp = 100;
        Manager.instance.playerState.str = 30;
        Manager.instance.playerState.def = 30;
        Manager.instance.playerState.dex = 30;
        Manager.instance.playerState._int = 30;

        foreach (Transform slot in Manager.instance.inventorySlot)
        {
            if (slot.childCount > 0)
            {
                Destroy(slot.GetChild(0).gameObject);
            }
        }
        foreach (Transform stateSlot in Manager.instance.stateSlot)
        {
            if (stateSlot.childCount > 0)
            {
                Destroy(stateSlot.GetChild(0).gameObject);
            }
        }

        string filePath = string.Format(Application.streamingAssetsPath + "/GameData.xml");
        XmlDocument document = new XmlDocument();
        document.Load(filePath);
        XmlElement dataInfo = (XmlElement)document.FirstChild;

        XmlElement temp = dataInfo["인벤"];
        Manager.instance.playerSlotCount = Convert.ToInt32(temp.GetAttribute("InvenCount"));

        if (Manager.instance.playerSlotCount > 0)
        {
            foreach (XmlElement data in temp.ChildNodes)
            {
                GameObject tem = Instantiate(Manager.instance.Item);
                tem.transform.parent = Manager.instance.inventorySlot[Convert.ToInt32(data.GetAttribute("슬롯번호"))].transform;
                tem.transform.localPosition = Vector3.zero;
                tem.transform.localScale = Vector3.one;
                tem.transform.GetChild(0).gameObject.SetActive(false);
                Item item = tem.GetComponent<Item>();

                if (data.GetAttribute("이름") == "도끼")
                {
                    itemdata.hp = 0;
                    itemdata.mp = 0;
                    itemdata.str = 15;
                    itemdata.def = 0;
                    itemdata.dex = -5;
                    itemdata._int = 0;

                    item.itemData = new SelectItemData("도끼", 200, "날이 예리한 도끼\n힘+15 민첩-5", "axe", 90, ItemType.left, itemdata);
                }
                else if (data.GetAttribute("이름") == "롱소드")
                {
                    itemdata.hp = 0;
                    itemdata.mp = 0;
                    itemdata.str = 10;
                    itemdata.def = 0;
                    itemdata.dex = -2;
                    itemdata._int = 0;

                    item.itemData = new SelectItemData("롱소드", 1200, "장인의 손길로 만들어진 칼\n힘 +10 민첩 -2", "sword", 80, ItemType.left, itemdata);
                }
                else if (data.GetAttribute("이름") == "마법책")
                {
                    itemdata.hp = 0;
                    itemdata.mp = 20;
                    itemdata.str = 0;
                    itemdata.def = 0;
                    itemdata.dex = 0;
                    itemdata._int = 10;

                    item.itemData = new SelectItemData("마법책", 9000, "백과사전\n기력 +20 지능 10", "book", 90, ItemType.left, itemdata);
                }
                else if (data.GetAttribute("이름") == "스크롤")
                {
                    itemdata.hp = 0;
                    itemdata.mp = 10;
                    itemdata.str = 0;
                    itemdata.def = 0;
                    itemdata.dex = 0;
                    itemdata._int = 5;

                    item.itemData = new SelectItemData("스크롤", 7200, "신비한 마법의 스크롤\n기력 +10 지능 5", "scroll", 80, ItemType.left, itemdata);
                }
                else if (data.GetAttribute("이름") == "가죽갑옷")
                {
                    itemdata.hp = 50;
                    itemdata.mp = 0;
                    itemdata.str = 0;
                    itemdata.def = 20;
                    itemdata.dex = -5;
                    itemdata._int = 0;

                    item.itemData = new SelectItemData("가죽갑옷", 1100, "섬세한 가죽갑옷\n체력 +50 방어 +20 민첩 -5", "armor", 85, ItemType.body, itemdata);
                }
                else if (data.GetAttribute("이름") == "도란방패")
                {
                    itemdata.hp = 0;
                    itemdata.mp = 0;
                    itemdata.str = 0;
                    itemdata.def = 10;
                    itemdata.dex = -10;
                    itemdata._int = 0;

                    item.itemData = new SelectItemData("도란방패", 5000, "메두사의 방패\n방어 +10 민첩 -10", "shield", 90, ItemType.right, itemdata);
                }
                else if (data.GetAttribute("이름") == "가죽장화")
                {
                    itemdata.hp = 0;
                    itemdata.mp = 0;
                    itemdata.str = 0;
                    itemdata.def = 5;
                    itemdata.dex = 5;
                    itemdata._int = 0;

                    item.itemData = new SelectItemData("가죽장화", 800, "섬세한 가죽장화\n방어 +5 민첩 +5", "boots", 85, ItemType.leg, itemdata);
                }
                else if (data.GetAttribute("이름") == "강철투구")
                {
                    itemdata.hp = 20;
                    itemdata.mp = 0;
                    itemdata.str = 0;
                    itemdata.def = 10;
                    itemdata.dex = 0;
                    itemdata._int = 0;

                    item.itemData = new SelectItemData("강철투구", 3000, "단단한 강철투구\n체력 +20 방어 +10", "helmets", 85, ItemType.head, itemdata);
                }
                else if (data.GetAttribute("이름") == "나무활")
                {
                    itemdata.hp = 0;
                    itemdata.mp = 0;
                    itemdata.str = 0;
                    itemdata.def = 0;
                    itemdata.dex = 10;
                    itemdata._int = 0;

                    item.itemData = new SelectItemData("나무활", 500, "초보자용 나무활\n민첩 +10", "bow", 85, ItemType.left, itemdata);
                }
                else if (data.GetAttribute("이름") == "마법반지")
                {
                    itemdata.hp = 0;
                    itemdata.mp = 0;
                    itemdata.str = 0;
                    itemdata.def = 0;
                    itemdata.dex = 0;
                    itemdata._int = 10;

                    item.itemData = new SelectItemData("마법반지", 2500, "영롱한 반지\n지능 +10", "rings", 85, ItemType.right, itemdata);
                }

                UISprite sprite = item.GetComponent<UISprite>();
                sprite.spriteName = item.itemData.sprite;
                sprite.height = item.itemData.spriteSize;
                sprite.width = item.itemData.spriteSize;

                Manager.instance.possessionItem.AddLast(tem);
                Manager.instance.playerItem.Add(item);
            }
        }
        temp = dataInfo["장착"];
        Manager.instance.equipItemCount = Convert.ToInt32(temp.GetAttribute("EquipCount"));

        if (Manager.instance.equipItemCount > 0)
        {
            foreach (XmlElement data in temp.ChildNodes)
            {
                GameObject tem = Instantiate(Manager.instance.Item);
                tem.transform.GetChild(0).gameObject.SetActive(false);
                Item item = tem.GetComponent<Item>();
                item.equipItem = true;

                if (data.GetAttribute("이름") == "도끼")
                {
                    itemdata.hp = 0;
                    itemdata.mp = 0;
                    itemdata.str = 15;
                    itemdata.def = 0;
                    itemdata.dex = -5;
                    itemdata._int = 0;

                    item.itemData = new SelectItemData("도끼", 200, "날이 예리한 도끼\n힘+15 민첩-5", "axe", 90, ItemType.left, itemdata);
                }
                else if (data.GetAttribute("이름") == "롱소드")
                {
                    itemdata.hp = 0;
                    itemdata.mp = 0;
                    itemdata.str = 10;
                    itemdata.def = 0;
                    itemdata.dex = -2;
                    itemdata._int = 0;

                    item.itemData = new SelectItemData("롱소드", 1200, "장인의 손길로 만들어진 칼\n힘 +10 민첩 -2", "sword", 80, ItemType.left, itemdata);
                }
                else if (data.GetAttribute("이름") == "마법책")
                {
                    itemdata.hp = 0;
                    itemdata.mp = 20;
                    itemdata.str = 0;
                    itemdata.def = 0;
                    itemdata.dex = 0;
                    itemdata._int = 10;

                    item.itemData = new SelectItemData("마법책", 9000, "백과사전\n기력 +20 지능 10", "book", 90, ItemType.left, itemdata);
                }
                else if (data.GetAttribute("이름") == "스크롤")
                {
                    itemdata.hp = 0;
                    itemdata.mp = 10;
                    itemdata.str = 0;
                    itemdata.def = 0;
                    itemdata.dex = 0;
                    itemdata._int = 5;

                    item.itemData = new SelectItemData("스크롤", 7200, "신비한 마법의 스크롤\n기력 +10 지능 5", "scroll", 80, ItemType.left, itemdata);
                }
                else if (data.GetAttribute("이름") == "가죽갑옷")
                {
                    itemdata.hp = 50;
                    itemdata.mp = 0;
                    itemdata.str = 0;
                    itemdata.def = 20;
                    itemdata.dex = -5;
                    itemdata._int = 0;

                    item.itemData = new SelectItemData("가죽갑옷", 1100, "섬세한 가죽갑옷\n체력 +50 방어 +20 민첩 -5", "armor", 85, ItemType.body, itemdata);
                }
                else if (data.GetAttribute("이름") == "도란방패")
                {
                    itemdata.hp = 0;
                    itemdata.mp = 0;
                    itemdata.str = 0;
                    itemdata.def = 10;
                    itemdata.dex = -10;
                    itemdata._int = 0;

                    item.itemData = new SelectItemData("도란방패", 5000, "메두사의 방패\n방어 +10 민첩 -10", "shield", 90, ItemType.right, itemdata);
                }
                else if (data.GetAttribute("이름") == "가죽장화")
                {
                    itemdata.hp = 0;
                    itemdata.mp = 0;
                    itemdata.str = 0;
                    itemdata.def = 5;
                    itemdata.dex = 5;
                    itemdata._int = 0;

                    item.itemData = new SelectItemData("가죽장화", 800, "섬세한 가죽장화\n방어 +5 민첩 +5", "boots", 85, ItemType.leg, itemdata);
                }
                else if (data.GetAttribute("이름") == "강철투구")
                {
                    itemdata.hp = 20;
                    itemdata.mp = 0;
                    itemdata.str = 0;
                    itemdata.def = 10;
                    itemdata.dex = 0;
                    itemdata._int = 0;

                    item.itemData = new SelectItemData("강철투구", 3000, "단단한 강철투구\n체력 +20 방어 +10", "helmets", 85, ItemType.head, itemdata);
                }
                else if (data.GetAttribute("이름") == "나무활")
                {
                    itemdata.hp = 0;
                    itemdata.mp = 0;
                    itemdata.str = 0;
                    itemdata.def = 0;
                    itemdata.dex = 10;
                    itemdata._int = 0;

                    item.itemData = new SelectItemData("나무활", 500, "초보자용 나무활\n민첩 +10", "bow", 85, ItemType.left, itemdata);
                }
                else if (data.GetAttribute("이름") == "마법반지")
                {
                    itemdata.hp = 0;
                    itemdata.mp = 0;
                    itemdata.str = 0;
                    itemdata.def = 0;
                    itemdata.dex = 0;
                    itemdata._int = 10;

                    item.itemData = new SelectItemData("마법반지", 2500, "영롱한 반지\n지능 +10", "rings", 85, ItemType.right, itemdata);
                }

                Manager.instance.playerState = Item.StateAdd(Manager.instance.playerState, item.itemData.state);
                UISprite sprite = item.GetComponent<UISprite>();
                sprite.spriteName = item.itemData.sprite;
                sprite.height = item.itemData.spriteSize;
                sprite.width = item.itemData.spriteSize;

                Manager.instance.possessionItem.AddLast(tem);
                Manager.instance.playerItem.Add(item);

                foreach (Transform slot in Manager.instance.stateSlot)
                {
                    if (item.itemData.type == slot.gameObject.GetComponent<StateSlot>().slotType)
                    {
                        tem.transform.parent = slot;
                        tem.transform.localPosition = Vector3.zero;
                        tem.transform.localScale = Vector3.one;
                    }
                }
            }
        }

        temp = dataInfo["소지금"];
        Manager.instance.curMoney = Convert.ToInt32(temp.GetAttribute("돈"));

        Manager.instance.block.SetActive(true);
        Manager.instance.msg.SetActive(true);
        TweenScale ts = Manager.instance.msg.GetComponent<TweenScale>();
        UILabel label = Manager.instance.msg.transform.GetChild(0).GetComponent<UILabel>();
        label.text = "불러오기가 완료되었습니다.";
        ts.PlayForward();
        SoundAction.instance.audio.volume = 0.5f;
        SoundAction.instance.audio.PlayOneShot(SoundAction.instance.clips[2]);
    }
}
