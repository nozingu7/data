using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellerItem : MonoBehaviour
{
    public string name;
    public int price;
    public SelectItemData selectItem;
    public PlayerState pState;

    private void Start()
    {
        pState = selectItem.state;
    }

    void OnClick()
    {
        // 이전에 선택한 아이템이 없다면 현재 클릭한 아이템을 셀렉트로 한다.
        if (Manager.instance.selectObject == null)
        {
            Manager.instance.selectObject = this.transform;
            Manager.instance.tempItem = selectItem;
            GameObject selImg = Instantiate(Manager.instance.selectImg);
            selImg.transform.parent = this.transform.parent;
            selImg.transform.localPosition = Vector3.zero;
            selImg.transform.localScale = Vector3.one;
            Manager.instance.sellerLabel.text = string.Format("이름 : {0}\n{1}\n가격 : {2}원", selectItem.name, selectItem.information, selectItem.price);

            // 인벤토리에 선택된 아이템이 있다면, 인벤토리에 선택된 아이템의 셀렉트는 해제한다.
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
        // 이전에 선택한 아이템과 현재 클릭한 아이템이 같다면 셀렉트만 해제한다.
        else if (Manager.instance.selectObject.transform == gameObject.transform)
        {
            Destroy(GameObject.FindGameObjectWithTag("Select"));
            Manager.instance.selectObject = null;
            Manager.instance.sellerLabel.text = "";
        }
        // 이전에 선택한 아이템이 있다면, 현재 클릭한 아이템으로 셀렉트를 변경한다.
        else if (Manager.instance.selectObject != null)
        {
            Destroy(GameObject.FindGameObjectWithTag("Select"));
            Manager.instance.selectObject = null;

            Manager.instance.selectObject = this.transform;
            Manager.instance.tempItem = selectItem;
            GameObject selImg = Instantiate(Manager.instance.selectImg);
            selImg.transform.parent = this.transform.parent;
            selImg.transform.localPosition = Vector3.zero;
            selImg.transform.localScale = Vector3.one;
            Manager.instance.sellerLabel.text = string.Format("이름 : {0}\n{1}\n가격 : {2}원", selectItem.name, selectItem.information, selectItem.price);
        }
    }
}
