using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seller : MonoBehaviour
{
    enum messageState
    {
        nonSelect = 0,
        minusMoney,
        inventoryFull,
    }

    messageState msgState;
    public GameObject moneyEffect;
    public GameObject message;
    public GameObject block;
    TweenScale twScale;
    bool isBuy = false;

    private void Start()
    {
        twScale = message.GetComponent<TweenScale>();
    }

    private void Update()
    {
        if (block.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                block.SetActive(false);
                twScale.PlayReverse();
            }
        }

    }

    public void Buy()
    {
        if (Manager.instance.selectObject == null)
        {
            msgState = messageState.nonSelect;
            Message();
            Debug.Log("선택한 아이템이 없습니다.");
            if (Manager.instance.inventorySelectObject != null)
            {
                foreach (GameObject list in Manager.instance.possessionItem)
                {
                    if (list.transform.GetChild(0).gameObject.active == true)
                    {
                        list.transform.GetComponent<Item>().isSelect = false;
                        list.transform.GetChild(0).gameObject.SetActive(false);
                        Manager.instance.inventorySelectObject = null;
                        break;
                    }
                }
            }
        }
        else if (Manager.instance.curMoney < Manager.instance.tempItem.price)
        {
            msgState = messageState.minusMoney;
            Message();
            Debug.Log("보유한 돈이 적습니다.");
        }
        else if (Manager.instance.playerSlotCount >= 20)
        {
            msgState = messageState.inventoryFull;
            Message();
            Debug.Log("아이템창이 꽉찼습니다.");
        }
        else
        {
            for (int a = 0; a < Manager.instance.inventorySlot.Length; a++)
            {
                if (Manager.instance.inventorySlot[a].childCount == 0)
                {
                    Manager.instance.playerSlotCount++;
                    GameObject item = Instantiate(Manager.instance.Item);
                    item.transform.GetChild(0).gameObject.SetActive(false);
                    item.transform.parent = Manager.instance.inventorySlot[a].transform;
                    item.transform.localPosition = Vector3.zero;
                    item.transform.localScale = Vector3.one;
                    Item temp = item.GetComponent<Item>();
                    temp.name = Manager.instance.tempItem.name;
                    temp.price = Manager.instance.tempItem.price;
                    temp.itemData = Manager.instance.tempItem;
                    UISprite tempSP = item.GetComponent<UISprite>();
                    tempSP.spriteName = Manager.instance.tempItem.sprite;
                    tempSP.width = Manager.instance.tempItem.spriteSize;
                    tempSP.height = Manager.instance.tempItem.spriteSize;
                    Manager.instance.curMoney -= Manager.instance.tempItem.price;
                    Manager.instance.possessionItem.AddLast(item);
                    Manager.instance.playerItem.Add(temp);
                    isBuy = true;
                    MoneyEffect(Manager.instance.tempItem.price);
                    Destroy(GameObject.FindGameObjectWithTag("Select"));
                    Manager.instance.selectObject = null;

                    Manager.instance.sellerLabel.text = "";

                    SoundAction.instance.audio.volume = 0.8f;
                    SoundAction.instance.audio.PlayOneShot(SoundAction.instance.clips[3]);
                    return;
                }
            }
        }
    }

    public void Sell()
    {
        if (Manager.instance.inventorySelectObject == null)
        {
            msgState = messageState.nonSelect;
            Message();
            Debug.Log("선택한 아이템이 없습니다.");

            if (Manager.instance.selectObject != null)
            {
                Destroy(GameObject.FindGameObjectWithTag("Select"));
                Manager.instance.selectObject = null;
                Manager.instance.sellerLabel.text = "";
            }
        }
        else
        {
            foreach (Transform slotList in Manager.instance.inventorySlot)
            {
                if (slotList.childCount > 0 && slotList.GetChild(0).transform == Manager.instance.inventorySelectObject)
                {
                    Manager.instance.playerSlotCount--;
                    Manager.instance.possessionItem.Remove(slotList.GetChild(0).gameObject);
                    Manager.instance.playerItem.Remove(slotList.GetChild(0).GetComponent<Item>());
                    Destroy(slotList.GetChild(0).gameObject);
                    Manager.instance.inventorySelectObject = null;
                    Manager.instance.curMoney += slotList.GetChild(0).GetComponent<Item>().itemData.price;
                    isBuy = false;
                    MoneyEffect(slotList.GetChild(0).GetComponent<Item>().itemData.price);
                    SoundAction.instance.audio.volume = 0.8f;
                    SoundAction.instance.audio.PlayOneShot(SoundAction.instance.clips[3]);
                    break;
                }
            }
        }
    }

    void MoneyEffect(int money)
    {
        moneyEffect.SetActive(true);
        UILabel label = moneyEffect.GetComponent<UILabel>();

        if (isBuy)
        {
            label.gradientTop = Color.red;
            label.gradientBottom = Color.red;
            label.text = string.Format("-{0}", money);
        }
        else
        {
            label.gradientTop = Color.green;
            label.gradientBottom = Color.green;
            label.text = string.Format("+{0}", money);
        }

        TweenPosition twPositon = moneyEffect.GetComponent<TweenPosition>();
        twPositon.PlayForward();
        TweenAlpha twAlpha = moneyEffect.GetComponent<TweenAlpha>();
        twAlpha.PlayForward();
        twPositon.ResetToBeginning();
        twAlpha.ResetToBeginning();
    }

    void Message()
    {
        SoundAction.instance.audio.volume = 0.5f;
        SoundAction.instance.audio.PlayOneShot(SoundAction.instance.clips[2]);
        message.SetActive(true);
        block.SetActive(true);
        UILabel label = message.transform.GetChild(0).GetComponent<UILabel>();

        if (msgState == messageState.nonSelect)
        {
            label.text = "선택된 아이템이\n없습니다!";
        }
        else if (msgState == messageState.minusMoney)
        {
            label.text = "보유한 돈이\n부족합니다!";
        }
        else
        {
            label.text = "가방의 여유공간이\n없습니다!";
        }
        twScale.PlayForward();
    }
}
